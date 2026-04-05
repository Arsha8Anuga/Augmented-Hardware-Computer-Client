using UnityEngine;

public class ARStateAPIHandler : MonoBehaviour
{
    public HardwareRepository hardwareRepo;
    public SurfaceRepository surfaceRepo;
    public AppInfoRepository appInfoRepo;

    public WorldTitleDisplay worldTitleDisplay;

    public UIManager uiManager;

    public void HandleState(AppState state)
    {
        if (state == AppState.HARDWARE_STATE)
        {
            if (!hardwareRepo.IsReady())
            {
                StartCoroutine(hardwareRepo.Fetch(false, (success) =>
                {
                    if (success && uiManager != null)
                    {
                        uiManager.RefreshCurrentView();
                    }
                }));
            }
        }

        if (state == AppState.SURFACE_STATE)
        {
            if (!surfaceRepo.IsReady())
            {
                StartCoroutine(surfaceRepo.Fetch(false, (success) =>
                {
                    if (success && uiManager != null)
                    {
                        uiManager.RefreshCurrentView();
                    }
                }));
            }
        }

        // optional: load title sekali saja
        if (appInfoRepo.HasTitle())
        {
            worldTitleDisplay?.SetTitle(appInfoRepo.GetTitle());
        }
        else
        {
            StartCoroutine(appInfoRepo.FetchTitle(false, (success) =>
            {
                if (success)
                {
                    worldTitleDisplay?.SetTitle(appInfoRepo.GetTitle());
                }
            }));
        }
    }

    public bool IsLoading()
    {
        return !hardwareRepo.IsReady() || 
               !surfaceRepo.IsReady() || 
               !appInfoRepo.HasTitle();
    }
}