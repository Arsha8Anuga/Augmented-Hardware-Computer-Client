using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum HardwareType
{
    CPU,
    GPU,
    RAM
}

public class ARStateManager : MonoBehaviour
{   
    [Header("State")]
    public AppState currentState = AppState.SURFACE_STATE;

    [Header("Tracking")]
    public float lostDelay = 3f;
    private float lostTimer = 0f;
    private bool isTargetVisible = true;

    [Header("Objects")]
    public GameObject surfaceObj;
    public GameObject hardwareObj;
    public GameObject focusObj;

    [Header("Focus Registry")]
    public GameObject cpuFocus;
    public GameObject gpuFocus;
    public GameObject ramFocus;
    public GameObject currentFocus;

    [Header("UI")]
    public UIManager uIManager;

    private Dictionary<HardwareType, GameObject> focusMap;

    void Awake()
    {
        focusMap = new Dictionary<HardwareType, GameObject>()
        {
            {HardwareType.CPU, cpuFocus},
            {HardwareType.RAM, ramFocus},
            {HardwareType.GPU, gpuFocus}
        };
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      SetState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTargetVisible)
        {
            lostTimer += Time.deltaTime;

            if(lostTimer >= lostDelay)
            {
                ResetState();
            }
        }
        else
        {
            lostTimer = 0f;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SetState(AppState.SURFACE_STATE);
            SetTargetVisible(true);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SetState(AppState.HARDWARE_STATE);
        }

    }

    public GameObject GetFocusObject(HardwareType type)
    {
        if (focusMap.TryGetValue(type, out GameObject obj))
        {
            return obj;
        }

        return null;
    }

    public void SetTargetVisible(bool visible)
    {
        isTargetVisible = visible;

        if (visible)
        {
            lostTimer = 0f;
        }
    }

    public void SwitchMode()
    {
        if(currentState == AppState.SURFACE_STATE)
        {
            SetState(AppState.HARDWARE_STATE);
        }
        else if(currentState == AppState.HARDWARE_STATE)
        {
            SetState(AppState.SURFACE_STATE);
        }
    }

    public void EnterFocus(GameObject target, HardwareData data)
    {
        if(currentState != AppState.HARDWARE_STATE)
        {
            Debug.Log("Focus State hanya bisa dari Hardware State");
            return;
        }

        currentFocus = target;

        if(uIManager != null)
        {
            uIManager.UpdateInfo(data.title, data.description);    
        }

        SetState(AppState.FOCUS_STATE);
    }

    public void BackFromFocus()
    {
        if (currentState == AppState.FOCUS_STATE)
        {
            SetState(AppState.HARDWARE_STATE);
        }
    }

    void SetState(AppState newState)
    {
        currentState = newState;
        
        surfaceObj.SetActive(newState == AppState.SURFACE_STATE);
        hardwareObj.SetActive(newState == AppState.HARDWARE_STATE);

        
        
        if (focusObj != null)
        {
            foreach(Transform child in focusObj.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (newState == AppState.FOCUS_STATE && currentFocus != null)
        {
            currentFocus.SetActive(true);
        }

        uIManager.UpdateModeText(newState);

        if(uIManager != null)
        {
            if(newState == AppState.FOCUS_STATE)
            {
                uIManager.ShowFocusUI();
            }
            else
            {
                uIManager.ShowMainUI();
            }
        }

        Debug.Log("Current State : " + currentState);
    }

    void ResetState()
    {
        currentFocus = null;
        SetState(AppState.SURFACE_STATE);
    }

}
