/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public  class ImageLoader
{
     public static ImageLoader Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Load(string url, Image target)
    {
        StartCoroutine(LoadRoutine(url, target));
    }
     private IEnumerator LoadRoutine(string url, Image target)
    {
        using UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Texture2D tex = DownloadHandlerTexture.GetContent(req);
            target.sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
        }
        else
        {
            Debug.LogError("Image load failed: " + req.error);
        }
    }
}*/
