using TMPro;
using UnityEngine;

public class WorldTitleDisplay : MonoBehaviour
{
    public TextMeshPro text;

    private string lastTitle;

    public void SetTitle(string title)
    {
        lastTitle = title;

        if (text != null)
            text.text = title;
    }

    public void Refresh()
    {
        if (!string.IsNullOrEmpty(lastTitle) && text != null)
            text.text = lastTitle;
    }
}