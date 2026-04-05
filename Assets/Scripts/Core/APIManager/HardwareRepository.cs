using System;
using System.Collections;
using System.Net;
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
            yield break;
        }

        if(!force && Time.time - lastFetchTime < cooldown)
        {
            yield break;
        }

        isLoading = true;
        lastFetchTime = Time.time;

        yield return apiService.Get(endPoint, (json, success) =>
        {
            if (!success)
            {
                onDone?.Invoke(false);
                return;
            }

            HardwareDataList list = JsonUtility.FromJson<HardwareDataList>(json);

            cache.Clear();

            foreach (var item in list.data)
            {
                cache.Set(item.id, item);

                var local = registry.GetHardware(item.id);
                if(local != null)
                {
                    local.ApplyFromAPI(item);
                }
            }

            onDone?.Invoke(true);
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
}