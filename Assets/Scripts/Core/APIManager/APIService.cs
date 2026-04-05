using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIService : MonoBehaviour
{
    public string baseUrl = "htttp://your-api.com";

    public IEnumerator Get(string endpoint, Action<string, bool> callback)
    {
        string url = baseUrl + endpoint;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            callback?.Invoke(request.downloadHandler.text, true);
        }
        else
        {
            Debug.LogError("API Error: " + request.error);
            callback?.Invoke(null, false);
        }
    }
}