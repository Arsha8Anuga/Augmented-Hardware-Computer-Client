using System;
using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

public class HardwareRepository : MonoBehaviour
{
    public APIService apiService;
    public DataRegistry registry;
    public string endPoint;

    private DataCache<HardwareDataAPI> cache = new();

    private float lastFetchTime = -10f;
    public float cooldown = 2f;
    private bool isLoading = false;

    public IEnumerator Fetch(bool force = false, Action<bool> onDone = null)
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
                HardwareResponse response = JsonConvert.DeserializeObject<HardwareResponse>(json);

                if (response?.data == null)
                {
                    Debug.LogError("HardwareResponse invalid");
                    isLoading = false;
                    onDone?.Invoke(false);
                    return;
                }

                cache.Clear();

                foreach (var pair in response.data)
                {
                    string id = pair.Key;
                    var item = pair.Value;

                    item.id = id;

                    cache.Set(id, item);

                    var local = registry.GetHardware(id);
                    if (local != null)
                    {
                        local.ApplyFromAPI(item);
                    }
                    else
                    {
                        Debug.LogWarning($"Hardware tidak ditemukan di registry: {id}");
                    }
                }

                onDone?.Invoke(true);
            }
            catch (Exception e)
            {
                Debug.LogError("Hardware deserialize error: " + e.Message);
                onDone?.Invoke(false);
            }

            isLoading = false;
        });
    }

    public HardwareDataAPI Get(string id)
    {
        return cache.Get(id);
    }

    public bool IsReady()
    {
        return cache.HasData();
    }

    public bool CanFetch()
    {
        if (isLoading) return false;
        if (Time.time - lastFetchTime < cooldown) return false;
        return true;
    }
}