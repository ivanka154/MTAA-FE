using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataContainers
{
    public class Order
    {
        public List<string> activeUsers;
        public string id;
        public string owner;
        public string restaurant;
        public string table;
        public Dictionary<string, Subborder> subborders;

        public Dictionary<string, OrderItem> GetAllItems(Menu iMenu)
        {
            Dictionary<string, OrderItem> items = new Dictionary<string, OrderItem>();
            foreach (var suborder in subborders)
            {
                foreach (var item in suborder.Value.items)
                {
                    if (items.ContainsKey(item.Key))
                    {
                        items[item.Key].amount += item.Value.amount;
                        items[item.Key].price += item.Value.amount * iMenu.foods[item.Key].price;
                    }
                    else
                    {
                        items.Add(item.Key, new OrderItem());
                        items[item.Key].amount = item.Value.amount;
                        items[item.Key].id = item.Value.id;
                        items[item.Key].price = item.Value.amount * iMenu.foods[item.Key].price;
                        items[item.Key].name = iMenu.foods[item.Key].name;
                    }
                }
            }
            return items;
        }

        public Order(JSONObject iJson)
        {
            Debug.Log(iJson["activeUsers"].ToString());
            activeUsers = new List<string>();
            subborders = new Dictionary<string, Subborder>();
            foreach (var item in iJson["activeUsers"].keys)
            {
                activeUsers.Add(item);
                subborders.Add(item, new Subborder(iJson["suborders"][item]));
            }
            id = iJson["id"].str;
            owner = iJson["owner"].str;
            restaurant = iJson["restaurant"].str;
            table = iJson["table"].str;

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
                    Debug.Log(item.ToString());

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

   
}

