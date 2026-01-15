using TMPro;
using UnityEngine;
using SFB;


public class AvatarView : MonoBehaviour
{
    public AvatarService avatarService;
    [Header("List Settings")]
    public Transform container; // Drag 'Content' of ScrollView here
    public GameObject avatarItemPrefab; // Drag 'AvatarItem' Prefab here

    [Header("Add Avatar Form")]
    public TMP_InputField nameInput;
    private string selectedCharImageUrl;
    private string selectedTreasImageUrl;

    private void Start()
    {
        // Requirement: Avatar list shown immediately
        RefreshList();
    }

    public void OnSelectCharacterImage() {
        var extensions = new [] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg" ) };
        StandaloneFileBrowser.OpenFilePanelAsync("Select Character Image", "", extensions, false, (string[] paths) => {
            if (paths.Length > 0) {
                selectedCharImageUrl = paths[0];
                Debug.Log("Selected Character Image: " + selectedCharImageUrl);
            }
        });
    }

    public void OnSelectTreasureImage(){
    var extensions = new[] { new ExtensionFilter("Image Files", "png", "jpg", "jpeg")};
    StandaloneFileBrowser.OpenFilePanelAsync("Select Treasure Image","",extensions,false,(string[] paths) => {
        if (paths.Length > 0){
            selectedTreasImageUrl = paths[0];
            Debug.Log("Selected Treasure Image: " + selectedTreasImageUrl);
        }
    });
}

    public void RefreshList()
    {

        // Calling the service with your 3 specific parameters
       avatarService.ListAvatars(
        (avatarArray) => {
            Debug.Log($"SUCCESS! Received {avatarArray.Length} avatars.");
            },
            (error) => Debug.LogError(error)
        );


    }

    public void OnCreateAvatarClicked()
{
   // Requirement: Admin can add new avatar
        AvatarCreateData newData = new AvatarCreateData(
        nameInput.text,
        selectedCharImageUrl,
        selectedTreasImageUrl
        );
    // For now, using hardcoded test data

    avatarService.CreateAvatar(newData, // 4. THE DATA (This was missing!)
        () => {                             // 5. onSuccess
            Debug.Log("Avatar Added Successfully!");
            RefreshList(); // Auto-refresh the ScrollView
            ClearInputs();
        },
        (err) => {                          // 6. onError
            Debug.LogError("Add Error: " + err);
        }
    );
}
private void ClearInputs()
{
    nameInput.text = "";
    selectedCharImageUrl = "";
    selectedTreasImageUrl = "";

    Debug.Log("UI Form has been cleared for the next entry.");
}

}
