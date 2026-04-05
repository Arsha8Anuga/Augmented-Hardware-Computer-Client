using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Root")]
    public Canvas canvasRoot;
    public GameObject mainUI;
    public GameObject focusUI;

    [Header("Loading")]
    public GameObject loadingUI; 

    [Header("Focus UI")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI modeText;

    [Header("Interaction Control")]
    public CanvasGroup interactionGroup;
    

    private string lastTitle;
    private string lastDesc;

    public void SetCanvasRoot(bool active)
    {
        if (canvasRoot != null)
        {
            canvasRoot.gameObject.SetActive(active);
        }
    }

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
        lastTitle = title;
        lastDesc = desc;

        titleText.text = title;
        descriptionText.text = desc;
    }

    public void RefreshCurrentView()
    {
        if (!string.IsNullOrEmpty(lastTitle))
        {
            titleText.text = lastTitle;
            descriptionText.text = lastDesc;
        }
    }

    public void UpdateModeText(AppState state)
    {
        if (state == AppState.SURFACE_STATE)
        {
            modeText.text = "Mode: Surface";
        }
        else if (state == AppState.HARDWARE_STATE)
        {
            modeText.text = "Mode: Hardware";
        }
        else if (state == AppState.FOCUS_STATE)
        {
            modeText.text = "Mode: Focus";
        }
    }

    public void SetLoading(bool isLoading)
    {
        if (loadingUI != null)
        {
            loadingUI.SetActive(isLoading);
        }

        if (interactionGroup != null)
        {
            interactionGroup.interactable = !isLoading;
            interactionGroup.blocksRaycasts = !isLoading;
        }
    }
}