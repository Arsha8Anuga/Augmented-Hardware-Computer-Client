using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ARStateManager : MonoBehaviour
{
    public AppState currentState = AppState.SURFACE_STATE;

    public ARTrackingHandler trackingHandler;
    public ARStateAPIHandler apiHandler;
    public ARStateViewHandler viewHandler;

    public UIManager uIManager;

    public bool isTargetVisible = true;
    public GameObject currentFocus;

    private int loadingCount = 0;

    IEnumerator Start()
    {
        while (!viewHandler.IsReady)
            yield return null;

        setState(currentState);
    }

    void Update()
    {
        if (Keyboard.current != null)
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SetTargetVisible(true);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SetTargetVisible(false);
        }
    }

        if (trackingHandler != null)
        {
            bool shouldReset = trackingHandler.UpdateTracking(isTargetVisible);

            if (shouldReset)
            {
                ResetState();
            }
        }
    }

    public void setState(AppState newState)
    {
        currentState = newState;

        apiHandler?.HandleState(newState, this);
        viewHandler?.ApplyState(newState, isTargetVisible, currentFocus);

        Debug.Log("Current State: " + currentState);
    }

    public void EnterFocus(GameObject focusObject, HardwareData data)
    {
        if (focusObject == null || data == null)
            return;

        currentFocus = focusObject;

        uIManager?.UpdateInfo(data.title, data.description);

        setState(AppState.FOCUS_STATE);
    }

    void ResetState()
    {
        currentFocus = null;
        setState(AppState.SURFACE_STATE);
    }

    public void SetTargetVisible(bool visible)
    {
        if (isTargetVisible == visible)
            return;

        isTargetVisible = visible;

        viewHandler?.ApplyState(currentState, isTargetVisible, currentFocus);
    }

    public void SwitchMode()
    {
        Debug.Log("SwitchMode called, visible: " + isTargetVisible);
        if (!isTargetVisible) return;

        setState(currentState == AppState.SURFACE_STATE
            ? AppState.HARDWARE_STATE
            : AppState.SURFACE_STATE);
    }

    public void BackFromFocus()
    {
        if (currentState != AppState.FOCUS_STATE)
            return;

        if (currentFocus != null)
            currentFocus.SetActive(false);

        currentFocus = null;

        setState(AppState.HARDWARE_STATE);
    }

    public void BeginLoading()
    {
        loadingCount++;
        UpdateLoadingUI();
    }

    public void EndLoading()
    {
        loadingCount = Mathf.Max(0, loadingCount - 1);
        UpdateLoadingUI();
    }

    private void UpdateLoadingUI()
    {
        uIManager?.SetLoading(loadingCount > 0);
    }
}