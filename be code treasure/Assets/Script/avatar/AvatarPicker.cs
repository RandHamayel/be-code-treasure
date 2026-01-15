using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class AvatarPicker : MonoBehaviour
{
    public AvatarService avatarService;
    public Transform contentParent;
    public GameObject avatarItemPrefab;

    private List<AvatarItem> items = new();
    private AvatarItem selectedItem;
    private string selectedAvatarId;

    public void LoadAvatars()
    {
        Clear();

        avatarService.ListAvatars((avatars) =>
        {
            foreach (var avatar in avatars)
            {
                var go = Instantiate(avatarItemPrefab, contentParent);
                var item = go.GetComponent<AvatarItem>();

                bool isSelected = (selectedAvatarId != null && avatar.id == selectedAvatarId);
                item.Setup(avatar, this, isSelected);
                items.Add(item);

                if (isSelected)
                {
                    selectedItem = item;
                }
            }
        },
        (err) => Debug.LogError(err));
    }

    public void SelectAvatar(AvatarItem selectedItem, AvatarData avatar)
    {
        // Clear previous selection
        if (this.selectedItem != null)
        {
            this.selectedItem.SetSelected(false);
        }

        // Set new selection
        this.selectedItem = selectedItem;
        this.selectedItem.SetSelected(true);
        selectedAvatarId = avatar.id;

        Debug.Log("Selected avatar: " + avatar.name + " (ID: " + avatar.id + ")");
    }

    public string GetSelectedAvatarId()
    {
        return selectedAvatarId;
    }

    private void Clear()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        items.Clear();
        selectedItem = null;
        selectedAvatarId = null;
    }
}
