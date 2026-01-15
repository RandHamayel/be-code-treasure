using System;

[Serializable]
public class PlayerInfo
{
    public int id;
    public int coins;
    public int user_id;
    public int avatar_id;
}

[Serializable]
public class UserData
{
    public int id;
    public string name;
    public string email;
    public string user_type;
    public int is_deleted;
    public PlayerInfo player; // Maps the nested player object
}

[Serializable]
public class UserListWrapper
{
    // The keys "players" and "admins" match your backend res.json()
    public UserData[] players;
    public UserData[] admins;
}

[Serializable]
public class CreateUserRequest
{
    public string name;
    public string email;
    public string password;
    public int avatar_id;

    // Constructor for Admins
    public CreateUserRequest(string n, string e, string p)
    {
        name = n; email = e; password = p;
    }

    // Constructor for Players
    public CreateUserRequest(string n, string e, string p, int aId)
    {
        name = n; email = e; password = p; avatar_id = aId;
    }
}
