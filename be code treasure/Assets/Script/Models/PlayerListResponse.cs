using System;

[Serializable]
public class UserListResponse
{
    // The name of this array MUST match the key used in your JSON response (e.g., "users")
    public PlayerData[] users;
}
