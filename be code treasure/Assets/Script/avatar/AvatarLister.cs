using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;

public class AvatarLister : MonoBehaviour
{
    public AvatarService avatarService;
    public Transform contentParent;
    public GameObject avatarItemPrefab;

    public void LoadAvatars()
    {
        Clear();

        avatarService.ListAvatars((avatars) =>
        {
            foreach (var avatar in avatars)
            {
                var go = Instantiate(avatarItemPrefab, contentParent);
                var item = go.GetComponent<AvatarItem>();

                item.Setup(avatar);
            }
        },
        (err) => Debug.LogError(err));
    }

    private void Clear()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    }
}