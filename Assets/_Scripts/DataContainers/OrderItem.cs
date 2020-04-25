using System.Collections;
using System.Collections.Generic;

namespace DataContainers {
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
}

