using System.Collections.Generic;
using UnityEngine;

public class DataRegistry : MonoBehaviour
{
    public List<HardwareData> hardwareList;
    public List<SurfaceData> surfaceList;

    private Dictionary<string, HardwareData> hardwareDict = new();
    private Dictionary<string, SurfaceData> surfaceDict = new();

    void Awake()
    {
        foreach (var h in hardwareList)
            hardwareDict[h.id] = h;

        foreach (var s in surfaceList)
            surfaceDict[s.id] = s;
    }


    public HardwareData GetHardware(string id)
    {
        hardwareDict.TryGetValue(id, out var data);
        return data;
    }

    public SurfaceData GetSurface(string id)
    {
        surfaceDict.TryGetValue(id, out var data);
        return data;
    }

}