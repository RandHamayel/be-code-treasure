using UnityEngine;
using System;

public class UserService : MonoBehaviour
{
    private const string GetCreatePlayers_URL = "/users";
    private const string GetCraeteAdmins_URL = "/users/admins";
    private const string DeleteUser_URL = "users/:id";

    //public CharacterSelection avatarUI;
    public void GetPlayers(Action<UserData[]> onSuccess, Action<string> onError)
    {
        // GET / requires verifyJWT and admin middleware
        APIManager.Instance.ExecuteRequest<UserListWrapper>(GetCreatePlayers_URL, "GET", true, null,
            (res) => onSuccess?.Invoke(res.players), onError);
    }

    public void GetAdmins(Action<UserData[]> onSuccess, Action<string> onError)
    {
        // GET /admins requires verifyJWT and admin middleware
        APIManager.Instance.ExecuteRequest<UserListWrapper>(GetCraeteAdmins_URL, "GET", true, null,
            (res) => onSuccess?.Invoke(res.admins), onError);
    }

    public void CreatePlayer(CreateUserRequest data, Action onSuccess, Action<string> onError)
    {
        // POST / is public for player registration
        APIManager.Instance.ExecuteRequest<string>(GetCreatePlayers_URL, "POST", false, data,
            (res) => onSuccess?.Invoke(), onError);
    }

    public void CreateAdmin(CreateUserRequest data, Action onSuccess, Action<string> onError)
    {
        // POST /admins requires admin rights
        APIManager.Instance.ExecuteRequest<string>(GetCraeteAdmins_URL, "POST", true, data,
            (res) => onSuccess?.Invoke(), onError);
    }

    public void DeleteUser(int userId, Action onSuccess, Action<string> onError)
    {
        // DELETE /:id performs a soft delete (is_deleted = 1)
        APIManager.Instance.ExecuteRequest<string>(DeleteUser_URL, "DELETE", true, null,
            (res) => onSuccess?.Invoke(), onError);
    }
}
