using System;

[Serializable]
public class LoginRequest {
    public string email;
    public string password;
    public LoginRequest(string e, string p) { email = e; password = p; }
}

[Serializable]
public class RegisterRequest
{
    public string name;
    public string email;
    public string password;
    public string avatar_id;

    public RegisterRequest(string name, string email, string password, string avatarId)
    {
        this.name = name;
        this.email = email;
        this.password = password;
        this.avatar_id = avatarId;
    }
}

[Serializable]
public class AuthResponse {
    public string access_token; // Matches res.json({access_token: accessToken})
}

[Serializable]
public class UserMeWrapper {
    public UserData user; // Matches res.status(200).json({ user: userWithoutSensitiveData })
}

// SECURE SESSION (Memory only)
public static class AuthSession {
    public static string Token { get; set; }
    public static UserData CurrentUser { get; set; }
    public static bool IsLoggedIn => !string.IsNullOrEmpty(Token);

    public static void Clear() {
        Token = null;
        CurrentUser = null;
    }
}
