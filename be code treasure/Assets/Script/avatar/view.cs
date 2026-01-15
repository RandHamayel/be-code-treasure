using TMPro;
using UnityEngine;
using SFB;


public class AvatarView : MonoBehaviour
{
    public AvatarService avatarService;
    public FileService fileService;
    [Header("List Settings")]
    public Transform container; // Drag 'Content' of ScrollView here
    public GameObject avatarItemPrefab; // Drag 'AvatarItem' Prefab here

    [Header("Add Avatar Form")]
    public TMP_InputField nameInput;
    private string selectedCharImageUrl;
    private string selectedTreasImageUrl;
    private string uploadedCharImageUrl;
    private string uploadedTreasImageUrl;
    private bool charImageUploaded = false;
    private bool treasImageUploaded = false;

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
        // Check if images are selected
        if (string.IsNullOrEmpty(selectedCharImageUrl) || string.IsNullOrEmpty(selectedTreasImageUrl))
        {
            Debug.LogError("Please select both character and treasure images before creating avatar.");
            return;
        }

        // Reset state
        uploadedCharImageUrl = null;
        uploadedTreasImageUrl = null;
        charImageUploaded = false;
        treasImageUploaded = false;

        // Start both uploads in parallel
        UploadCharacterImage();
        UploadTreasureImage();
    }

    private void UploadCharacterImage()
    {
        string fileName = System.IO.Path.GetFileName(selectedCharImageUrl);
        string contentType = FileService.GetContentType(selectedCharImageUrl);
        fileService.GetPresignedUrl(fileName, contentType, "avatars/character", // optional path
            (presignResponse) => {
                // Got presigned URL, now upload the file
                fileService.UploadFile(presignResponse.upload_url, selectedCharImageUrl,
                    () => {
                        // Upload successful, store the file path
                        uploadedCharImageUrl = presignResponse.file_path;
                        charImageUploaded = true;
                        Debug.Log("Character image uploaded: " + uploadedCharImageUrl);
                        CheckIfBothUploadsComplete();
                    },
                    (error) => {
                        Debug.LogError("Failed to upload character image: " + error);
                    }
                );
            },
            (error) => {
                Debug.LogError("Failed to get presigned URL for character image: " + error);
            }
        );
    }

    private void UploadTreasureImage()
    {
        string fileName = System.IO.Path.GetFileName(selectedTreasImageUrl);
        string contentType = FileService.GetContentType(selectedTreasImageUrl);
        fileService.GetPresignedUrl(fileName, contentType, "avatars/treasure", // optional path
            (presignResponse) => {
                // Got presigned URL, now upload the file
                fileService.UploadFile(presignResponse.upload_url, selectedTreasImageUrl,
                    () => {
                        // Upload successful, store the file path
                        uploadedTreasImageUrl = presignResponse.file_path;
                        treasImageUploaded = true;
                        Debug.Log("Treasure image uploaded: " + uploadedTreasImageUrl);
                        CheckIfBothUploadsComplete();
                    },
                    (error) => {
                        Debug.LogError("Failed to upload treasure image: " + error);
                    }
                );
            },
            (error) => {
                Debug.LogError("Failed to get presigned URL for treasure image: " + error);
            }
        );
    }

    private void CheckIfBothUploadsComplete()
    {
        if (charImageUploaded && treasImageUploaded)
        {
            // Both uploads complete, now create the avatar
            CreateAvatar();
        }
    }

    private void CreateAvatar()
    {
        // Requirement: Admin can add new avatar
        AvatarCreateData newData = new AvatarCreateData(
            nameInput.text,
            uploadedCharImageUrl,
            uploadedTreasImageUrl
        );

        avatarService.CreateAvatar(newData,
            () => {
                Debug.Log("Avatar Added Successfully!");
                RefreshList(); // Auto-refresh the ScrollView
                ClearInputs();
            },
            (err) => {
                Debug.LogError("Add Error: " + err);
            }
        );
    }
private void ClearInputs()
{
    nameInput.text = "";
    selectedCharImageUrl = "";
    selectedTreasImageUrl = "";
    uploadedCharImageUrl = "";
    uploadedTreasImageUrl = "";
    charImageUploaded = false;
    treasImageUploaded = false;

    Debug.Log("UI Form has been cleared for the next entry.");
}

}
