using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Root")]
    public GameObject mainUI;
    public GameObject focusUI;

    [Header("Focus UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI modeText;

    public void ShowMainUI()
    {
        mainUI.SetActive(true);
        focusUI.SetActive(false);
    }
    public void ShowFocusUI()
    {
        mainUI.SetActive(false);
        focusUI.SetActive(true);
    }
    public void UpdateInfo(string title, string desc)
    {
        titleText.text = title;
        descriptionText.text = desc;
    }   

    public void UpdateModeText(AppState state)
    {
        if(state == AppState.SURFACE_STATE)
        {
            modeText.text = "Mode: Surface";
        }
        else if(state == AppState.HARDWARE_STATE)
        {
            modeText.text = "Mode: Hardware";
        }
    }   
}
