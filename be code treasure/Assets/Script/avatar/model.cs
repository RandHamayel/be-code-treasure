using System;

[Serializable]
public class AvatarData
{
    public string id;
    public string name;
    // Matching the JSON keys exactly:
    public string character_image;
    public string treasure_image;
}

[Serializable]
public class AvatarListWrapper
{
    // This MUST match the "avatars" key in your JSON
    public AvatarData[] avatars;
}

[Serializable]
public class AvatarCreateData
{
    public string name;
    public string character_image;
    public string treasure_image;

    public AvatarCreateData(string n, string charImg, string treasImg)
    {
        this.name = n;
        this.character_image = charImg;
        this.treasure_image = treasImg;
    }
}
