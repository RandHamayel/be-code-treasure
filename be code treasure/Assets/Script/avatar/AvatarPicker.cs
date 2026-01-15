using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AvatarPicker : MonoBehaviour
{
    public AvatarService avatarService;
    public AuthView authView;
    public Transform contentParent;
    public GameObject avatarItemPrefab;

    private List<AvatarItem> items = new();

    public void LoadAvatars()
    {
        Clear();

        avatarService.ListAvatars((avatars) =>
        {
            foreach (var avatar in avatars)
            {
                var go = Instantiate(avatarItemPrefab, contentParent);
                var item = go.GetComponent<AvatarItem>();

                item.Setup(avatar, this);
                items.Add(item);

                StartCoroutine(LoadImage(avatar.character_image, item.GetImage()));
            }
        },
        (err) => Debug.LogError(err));
    }

    public void SelectAvatar(AvatarItem selectedItem, AvatarData avatar)
    {
        foreach (var item in items)
            item.SetSelected(false);

        selectedItem.SetSelected(true);
        authView.selectedAvatarId = avatar.id;
    }

    private void Clear()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        items.Clear();
    }

  private IEnumerator LoadImage(string rawPath, Image targetImage)
{
    string finalUrl = rawPath;

    // Case 1: URL-encoded full URL inside a path
    if (rawPath.Contains("http%3A") || rawPath.Contains("https%3A"))
    {
        int index = rawPath.IndexOf("http");
        string encodedUrl = rawPath.Substring(index);

        finalUrl = Uri.UnescapeDataString(encodedUrl);
    }
    // Case 2: Proper absolute URL
    else if (Uri.IsWellFormedUriString(rawPath, UriKind.Absolute))
    {
        finalUrl = rawPath;
    }
    // Case 3: Relative path
    else
    {
        finalUrl = AppConstants.API_BASE_URL.TrimEnd('/') + "/" + rawPath.TrimStart('/');
    }

    Debug.Log("FINAL image URL: " + finalUrl);

    using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(finalUrl))
    {
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Image load failed: " + finalUrl);
            yield break;
        }

        Texture2D tex = DownloadHandlerTexture.GetContent(req);

        Sprite sprite = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f)
        );

        targetImage.sprite = sprite;
    }
}


}
