using System;
using System.Collections;
using UnityEngine;

public class AppInfoRepository : MonoBehaviour
{
    public APIService apiService;

    private string cachedTitle;

    public string endPoint;

    private float lastFetchTime = -10f;
    public float cooldown = 5f;
    private bool isLoading = false;

    public IEnumerator FetchTitle(bool force = false, Action<bool> onDone = null)
    {
        if (isLoading)
            yield break;

        if (!force && Time.time - lastFetchTime < cooldown)
            yield break;

        isLoading = true;
        lastFetchTime = Time.time;

        yield return apiService.Get(endPoint, (json, success) =>
        {
            if (!success)
            {
                onDone?.Invoke(false);
                return;
            }

            TitleResponse res = JsonUtility.FromJson<TitleResponse>(json);
            cachedTitle = res.data;

            onDone?.Invoke(true);
        });

        isLoading = false;
    }

    public string GetTitle()
    {
        return cachedTitle;
    }

    public bool HasTitle()
    {
        return !string.IsNullOrEmpty(cachedTitle);
    }
}