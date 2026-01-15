using System;

[Serializable]
public class PlayerData
{
	public int id;
	public int coins;
	public int user_id;
	public int avatar_id;

}

[Serializable]
public class PlayerCreationData
{
    // These keys match the properties destructured from req.body
    public string name;
    public string email;
    public string password;
    public int avatar_id;
	
	public PlayerCreationData(string name, string email, string password, int avatar_id)
	{
	    this.name = name;
        this.email = email;
        this.password = password;
        this.avatar_id = avatar_id;
	}
}
