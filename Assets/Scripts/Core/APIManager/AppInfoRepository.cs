using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

public class AppInfoRepository : MonoBehaviour
{
    public APIService apiService;
    public string endPoint;

    private string cachedTitle;

    private float lastFetchTime = -10f;
    public float cooldown = 5f;
    private bool isLoading = false;

    public IEnumerator FetchTitle(bool force = false, Action<bool> onDone = null)
    {
        if (isLoading)
        {
            onDone?.Invoke(false);
            yield break;
        }

        if (!force && Time.time - lastFetchTime < cooldown)
        {
            onDone?.Invoke(false);
            yield break;
        }

        isLoading = true;
        lastFetchTime = Time.time;

        yield return apiService.Get(endPoint, (json, success) =>
        {
            if (!success)
            {
                isLoading = false;
                onDone?.Invoke(false);
                return;
            }

            try
            {
                TitleResponse res = JsonConvert.DeserializeObject<TitleResponse>(json);

                if (res == null || string.IsNullOrEmpty(res.data))
                {
                    Debug.LogError("TitleResponse invalid");
                    onDone?.Invoke(false);
                    return;
                }

                cachedTitle = res.data;
                onDone?.Invoke(true);
            }
            catch (Exception e)
            {
                Debug.LogError("Title parse error: " + e.Message);
                onDone?.Invoke(false);
            }

            isLoading = false;
        });
    }

    public string GetTitle()
    {
        return cachedTitle;
    }

    public bool HasTitle()
    {
        return !string.IsNullOrEmpty(cachedTitle);
    }

    public bool CanFetch()
    {
        if (isLoading) return false;
        if (Time.time - lastFetchTime < cooldown) return false;
        return true;
    }
}