using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataContainers
{
    [System.Serializable]
    [SerializeField]
    public class Order
    {
        public Dictionary<string, OrderUser> activeUsers;
        public string id;
        public string owner;
        public string restaurant;
        public string table;
        public Dictionary<string, Suborder> suborders;
        public Dictionary<string, JoinRequest> joinRequests;
        public Dictionary<string, TransferRequest> transferRequests;

        public bool Loaded() 
        {
            if (AreJoinRequestsLoaded() && AreTransferRequestsLoaded() && AreUsersLoaded())
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
            foreach (var suborder in suborders)
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
            if (suborders.ContainsKey(iUserId))
            {
                foreach (var item in suborders[iUserId].items)
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

        //public Order(JSONObject iJson)
        //{
        //    activeUsers = new Dictionary<string, OrderUser>();
        //    suborders = new Dictionary<string, Suborder>();
        //    joinRequests = new Dictionary<string, JoinRequest>();
        //    transferRequests = new Dictionary<string, TransferRequest>();
        //    foreach (var item in iJson["activeUsers"].keys)
        //    {
        //        activeUsers.Add(item, new OrderUser (item, iJson["activeUsers"][item].str));
        //        if (iJson["activeUsers"][item].str.Equals("active"))
        //        {
        //            suborders.Add(item, new Suborder(iJson["suborders"][item]));
        //        }
        //    }

        //    id = iJson["id"].str;
        //    owner = iJson["owner"].str;
        //    restaurant = iJson["restaurant"].str;
        //    table = iJson["table"].str;
        //    if (iJson["joinRequests"] != null)
        //    {
        //        foreach (var item in iJson["joinRequests"].keys)
        //        {
        //            joinRequests.Add(item, new JoinRequest(restaurant, table, item));
        //        }
        //    }
        //    if (iJson["transferRequests"] != null)
        //    {
        //        foreach (var item in iJson["transferRequests"].keys)
        //        {
        //            transferRequests.Add(item, new TransferRequest(restaurant, table, item));
        //        }
        //    }

        //}

        public Order(string iId, string iTable, string iRestaurantId)
        {
            activeUsers = new Dictionary<string, OrderUser>();
            suborders = new Dictionary<string, Suborder>();
            joinRequests = new Dictionary<string, JoinRequest>();
            transferRequests = new Dictionary<string, TransferRequest>();
            id = iId;
            owner = UserController.Instance.user.id;
            restaurant = iRestaurantId;
            table = iTable;
            activeUsers.Add(UserController.Instance.user.id, 
                new OrderUser(UserController.Instance.user, "active"));
            suborders.Add(UserController.Instance.user.id,
                new Suborder(new Dictionary<string, OrderItem>(), "open"));
        }
        public override string ToString()
        {
            string s = "Owner: " + owner + " Restaurant: " + restaurant + " Table: " + table + " Id: " + id;
            foreach (var item in suborders)
            {
                s += "\nSuborder: " + item.Key + "\n" + item.Value.ToString();
            }
            return s; 
        }
    }

    [System.Serializable]
    [SerializeField]
    public class Suborder
    {
        public Dictionary<string, OrderItem> items;
        public string status;

        public Suborder(JSONObject iJson)
        {
            status = iJson["status"].str;
            items = new Dictionary<string, OrderItem>();
            if (iJson["items"] != null)
            {
                foreach (var item in iJson["items"].list)
                {
                    if (item == null || item.ToString().Equals("null"))
                    {
                        continue;
                    }
                    items.Add(item["id"].ToString(), new OrderItem(item));
                }
            }
        }

        public Suborder(Dictionary<string, OrderItem> iItems, string iStatus)
        {
            items = iItems;
            status = iStatus;
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

    [System.Serializable]
    [SerializeField]
    public class OrderItem
    {
        public string name;
        public int amount;
        public float price;
        public string id;
        public int ordered;
        public int delivered;
        public int transfered;

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

        public OrderItem(Firebase.Database.DataSnapshot sn)
        {
            OrderItem oi = JsonUtility.FromJson<DataContainers.OrderItem>(sn.GetRawJsonValue());

            string p = sn.Child("price").Value.ToString().Replace(",", ".");

            name = oi.name;
            price = float.Parse(p);
            id = oi.id;
            ordered = oi.ordered;
            delivered = oi.delivered;
            transfered = oi.transfered;
            amount = ordered + delivered + transfered;
        }

        public OrderItem()
        {
            ordered = 0;
            delivered = 0;
            transfered = 0;
        }

        public OrderItem(int iDelivered, string iId)
        {
            id = iId;
            name = UserController.Instance.menu.foods[iId].name;
            price = UserController.Instance.menu.foods[iId].price;
            delivered = iDelivered;
            transfered = 0;
            ordered = 0;
        }


        public override string ToString()
        {
            string s = "Name: " + name + " amount: " + amount + " price: " + price + " Id: " + id + " ordered: " + ordered + " delivered: " + delivered + " transfered: " + transfered;
            return s;
        }
    }

    [System.Serializable]
    [SerializeField]
    public class OrderUser
    {
        public string status;
        public User user;

        //public OrderUser(string UserId, string iStatus)
        //{
        //    status = iStatus;
        ////    RestaurantController.Instance.StartCoroutine(RestController.Instance.GetUser(UserId, setUser, false));
        //}
        public OrderUser(DataContainers.User iUser, string iStatus)
        {
            status = iStatus;
            user = iUser;
        }

        public void setUser(User u)
        {
            user = u;   
        }

    }

    [System.Serializable]
    [SerializeField]
    public class JoinRequest
    {
        public string aprover;
        public string requirer;
        public string id;
        public bool loaded;

        

        //public JoinRequest(string restaurantId, string tableId, string requestId)
        //{
        //    loaded = false;
        ////    RestaurantController.Instance.StartCoroutine(RestController.Instance.GetJoinRequest(requestId, restaurantId, tableId, setJoinRequest));
        //}

        public void setJoinRequest(JSONObject iJson) 
        {
            id = iJson["id"].str;
            requirer = iJson["requirer"].str;
            aprover = iJson["aprover"].str;
            loaded = true;
        }
    }

    [System.Serializable]
    [SerializeField]
    public class TransferRequest
    {
        public string aprover;
        public string requirer;
        public string id;
        public transferItem item;
        public bool loaded;

        //public TransferRequest(string restaurantId, string tableId, string requestId)
        //{
        //    loaded = false;
        ////    RestaurantController.Instance.StartCoroutine(RestController.Instance.GetTransferRequest(requestId, restaurantId, tableId,  setTransferRequest));
        //}

        public void setTransferRequest(JSONObject iJson)
        {
            id = iJson["id"].str;
            requirer = iJson["requirer"].str;
            aprover = iJson["aprover"].str;
            item = new transferItem(iJson["item"]["id"].n.ToString(), int.Parse( iJson["item"]["amount"].ToString()));
            loaded = true;
        }
    }

    [SerializeField]
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

