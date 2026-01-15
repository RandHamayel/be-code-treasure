using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AvatarItem : MonoBehaviour
{
    public Image avatarImage;
    public TMP_Text nameText;
    public GameObject selectionBorder;

    private AvatarData avatar;
    private AvatarPicker picker;

    public void Setup(AvatarData data, AvatarPicker pickerRef)
    {
        avatar = data;
        picker = pickerRef;

        nameText.text = data.name;
        selectionBorder.SetActive(false);
    }

    public void OnClick()
    {
        picker.SelectAvatar(this, avatar);
    }

    public void SetSelected(bool selected)
    {
        selectionBorder.SetActive(selected);
    }

    public Image GetImage()
    {
        return avatarImage;
    }
}
