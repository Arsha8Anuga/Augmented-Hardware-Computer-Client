using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceData", menuName = "Surface/Data")]
public class SurfaceData : ScriptableObject
{
    [Header("Field")]
    public string id;
    public string title;
    public string description;

    public void ApplyFromAPI(SurfaceDataAPI api)
    {
        title = api.title;
        description = api.description;
    }
}
