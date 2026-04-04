using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class APIManager : MonoBehaviour
{
    [Header("Base URL")]
    public string baseUrl = "http://api.com";

    [Header("Endpoints")] 
    public string hardwareEndpoint = "/hardware";
    public string surfaceEndpoint = "/surface";

    [Header("Dependencies")]
    public DataRegistry registry;

    [Header("Cooldown")]
    public float fetchCooldown = 2f;

    private float lastHardwareFetchTime = -10f;
    private float lastSurfaceFetchTime = -10f;

    private bool isFetchingHardware = false;
    private bool isFetchingSurface = false;

    private Dictionary<string, HardwareDataAPI> hardwareCache = new();
    private Dictionary<string, SurfaceDataAPI> surfaceCache = new();

    public IEnumerator FetchHardware(bool force = false, Action<bool> onDone = null)
    {
        if (isFetchingHardware)
            yield break;

        if (!force && Time.time - lastHardwareFetchTime < fetchCooldown)
            yield break;

        isFetchingHardware = true;
        lastHardwareFetchTime = Time.time;

        string url = baseUrl + hardwareEndpoint;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            HardwareDataList list = JsonUtility.FromJson<HardwareDataList>(request.downloadHandler.text);

            hardwareCache.Clear();

            foreach (var item in list.data)
            {
                hardwareCache[item.id] = item;

                var local = registry.GetHardware(item.id);
                if (local != null)
                {
                    local.ApplyFromAPI(item);
                }
                else
                {
                    Debug.LogWarning("Hardware tidak ditemukan di registry: " + item.id);
                }
            }

            Debug.Log("Hardware Loaded: " + hardwareCache.Count);
            onDone?.Invoke(true);
        }
        else
        {
            Debug.LogError("Hardware API Error: " + request.error);
            onDone?.Invoke(false);
        }

        isFetchingHardware = false;
    }

    public IEnumerator FetchSurface(bool force = false, Action<bool> onDone = null)
    {
        if (isFetchingSurface)
            yield break;

        if (!force && Time.time - lastSurfaceFetchTime < fetchCooldown)
            yield break;

        isFetchingSurface = true;
        lastSurfaceFetchTime = Time.time;

        string url = baseUrl + surfaceEndpoint;

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            SurfaceDataList list = JsonUtility.FromJson<SurfaceDataList>(request.downloadHandler.text);

            surfaceCache.Clear();

            foreach (var item in list.data)
            {
                surfaceCache[item.id] = item;

                var local = registry.GetSurface(item.id);
                if (local != null)
                {
                    local.ApplyFromAPI(item);
                }
                else
                {
                    Debug.LogWarning("Surface tidak ditemukan di registry: " + item.id);
                }
            }

            Debug.Log("Surface Loaded: " + surfaceCache.Count);
            onDone?.Invoke(true);
        }
        else
        {
            Debug.LogError("Surface API Error: " + request.error);
            onDone?.Invoke(false);
        }

        isFetchingSurface = false;
    }

    public HardwareDataAPI GetHardwareAPI(string id)
    {
        hardwareCache.TryGetValue(id, out var data);
        return data;
    }

    public SurfaceDataAPI GetSurfaceAPI(string id)
    {
        surfaceCache.TryGetValue(id, out var data);
        return data;
    }

    public bool IsHardwareReady()
    {
        return hardwareCache.Count > 0;
    }

    public bool IsSurfaceReady()
    {
        return surfaceCache.Count > 0;
    }

    public bool IsLoadingHardware()
    {
        return isFetchingHardware;
    }

    public bool IsLoadingSurface()
    {
        return isFetchingSurface;
    }
    public void ClearCache()
    {
        hardwareCache.Clear();
        surfaceCache.Clear();
    }
}