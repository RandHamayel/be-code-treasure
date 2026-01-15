using System;

[Serializable]
public class PresignRequest
{
    public string file_name;
    public string content_type;
    public string path; // optional pre path

    public PresignRequest(string fileName, string contentType, string path = null)
    {
        this.file_name = fileName;
        this.content_type = contentType;
        this.path = path;
    }
}

[Serializable]
public class PresignResponse
{
    public string upload_url;
    public string file_path;
}
