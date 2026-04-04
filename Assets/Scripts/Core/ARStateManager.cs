using UnityEngine;
using UnityEngine.InputSystem;

public class ARStateManager : MonoBehaviour
{
    public SimpleFadeMove hardwareAnim;
    public SimpleFadeMove focusAnim;

    [Header("State")]
    public AppState currentState = AppState.SURFACE_STATE;

    [Header("API")]
    public APIManager apiManager;

    [Header("Tracking")]
    public float lostDelay = 3f;
    private float lostTimer = 0f;
    private bool isTargetVisible = true;
    private bool pendingReset = false;

    [Header("Objects")]
    public GameObject surfaceObj;
    public GameObject hardwareObj;
    public GameObject focusObj;

    [Header("Focus")]
    public GameObject currentFocus;

    [Header("UI")]
    public UIManager uIManager;

    void Start()
    {
        SetState(currentState);
    }

    void Update()
    {
        if (!isTargetVisible)
        {
            lostTimer += Time.deltaTime;

            if (lostTimer >= lostDelay)
                pendingReset = true;
        }
        else
        {
            if (pendingReset)
            {
                ResetState();
                pendingReset = false;
            }

            lostTimer = 0f;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SetTargetVisible(true);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SetTargetVisible(false);
    }

    public void SetTargetVisible(bool visible)
    {
        isTargetVisible = visible;

        if (visible)
            lostTimer = 0f;

        surfaceObj.SetActive(visible && currentState == AppState.SURFACE_STATE);
        hardwareObj.SetActive(visible && currentState == AppState.HARDWARE_STATE);
        focusObj.SetActive(visible && currentState == AppState.FOCUS_STATE);

        if (uIManager != null)
            uIManager.SetCanvasRoot(visible);
    }

    public void SwitchMode()
    {
        if (currentState == AppState.SURFACE_STATE)
            SetState(AppState.HARDWARE_STATE);
        else if (currentState == AppState.HARDWARE_STATE)
            SetState(AppState.SURFACE_STATE);
    }

    public void EnterFocus(GameObject focusTarget, HardwareData data)
    {
        if (currentState != AppState.HARDWARE_STATE)
        {
            Debug.LogWarning("Masuk focus hanya dari HARDWARE_STATE");
            return;
        }

        if (apiManager != null && !apiManager.IsHardwareReady())
        {
            Debug.LogWarning("Data API belum siap");
            return;
        }

        currentFocus = focusTarget;

        if (uIManager != null)
        {
            uIManager.UpdateInfo(data.title, data.description);
        }

        SetState(AppState.FOCUS_STATE);
    }

    public void BackFromFocus()
    {
        if (currentState == AppState.FOCUS_STATE)
        {
            currentFocus = null;
            SetState(AppState.HARDWARE_STATE);
        }
    }

    void SetState(AppState newState)
    {
        currentState = newState;

        if (apiManager != null)
        {
            if (newState == AppState.HARDWARE_STATE)
            {
                if (!apiManager.IsLoadingHardware())
                {
                    StartCoroutine(apiManager.FetchHardware(false, (success) =>
                    {
                        if (!success)
                        {
                            Debug.LogWarning("Gagal load hardware");
                            return;
                        }

                        Debug.Log("Hardware ready (callback)");

                        if (uIManager != null)
                            uIManager.RefreshCurrentView();
                    }));
                }
            }

            if (newState == AppState.SURFACE_STATE)
            {
                if (!apiManager.IsLoadingSurface())
                {
                    StartCoroutine(apiManager.FetchSurface(false, (success) =>
                    {
                        if (!success)
                        {
                            Debug.LogWarning("Gagal load surface");
                            return;
                        }

                        Debug.Log("Surface ready (callback)");

                        if (uIManager != null)
                            uIManager.RefreshCurrentView();
                    }));
                }
            }
        }

        surfaceObj.SetActive(isTargetVisible && newState == AppState.SURFACE_STATE);
        hardwareObj.SetActive(isTargetVisible && newState == AppState.HARDWARE_STATE);

        if (focusObj != null)
        {
            foreach (Transform child in focusObj.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (newState == AppState.FOCUS_STATE && currentFocus != null)
        {
            currentFocus.SetActive(true);

            if (focusAnim != null)
                focusAnim.Play();
        }

        if (newState == AppState.HARDWARE_STATE && hardwareAnim != null)
        {
            hardwareAnim.Play();
        }

        if (uIManager != null)
        {
            uIManager.UpdateModeText(newState);

            if (newState == AppState.FOCUS_STATE)
                uIManager.ShowFocusUI();
            else
                uIManager.ShowMainUI();

            uIManager.SetLoading(
                apiManager.IsLoadingHardware() || apiManager.IsLoadingSurface()
            );
        }

        Debug.Log("Current State: " + currentState);
    }
    void ResetState()
    {
        currentFocus = null;
        SetState(AppState.SURFACE_STATE);
    }
}