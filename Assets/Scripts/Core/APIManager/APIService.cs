using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIService : MonoBehaviour
{
    public string baseUrl = "http://your-api.com"; // kamu tadi typo "htttp", nice try

    public IEnumerator Get(string endpoint, Action<string, bool> callback)
    {
        string url = baseUrl + endpoint;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 5;

            yield return request.SendWebRequest();

            try
            {
                bool success = request.result == UnityWebRequest.Result.Success;

                if (success)
                {
                    SafeCallback(callback, request.downloadHandler.text, true);
                }
                else
                {
                    Debug.LogWarning($"API Failed: {request.error} | URL: {url}");
                    SafeCallback(callback, null, false);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Unexpected error: " + e.Message);
                SafeCallback(callback, null, false);
            }
        }
    }

    private void SafeCallback(Action<string, bool> callback, string data, bool success)
    {
        try
        {
            callback?.Invoke(data, success);
        }
        catch (Exception e)
        {
            Debug.LogError("Callback error (ini bukan dari API, tapi dari kode kamu): " + e.Message);
        }
    }
}