using UnityEngine;
using System;

public class AuthService : MonoBehaviour {

    private const string LOGIN_URL  = "/auth/";
    private const string ME_URL     = "/auth/me";
    private const string LOGOUT_URL = "/auth/logout";
    private const string REGISTER_URL = "/users";

    // 1. LOGIN (POST /)
    public void Login(LoginRequest data, Action<UserData> onSuccess, Action<string> onError) {
        APIManager.Instance.ExecuteRequest<AuthResponse>(LOGIN_URL, "POST", false, data, (res) => {
          // Save token in memory
                AuthSession.Token = res.access_token;
                // Save token persistently
                PlayerPrefs.SetString("access_token", res.access_token);
                PlayerPrefs.Save();
                // Fetch user immediately
                GetMe(onSuccess, onError);
            },
            onError
        );
    }

    // 2. GET CURRENT USER (GET /me)
    public void GetMe(Action<UserData> onSuccess, Action<string> onError) {
        // needsToken is true because of verifyJWT middleware
        APIManager.Instance.ExecuteRequest<UserMeWrapper>(ME_URL, "GET", true, null, (res) => {
            AuthSession.CurrentUser = res.user;
            onSuccess?.Invoke(res.user);
        }, onError);
    }

    // 3. LOGOUT (POST /logout)
    public void Logout(Action onSuccess, Action<string> onError) {
        APIManager.Instance.ExecuteRequest<string>(LOGOUT_URL, "POST", true, null, (res) => {
            AuthSession.Clear();
            PlayerPrefs.DeleteKey("access_token");
            onSuccess?.Invoke();
        }, onError);
    }

    // 4. Register (POST /users)
    public void Register(RegisterRequest data, Action onSuccess, Action<string> onError)
    {
        APIManager.Instance.ExecuteRequest<string>(REGISTER_URL, "POST", false, data, (res) =>{
                // After register → go to login
                onSuccess?.Invoke();
            },
            onError
        );
    }
}
