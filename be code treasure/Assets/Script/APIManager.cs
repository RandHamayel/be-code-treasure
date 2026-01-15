using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class APIManager : MonoBehaviour
{
    public static APIManager Instance;
    private string URL;

    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        // This line keeps the APIManager alive when you change scenes
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}

    // This is the "Connection Work" function your Service calls
    public void ExecuteRequest<T>(string url, string method, bool needsToken, object body, Action<T> onSuccess, Action<string> onError)
    {
        URL = AppConstants.API_BASE_URL + url;

        StartCoroutine(RequestCoroutine(URL, method, needsToken, body, onSuccess, onError));

    }

    private IEnumerator RequestCoroutine<T>(string url, string method, bool needsToken, object body, Action<T> onSuccess, Action<string> onError)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            // 1. Handle JSON Body for POST/PUT
            if (body != null)
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(body));
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.SetRequestHeader("Content-Type", "application/json");
            }

            request.downloadHandler = new DownloadHandlerBuffer();

            // 2. Handle Token
            if (needsToken)
            {
                // Professional check: Use the Memory-based AuthSession
             if (!string.IsNullOrEmpty(AuthSession.Token)) {
                 request.SetRequestHeader("Authorization", "Bearer " + AuthSession.Token);
                Debug.Log("Token Attached: " + AuthSession.Token); // Add this to verify in console
            } else {
                Debug.LogError("Token needed but AuthSession.Token is NULL!");
                }

            }

            yield return request.SendWebRequest();

            // 3. Handle Response
            if (request.result == UnityWebRequest.Result.Success)
            {
                // If the expected type is a string (like a simple message), return raw text
                if (typeof(T) == typeof(string))
                {
                    onSuccess?.Invoke((T)(object)request.downloadHandler.text);
                }
                else
                {
                    // Otherwise, deserialize the JSON into our Model
                    T data = JsonUtility.FromJson<T>(request.downloadHandler.text);
                    onSuccess?.Invoke(data);
                }
            }
            else
            {
                onError?.Invoke(request.error + ": " + request.downloadHandler.text);
            }
        }
    }
}
