using UnityEngine;

public class ARStateManager : MonoBehaviour
{
    public AppState currentState = AppState.SURFACE_STATE;

    public ARTrackingHandler trackingHandler;
    public ARStateAPIHandler apiHandler;
    public ARStateViewHandler viewHandler;

    public UIManager uIManager;

    public bool isTargetVisible = true;
    public GameObject currentFocus;

    void Start()
    {
        setState(currentState);
    }

    void Update()
    {
        if(trackingHandler != null)
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

        apiHandler?.HandleState(newState);
        viewHandler?.ApplyState(newState, isTargetVisible, currentFocus);

        if (uIManager != null && apiHandler != null)
        {
            uIManager.SetLoading(apiHandler.IsLoading());
        }

        Debug.Log("Current State: " + currentState);
    }

    void ResetState()
    {
        currentFocus = null;
        setState(AppState.SURFACE_STATE);
    }
}