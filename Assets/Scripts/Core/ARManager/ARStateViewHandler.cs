using UnityEngine;

public class ARStateViewHandler : MonoBehaviour
{
    public GameObject surfaceObj;
    public GameObject hardwareObj;
    public GameObject focusObj;

    public SimpleFadeMove hardwareAnim;
    public SimpleFadeMove focusAnim;

    public WorldTitleDisplay worldTitleDisplay;

    public DataRegistry dataRegistry;

    public UIManager uIManager;

    private SurfaceItemBinder[] surfaceBinders;

    void Start()
    {
        surfaceBinders = surfaceObj.GetComponentsInChildren<SurfaceItemBinder>(true);
    }

    public void ApplyState(AppState state, bool isVisible, GameObject currentFocus)
    {
        surfaceObj.SetActive(isVisible && state == AppState.SURFACE_STATE);
        hardwareObj.SetActive(isVisible && state == AppState.HARDWARE_STATE);

        if(focusObj != null)
        {
            foreach (Transform child in focusObj.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (state == AppState.SURFACE_STATE)
        {
            foreach (var binder in surfaceBinders)
            {
                binder.Bind(dataRegistry);
            }
        }

        if(state == AppState.FOCUS_STATE && currentFocus != null)
        {
            currentFocus.SetActive(true);
            focusAnim?.Play();
        }

        if (state == AppState.HARDWARE_STATE)
        {
            hardwareAnim?.Play();
        }

        if (uIManager != null)
        {
            uIManager.UpdateModeText(state);

            if (state == AppState.FOCUS_STATE)
            {
                uIManager.ShowFocusUI();
            }
            else
            {
                uIManager.ShowMainUI();
            }
        }

        worldTitleDisplay?.Refresh();
    }
}