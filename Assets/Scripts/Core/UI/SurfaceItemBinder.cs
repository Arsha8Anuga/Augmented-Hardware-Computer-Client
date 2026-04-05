using TMPro;
using UnityEngine;

public class SurfaceItemBinder : MonoBehaviour
{
    public string surfaceId;

    public TextMeshPro titleText;
    public TextMeshPro descText;

    private SurfaceData data;

    public void Bind(DataRegistry registry)
    {
        data = registry.GetSurface(surfaceId);

        if(data == null)
        {
            Debug.LogWarning("SurfaceData tidak ditemukan: " + surfaceId);
            return;
        }

        Apply();
    }

    public void Apply()
    {
        if(data == null) return;

        if(titleText != null)
        {
            titleText.text = data.title;
        }

        if (descText != null)
        {
            descText.text = data.description;
        }

    }
}