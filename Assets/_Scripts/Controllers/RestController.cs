using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
public class RestController : MonoBehaviour
{
    public delegate void MenuLoaded(DataContainers.Menu iMenu);
    public static MenuLoaded OnMenuLoaded;

    public delegate void UserLogedIn(DataContainers.User iUser);
    public static UserLogedIn OnUserLogedIn;

    public delegate void OrderLoaded(DataContainers.Order iOrder);
    public static OrderLoaded OnOrderLoaded;

    private static RestController _instance;
    public static RestController Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", "test1@email.com");
        form.AddField("password", "Alder2020");

        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/helloWorld"))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text.ToString());
            }
        }
    }

    public IEnumerator GetMenu(string restaurantId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/restaurant/getMenu?restaurantID=" + restaurantId))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIViewManager.Instance.ErrorNotification(www.error);
                yield break;
            }
            else
            {
                string s = www.downloadHandler.text.Replace(@"\", "");
                JSONObject json = new JSONObject(s);
                DataContainers.Menu m = new DataContainers.Menu(json["menu"]);
                OnMenuLoaded?.Invoke(m);
            }
        }
    }

    public IEnumerator Register(string name, string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("email", email);
        form.AddField("password", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/user/register", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {

                JSONObject json = new JSONObject(www.downloadHandler.text);
                Debug.Log(json["message"].str);
                UIViewManager.Instance.ErrorNotification(json["message"].str);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                DataContainers.User u = new DataContainers.User(json["user"]);
                UIViewManager.Instance.SuccesNotification(json["message"].str);
                OnUserLogedIn?.Invoke(u);
            }
        }
    }

    public void CorutineStarter(IEnumerator corutine)
    {
        StartCoroutine(corutine);
    }

    public IEnumerator LogIn(string email, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/user/login", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);

                Debug.Log(json["message"].str);
                UIViewManager.Instance.ErrorNotification(json["message"].str);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                DataContainers.User u = new DataContainers.User(json["user"]);
                UIViewManager.Instance.SuccesNotification(json["message"].str);
                OnUserLogedIn?.Invoke(u);
            }
        }
    }

    public IEnumerator GetUser(string iUserId, System.Action<DataContainers.User> action, bool login = false)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/user?userId=" + iUserId))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIViewManager.Instance.ErrorNotification(www.error);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                DataContainers.User u = new DataContainers.User(json["user"]);
                Debug.Log(u.ToString());
                if (login)
                    OnUserLogedIn?.Invoke(u);
                action?.Invoke(u);
            }
        }
    }

    public IEnumerator GetJoinRequest(string iRequestId, string iRestaurantId, string iTableId, System.Action<JSONObject> action)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/joinRequest?requestId=" + iRequestId + "&restaurantId=" + iRestaurantId + "&tableId=" + iTableId))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIViewManager.Instance.ErrorNotification(www.error);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                action?.Invoke(json["request"]);
            }
        }
    }
    public IEnumerator GetTransferRequest(string iRequestId, string iRestaurantId, string iTableId, System.Action<JSONObject> action)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/transferRequest?requestId=" + iRequestId + "&restaurantId=" + iRestaurantId + "&tableId=" + iTableId))
        {
            Debug.Log("http://localhost:5000/transferRequest?requestId=" + iRequestId + "&restaurantId=" + iRestaurantId + "&tableId=" + iTableId);
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIViewManager.Instance.ErrorNotification(www.error);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                action?.Invoke(json["request"]);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(GetTransferRequest("-M5rQ1GPOikqDe-nJWYn", "restID01", "2", setTransferRequest));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(GetJoinRequest("-M49AuPxrKwAcWfHnfZw", "restID01", "2", setTransferRequest));
        }
    }

    public void setTransferRequest(JSONObject iJson)
    {
        Debug.Log("jeee");
    }

    public IEnumerator GetOrder(string restaurantId, string tableId, string orderId)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/order?restaurantId=" + restaurantId + "&tableId=" + tableId + "&orderId=" +orderId))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIViewManager.Instance.ErrorNotification(www.error);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                DataContainers.Order o = new DataContainers.Order(json);
                while (!o.Loaded())
                {
                 //   Debug.Log("yeald");
                    yield return null;
                }
                OnOrderLoaded?.Invoke(o);
            }
        }
    }

    public IEnumerator GetOrderCheckIfUserIsInAndJoin(string restaurantId, string tableId, string orderId, string userId, bool openTableView = false)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/order?restaurantId=" + restaurantId + "&tableId=" + tableId + "&orderId=" + orderId))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIViewManager.Instance.ErrorNotification(www.error);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                DataContainers.Order o = new DataContainers.Order(json);
                if (o.activeUsers.ContainsKey(userId))
                {
                    if (o.activeUsers[userId].status.Equals("active"))
                    {
                        while (!o.Loaded())
                        {
                      //      Debug.Log("yeald");
                            yield return null;
                        }
                        OnOrderLoaded?.Invoke(o);
                        if (openTableView)
                        {
                            UIViewManager.Instance.OpenPanel("TableView");
                        }
                        yield break;
                    }
                    else
                    {
                        UIViewManager.Instance.ErrorNotification("Request allready send");
                    }
                }

            }
        }
    }

    public IEnumerator CreateNewOrder(string restaurantId, string tableId, string userId)
    {
        WWWForm form = new WWWForm();
        form.AddField("restaurantId", restaurantId);
        form.AddField("tableId", tableId);
        form.AddField("userId", userId);
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/order/createNew", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {

                JSONObject json = new JSONObject(www.downloadHandler.text);
                if (www.responseCode == 403)
                {
                    StartCoroutine(GetOrderCheckIfUserIsInAndJoin(restaurantId, tableId, json["order"].str, userId, true));
                }
                Debug.Log(json["message"].str);
                UIViewManager.Instance.ErrorNotification(json["message"].str);
            }
            else
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                DataContainers.Order o = new DataContainers.Order(json);
                Debug.Log(o.ToString());
            }
        }
    }

    public IEnumerator AddNewItemToOrder(string restaurantId, string tableId, string userId, string orderId, Dictionary<string, DataContainers.OrderItem> order)
    {
        WWWForm form = new WWWForm();
        Debug.Log(form.ToString());

        form.AddField("restaurantId", restaurantId);
        form.AddField("tableId", tableId);
        form.AddField("userId", userId);
        form.AddField("orderId", orderId);
        foodsOnOrder fo = new foodsOnOrder(order);

        Debug.Log(JsonUtility.ToJson(fo));
        form.AddField("foods", JsonUtility.ToJson(fo).Replace("'", ""));
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5000/order/addNewItem", form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                JSONObject json = new JSONObject(www.downloadHandler.text);
                Debug.Log(json["message"].str);
                UIViewManager.Instance.ErrorNotification(json["message"].str);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                StartCoroutine(GetOrder(restaurantId, tableId, orderId));
            }
        }
    }
    [System.Serializable]
    class foodsOnOrder 
    {
        [SerializeField]
        public List<DataContainers.transferItem> foods;

        public foodsOnOrder(Dictionary<string, DataContainers.OrderItem> order) 
        {
            foods = new List<DataContainers.transferItem>();
            foreach (var item in order.Values)
            {
                foods.Add(new DataContainers.transferItem(item.id, item.amount));
            }
            Debug.Log("---");

            Debug.Log(JsonUtility.ToJson(foods));

        }
    }



}
