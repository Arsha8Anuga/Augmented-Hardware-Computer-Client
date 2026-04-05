using System;
using System.Collections.Generic;

[Serializable]
public class SurfaceResponse
{
    public string type;
    public Dictionary<string, SurfaceDataAPI> data;
}