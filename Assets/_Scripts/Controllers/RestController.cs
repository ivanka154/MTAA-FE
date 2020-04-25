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

    

    public IEnumerator GetMenu()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/restaurant/getMenu?restaurantID=restID00"))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                UIViewManager.Instance.ErrorNotification(www.error);
            }
            else
            {
                string s = www.downloadHandler.text.Replace(@"\", "");
                JSONObject json = new JSONObject(s);
                DataContainers.Menu m = new DataContainers.Menu(json["menu"]);

                foreach (var item in m.foods)
                {
                    Debug.Log(item.Value.ToString());
                }
                Debug.Log(m.ToString());
                OnMenuLoaded.Invoke(m);
            }
        }
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
                StartCoroutine(GetUser(json["userId"].str));
                UIViewManager.Instance.ErrorNotification(json["message"].str);
            }
        }
    }

    public IEnumerator GetUser(string iUserId)
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
                OnUserLogedIn?.Invoke(u);
            }
        }
    }

    IEnumerator GetOrder()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:5000/order?restaurantId=restID00&tableId=2&orderId=-M49mU6rwU0W7eieqmHb"))
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
                if (o.id.Equals("-M49mU6rwU0W7eieqmHb"))
                {
                    Debug.Log("jeee");
                }
                else
                {
                    if (o.id.Contains("-M49mU6rwU0W7eieqmHb"))
                    {
                        Debug.Log("meh");
                    }
                    else
                    {
                        Debug.Log("shit");
                    }
                }
                Debug.Log(o.ToString());
            }
        }
    }
}
