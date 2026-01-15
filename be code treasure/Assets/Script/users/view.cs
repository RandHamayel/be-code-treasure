using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UserView : MonoBehaviour
{
    public UserService userService;
    public AvatarService avatarService;

    [Header("UI Hierarchy")]
    public Transform container; // Drag 'Content' from UserScrollView here
    public GameObject userItemPrefab; // Drag your 'UserItem' prefab here

    [Header("Create Form (Player/Admin)")]
    public TMP_InputField nameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passInput;
    public TMP_Dropdown avatarDropdown;

    private List<AvatarData> availableAvatars = new List<AvatarData>();

    private void Start()
    {
        // Populate the dropdown immediately so it's ready for Player creation
        PopulateAvatarDropdown();
    }

    // --- REFRESH LOGIC ---

    public void RefreshPlayerList()
    {
        ClearContainer();
        userService.GetPlayers((players) => {
            PopulateUI(players);
        }, (err) => Debug.LogError("Player Load Error: " + err));
    }

    public void RefreshAdminList()
    {
        ClearContainer();
        userService.GetAdmins((admins) => {
            PopulateUI(admins);
        }, (err) => Debug.LogError("Admin Load Error: " + err));
    }

    private void PopulateUI(UserData[] users)
    {
        foreach (var data in users)
        {
            GameObject item = Instantiate(userItemPrefab, container);
            // This assumes you have a UserItem script on the prefab
            //item.GetComponent<UserItem>().Setup(data, this);
        }
    }

    private void ClearContainer()
    {
        if (container == null) return;
        foreach (Transform child in container) Destroy(child.gameObject);
    }

    // --- CHARACTER SELECTION & CREATION ---

    public void PopulateAvatarDropdown()
    {
        // Matches the AvatarService signature we fixed earlier
        avatarService.ListAvatars( (avatars) => {
            availableAvatars = new List<AvatarData>(avatars);
            avatarDropdown.ClearOptions();

            List<string> options = new List<string>();
            foreach (var av in avatars) options.Add(av.name);
            avatarDropdown.AddOptions(options);
        }, (err) => Debug.LogError("Dropdown Error: " + err));
    }

    /*public void OnSubmitPlayer()
    {
       int selectedAvatarId = availableAvatars[avatarDropdown.value].id;
        CreateUserRequest req = new CreateUserRequest(nameInput.text, emailInput.text, passInput.text, selectedAvatarId);

        userService.CreatePlayer(req, () => {
            RefreshPlayerList(); // Refresh players after successful creation
            ClearForm();
        }, (err) => Debug.LogError("Create Player Error: " + err));
    }*/

    private void ClearForm()
    {
        nameInput.text = "";
        emailInput.text = "";
        passInput.text = "";
    }
}
