using UnityEngine;
using System;

public class AvatarService : MonoBehaviour
{
    private const string AVATAR_URL = "/avatars/";
    // Function 1: List Avatars
    public void ListAvatars( Action<AvatarData[]> onSuccess, Action<string> onError)
    {
        // We pass the 3 parameters + callbacks to the APIManager
       APIManager.Instance.ExecuteRequest<AvatarListWrapper>(AVATAR_URL, "GET", false, null,
        (wrapper) => {
            // Check if the wrapper or the list is null for safety
            if (wrapper != null && wrapper.avatars != null)
            {
                onSuccess?.Invoke(wrapper.avatars);
            }
            else
            {
                onError?.Invoke("JSON parsed but avatar list is empty or null.");
            }
        },
        onError
    );
    }

    // Function 2: Create Avatar
    public void CreateAvatar( AvatarCreateData data, Action onSuccess, Action<string> onError)
 {
    // We pass 'data' as the body parameter.
    // We expect a string back (like "user created" or a success message).
    APIManager.Instance.ExecuteRequest<string>(AVATAR_URL, "POST", true, data,
        (response) => {
            Debug.Log("Server Response: " + response);
            onSuccess?.Invoke();
        },
        onError
    );
 }
 public void DeleteUser( Action onSuccess, Action<string> onError)
{

    APIManager.Instance.ExecuteRequest<string>(AVATAR_URL, "DELETE", true, null,
        (res) => {
            Debug.Log($"User soft-deleted successfully.");
            onSuccess?.Invoke();
        },
        onError
    );
}
}
