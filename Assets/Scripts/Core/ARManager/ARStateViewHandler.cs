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

    public bool IsReady { get; private set; }

    void Awake()
    {
        if (surfaceObj != null)
        {
            surfaceBinders = surfaceObj.GetComponentsInChildren<SurfaceItemBinder>(true);
        }

        IsReady = true;
    }

    public void ApplyState(AppState state, bool isVisible, GameObject currentFocus)
    {

        if (surfaceObj == null || hardwareObj == null)
        {
            Debug.LogError("Surface/Hardware object not assigned");
            return;
        }

        if (!isVisible)
        {
            surfaceObj.SetActive(false);
            hardwareObj.SetActive(false);

            if (focusObj != null)
            {
                foreach (Transform child in focusObj.transform)
                    child.gameObject.SetActive(false);
            }

            uIManager?.ShowMainUI(false);
            uIManager?.ShowFocusUI(false);

            return;
        }

        surfaceObj.SetActive(state == AppState.SURFACE_STATE);
        hardwareObj.SetActive(state == AppState.HARDWARE_STATE);

        if (focusObj != null)
        {
            foreach (Transform child in focusObj.transform)
                child.gameObject.SetActive(false);
        }

        if (state == AppState.SURFACE_STATE)
        {
            if (surfaceBinders == null || dataRegistry == null)
                return;

            foreach (var binder in surfaceBinders)
            {
                if (binder != null)
                    binder.Bind(dataRegistry);
            }
        }

        if (state == AppState.FOCUS_STATE && currentFocus != null)
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
            uIManager.ShowMainUI(state != AppState.FOCUS_STATE);
            uIManager.ShowFocusUI(state == AppState.FOCUS_STATE);
        }

        worldTitleDisplay?.Refresh();
    }
}