using System;
using System.Collections.Generic;

[Serializable]
public class HardwareResponse
{
    public string type;
    public Dictionary<string, HardwareDataAPI> data;
}