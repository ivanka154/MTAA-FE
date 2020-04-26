﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataContainers
{
    public class Order
    {
        public Dictionary<string, OrderUser> activeUsers;
        public string id;
        public string owner;
        public string restaurant;
        public string table;
        public Dictionary<string, Subborder> subborders;
        public Dictionary<string, JoinRequest> joinRequests;
        public Dictionary<string, TransferRequest> transferRequests;

        public bool Loaded() 
        {
            if (AreJoinRequestsLoaded() && AreTransferRequestsLoaded() && AreTransferRequestsLoaded())
            {
                return true;
            }
            return false;
        }

        public bool AreUsersLoaded()
        {
            foreach (var item in activeUsers)
            {
                if (item.Value.user == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AreJoinRequestsLoaded()
        {
            foreach (var item in joinRequests)
            {
                if (item.Value.loaded == false)
                {
                    return false;
                }
            }
            return true;
        }
        public bool AreTransferRequestsLoaded()
        {
            foreach (var item in transferRequests)
            {
                if (item.Value.loaded == false)
                {
                    return false;
                }
            }
            return true;
        }

        public Dictionary<string, OrderItem> GetAllItems()
        {
            Dictionary<string, OrderItem> items = new Dictionary<string, OrderItem>();
            foreach (var suborder in subborders)
            {
                foreach (var item in suborder.Value.items)
                {
                    if (items.ContainsKey(item.Key))
                    {
                        items[item.Key].amount += item.Value.amount;
                        items[item.Key].price += item.Value.amount * item.Value.price; // iMenu.foods[item.Key].price;
                    }
                    else
                    {
                        items.Add(item.Key, new OrderItem());
                        items[item.Key].amount = item.Value.amount;
                        items[item.Key].id = item.Value.id;
                        items[item.Key].price = item.Value.amount * item.Value.price;
                        items[item.Key].name = item.Value.name;
                    }
                }
            }
            return items;
        }

        public Dictionary<string, OrderItem> GetUsersItems(string iUserId)
        {
            Dictionary<string, OrderItem> items = new Dictionary<string, OrderItem>();
            if (subborders.ContainsKey(iUserId))
            {
                foreach (var item in subborders[iUserId].items)
                {
                    if (items.ContainsKey(item.Key))
                    {
                        items[item.Key].amount += item.Value.amount;
                        items[item.Key].price += item.Value.amount * item.Value.price; // iMenu.foods[item.Key].price;
                    }
                    else
                    {
                        items.Add(item.Key, new OrderItem());
                        items[item.Key].amount = item.Value.amount;
                        items[item.Key].id = item.Value.id;
                        items[item.Key].price = item.Value.amount * item.Value.price;
                        items[item.Key].name = item.Value.name;
                    }
                }
            }
            return items;
        }

        public Order(JSONObject iJson)
        {
            activeUsers = new Dictionary<string, OrderUser>();
            subborders = new Dictionary<string, Subborder>();
            joinRequests = new Dictionary<string, JoinRequest>();
            transferRequests = new Dictionary<string, TransferRequest>();
            foreach (var item in iJson["activeUsers"].keys)
            {
                activeUsers.Add(item, new OrderUser (item, iJson["activeUsers"][item].str));
                if (iJson["activeUsers"][item].str.Equals("active"))
                {
                    subborders.Add(item, new Subborder(iJson["suborders"][item]));
                }
            }

            id = iJson["id"].str;
            owner = iJson["owner"].str;
            restaurant = iJson["restaurant"].str;
            table = iJson["table"].str;

            foreach (var item in iJson["joinRequests"].keys)
            {
                Debug.Log(item.ToString());
                joinRequests.Add(item, new JoinRequest(restaurant, table, item));
            }
            foreach (var item in iJson["transferRequests"].keys)
            {
                transferRequests.Add(item, new TransferRequest(restaurant, table, item));
            }
        }

        public override string ToString()
        {
            string s = "Owner: " + owner + " Restaurant: " + restaurant + " Table: " + table + " Id: " + id;
            foreach (var item in subborders)
            {
                s += "\nSuborder: " + item.Key + "\n" + item.Value.ToString();
            }
            return s; 
        }
    }

    public class Subborder
    {
        public Dictionary<string, OrderItem> items;
        public string status;

        public Subborder(JSONObject iJson)
        {
            Debug.Log(iJson.ToString());
            status = iJson["status"].str;
            items = new Dictionary<string, OrderItem>();
            if (iJson["items"] != null)
            {
                foreach (var item in iJson["items"].list)
                {
                    if (item == null || item.ToString().Equals("null"))
                    {
                        Debug.Log("jeee");

                        continue;
                    }
                    items.Add(item["id"].ToString(), new OrderItem(item));
                }
            }
        }

        public override string ToString()
        {
            string s = "Status: " + status;
            foreach (var item in items)
            {
                s += "\nItem: " + item.Key + "\n" + item.Value.ToString();
            }
            return s;
        }
    }

    public class OrderItem
    {
        public string name;
        public int amount;
        public float price;
        public string id;
        private int ordered;
        private int delivered;
        private int transfered;

        public OrderItem(JSONObject iJson)
        {
            name = iJson["name"].str;
            var v = iJson["price"];
            price = float.Parse(iJson["price"].str.Replace(',', '.'));
            id = iJson["id"].ToString();
            ordered = (iJson["ordered"] == null) ? 0 : int.Parse(iJson["ordered"].ToString());
            delivered = (iJson["delivered"] == null) ? 0 : int.Parse(iJson["delivered"].ToString());
            transfered = (iJson["transfered"] == null) ? 0 : int.Parse(iJson["transfered"].ToString());
            amount = ordered + delivered + transfered;
        }

        public OrderItem()
        {
            ordered = 0;
            delivered = 0;
            transfered = 0;
        }

        public override string ToString()
        {
            string s = "Name: " + name + " amount: " + amount + " price: " + price + " Id: " + id + " ordered: " + ordered + " delivered: " + delivered + " transfered: " + transfered;
            return s;
        }
    }

    public class OrderUser
    {
        public string status;
        public User user;

        public OrderUser(string UserId, string iStatus)
        {
            status = iStatus;
            RestaurantController.Instance.StartCoroutine(RestController.Instance.GetUser(UserId, setUser, false));
        }

        public void setUser(User u)
        {
            user = u;   
        }

    }

    public class JoinRequest
    {
        public string aprover;
        public string requirer;
        public string id;
        public bool loaded;

        public JoinRequest(string restaurantId, string tableId, string requestId)
        {
            loaded = false;
            RestaurantController.Instance.StartCoroutine(RestController.Instance.GetJoinRequest(requestId, restaurantId, tableId, setJoinRequest));
        }

        public void setJoinRequest(JSONObject iJson) 
        {
            Debug.Log(iJson.ToString());

            id = iJson["id"].str;
            requirer = iJson["requirer"].str;
            aprover = iJson["aprover"].str;
            loaded = true;
        }
    }

    public class TransferRequest
    {
        public string aprover;
        public string requirer;
        public string id;
        public transferItem item;
        public bool loaded;

        public TransferRequest(string restaurantId, string tableId, string requestId)
        {
            loaded = false;
            RestaurantController.Instance.StartCoroutine(RestController.Instance.GetTransferRequest(requestId, restaurantId, tableId,  setTransferRequest));
        }

        public void setTransferRequest(JSONObject iJson)
        {
            Debug.Log(iJson.ToString());
            id = iJson["id"].str;
            requirer = iJson["requirer"].str;
            aprover = iJson["aprover"].str;
            item = new transferItem(iJson["item"]["id"].str, int.Parse( iJson["item"]["amount"].ToString()));
            loaded = true;
        }
    }

    [System.Serializable]
    public class transferItem
    {

        [SerializeField] public string id;
        [SerializeField] public int amount;
        public transferItem(string iId, int iAmont)
        {
            id = iId;
            amount = iAmont;
        }
    }
}

