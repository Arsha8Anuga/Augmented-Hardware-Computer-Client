using UnityEngine;

public class ARStateAPIHandler : MonoBehaviour
{
    public HardwareRepository hardwareRepo;
    public SurfaceRepository surfaceRepo;
    public AppInfoRepository appInfoRepo;

    public WorldTitleDisplay worldTitleDisplay;
    public UIManager uiManager;

    public bool forceFetchMode = true; 

    public void HandleState(AppState state, ARStateManager manager)
    {
        bool forceFetch = forceFetchMode; // ambil dari setting

        if (state == AppState.HARDWARE_STATE)
        {
            if (hardwareRepo.CanFetch() || forceFetch)
            {
                manager.BeginLoading();
                StartCoroutine(hardwareRepo.Fetch(forceFetch, (success) =>
                {
                    manager.EndLoading();
                    if (success)
                        uiManager?.RefreshCurrentView();
                }));
            }
        }

        if (state == AppState.SURFACE_STATE)
        {
            if (surfaceRepo.CanFetch() || forceFetch)
            {
                manager.BeginLoading();
                StartCoroutine(surfaceRepo.Fetch(forceFetch, (success) =>
                {
                    manager.EndLoading();
                    if (success)
                        uiManager?.RefreshCurrentView();
                }));
            }
        }


        // TITLE
        if (appInfoRepo.HasTitle() && !forceFetchMode)
        {
            worldTitleDisplay?.SetTitle(appInfoRepo.GetTitle());
        }
        else if (appInfoRepo.CanFetch() || forceFetchMode)
        {
            manager.BeginLoading();
            StartCoroutine(appInfoRepo.FetchTitle(forceFetchMode, (success) =>
            {
                manager.EndLoading();
                if (success)
                    worldTitleDisplay?.SetTitle(appInfoRepo.GetTitle());
            }));
        }
    }
}