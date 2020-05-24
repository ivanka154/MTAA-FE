using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShareImageButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField]
    private Image image;

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(async () => {
            var v = await StorageFirebase.Instance.Upload(UserController.Instance.user.id + "/x" + Random.Range(0, 100000).ToString() + ".png", image.sprite.texture.GetRawTextureData());
            if (v)
            {
                UIViewManager.Instance.SuccesNotification("Image succesfully uploaded");
            }
            else
            {
                UIViewManager.Instance.ErrorNotification("Cant upload image");
            }
            UIViewManager.Instance.OpenPanel("TableView");
        });
    }
}
