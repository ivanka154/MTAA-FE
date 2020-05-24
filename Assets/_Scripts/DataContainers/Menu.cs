using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataContainers
{
    [SerializeField]
    [System.Serializable]
    public class Menu
    {
        public Dictionary<string, MenuItem> foods;
        public Menu(JSONObject iJson)
        {
            foods = new Dictionary<string, MenuItem>();
            foreach (var item in iJson.list)
            {
                foods.Add(item["id"].ToString(), new MenuItem(item));
            }
        }

        public Menu()
        {
            foods = new Dictionary<string, MenuItem>();
        }

        override public string ToString()
        {
            string s = "";
            foreach (var item in foods)
            {
                s += item.Key.ToString() + ". " + item.Value.ToString() + "\n";
            }
            return s;
        }
    }

    public class MenuItem
    {
        public string alergens;
        public string description;
        public string id;
        public string name;
        public float price;

        public MenuItem(JSONObject iJson)
        {
            description = iJson["description"].str;
            alergens = iJson["alergens"].str;
            id = iJson["id"].ToString();
            name = iJson["name"].str;
            Debug.Log(iJson["price"].str.Replace(',', '.'));
            price = float.Parse(iJson["price"].str.Replace(',', '.'));
        }

        public MenuItem(Firebase.Database.DataSnapshot sn)
        {
            MenuItem mi = JsonUtility.FromJson<DataContainers.MenuItem>(sn.GetRawJsonValue());

            id = sn.Child("id").Value.ToString();
            name = mi.name;
            description = mi.description;
            alergens = mi.alergens;
            price = mi.price;
        }

        override public string ToString()
        {
            string s = "Description: " + description + " Name: " + name + " Price: " + price.ToString() + " Alergens: " + alergens + " Id: " + id;
            return s;
        }

        public string SaveToString()
        {
            return JsonUtility.ToJson(this);
        }
    }


}

