using UnityEngine;

public class HardwarePart : MonoBehaviour
{
    public HardwareData data;
    public ARStateManager manager;
    public GameObject focusObject;

    public void OnClicked()
    {
        if (manager == null || data == null || focusObject == null)
        {
            Debug.LogWarning("Data belum lengkap!");
            return;
        }

        manager.EnterFocus(focusObject, data);
    }
}