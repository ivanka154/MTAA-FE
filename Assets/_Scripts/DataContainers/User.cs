using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataContainers
{
    public class User
    {
        public string email;
        public string id;
        public string name;
        public string role;

        public User(JSONObject iJson)
        {
            email = iJson["email"].str;
            role = iJson["role"].str;
            id = iJson["id"].str;
            name = iJson["name"].str;
        }
    }
}
