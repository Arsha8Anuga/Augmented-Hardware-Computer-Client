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

    private bool lastLoadingState = false;

    public void SetCanvasRoot(bool active)
    {
        if (canvasRoot == null) return;

        if (canvasRoot.gameObject.activeSelf == active)
            return;

        canvasRoot.gameObject.SetActive(active);
    }

    public void ShowMainUI(bool show)
    {
        if (mainUI == null) return;

        if (mainUI.activeSelf == show)
            return;

        mainUI.SetActive(show);
    }

    public void ShowFocusUI(bool show)
    {
        if (focusUI == null) return;

        if (focusUI.activeSelf == show)
            return;

        focusUI.SetActive(show);
    }

    public void UpdateInfo(string title, string desc)
    {
        lastTitle = title;
        lastDesc = desc;

        if (titleText != null)
            titleText.text = string.IsNullOrEmpty(title) ? "-" : title;

        if (descriptionText != null)
            descriptionText.text = string.IsNullOrEmpty(desc) ? "-" : desc;
    }

    public void RefreshCurrentView()
    {
        if (titleText != null)
            titleText.text = string.IsNullOrEmpty(lastTitle) ? "-" : lastTitle;

        if (descriptionText != null)
            descriptionText.text = string.IsNullOrEmpty(lastDesc) ? "-" : lastDesc;
    }

    public void UpdateModeText(AppState state)
    {
        if (modeText == null) return;

        string mode = state.ToString().Replace("_STATE", "");
        modeText.text = $"Mode: {mode}";
    }

    public void SetLoading(bool isLoading)
    {
        Debug.Log("Loading: " + isLoading);

        if (lastLoadingState == isLoading)
            return;

        lastLoadingState = isLoading;

        if (loadingUI != null)
            loadingUI.SetActive(isLoading);
    }
}