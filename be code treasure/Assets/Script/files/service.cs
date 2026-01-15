using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class FileService : MonoBehaviour
{
    private const string PRESIGN_URL = "/files/presign";

    // Function to get presigned URL for file upload
    public void GetPresignedUrl(string fileName, string contentType, string path, Action<PresignResponse> onSuccess, Action<string> onError)
    {
        PresignRequest request = new PresignRequest(fileName, contentType, path);

        APIManager.Instance.ExecuteRequest<PresignResponse>(PRESIGN_URL, "POST", true, request,
            (response) => {
                Debug.Log("Presigned URL obtained: " + response.upload_url);
                onSuccess?.Invoke(response);
            },
            onError
        );
    }

    // Function to upload file to presigned URL
    public void UploadFile(string presignedUrl, string filePath, Action onSuccess, Action<string> onError)
    {
        StartCoroutine(UploadFileCoroutine(presignedUrl, filePath, onSuccess, onError));
    }

    private IEnumerator UploadFileCoroutine(string presignedUrl, string filePath, Action onSuccess, Action<string> onError)
    {
        // Read file bytes
        byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

        using (UnityWebRequest request = UnityWebRequest.Put(presignedUrl, fileBytes))
        {
            // Set content type based on file extension
            string contentType = GetContentType(filePath);
            request.SetRequestHeader("Content-Type", contentType);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("File uploaded successfully to: " + presignedUrl);
                onSuccess?.Invoke();
            }
            else
            {
                onError?.Invoke(request.error + ": " + request.downloadHandler.text);
            }
        }
    }

    // Utility method to get content type from file path
    public static string GetContentType(string filePath)
    {
        string extension = System.IO.Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".png": return "image/png";
            case ".jpg":
            case ".jpeg": return "image/jpeg";
            case ".gif": return "image/gif";
            case ".bmp": return "image/bmp";
            default: return "application/octet-stream";
        }
    }
}
