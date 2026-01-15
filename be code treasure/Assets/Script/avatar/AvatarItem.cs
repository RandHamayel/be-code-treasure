using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class AvatarItem : MonoBehaviour
{
    public Image avatarImage; // Character image
    public Image treasureImage; // Treasure image
    public TMP_Text nameText;
    public GameObject selectionBorder; // Optional: only used in picker for selection indication

    private AvatarData avatar;
    private AvatarPicker picker;

    public void Setup(AvatarData data, AvatarPicker pickerRef = null, bool isSelected = false)
    {
        avatar = data;
        picker = pickerRef;

        nameText.text = data.name;
        SetSelected(isSelected);

        // Load images
        StartCoroutine(LoadImage(data.character_image, avatarImage));
        StartCoroutine(LoadImage(data.treasure_image, treasureImage));
    }

    public void OnClick()
    {
        if (picker != null)
        {
            picker.SelectAvatar(this, avatar);
        }
    }

    public void SetSelected(bool selected)
    {
        if (selectionBorder != null)
        {
            selectionBorder.SetActive(selected);
        }
    }

    public Image GetImage()
    {
        return avatarImage;
    }

    public Image GetTreasureImage()
    {
        return treasureImage;
    }

    private IEnumerator LoadImage(string imageUrl, Image targetImage)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
            else
            {
                Debug.LogError("Failed to load image: " + request.error);
            }
        }
    }
}
