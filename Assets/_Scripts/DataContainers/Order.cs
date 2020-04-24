using System.Collections;
using System.Collections.Generic;

namespace DataContainers
{
    public class Order
    {
        public List<string> activeUsers;
        public string id;
        public string owner;
        public string restaurant;
        public int table;
        public Dictionary<string, Subborder> subborders;
    }

    public class Subborder
    {
        public List<OrderItem> items;
        public string status;
    }
}

