using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MouseInputDebug : MonoBehaviour
{
    void Start()
    {
        Debug.Log("MouseInputDebug jalan");
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        HandleMouse();
        HandleTouch();
    }

    // ================= MOUSE =================
    void HandleMouse()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 pos = Mouse.current.position.ReadValue();

            Debug.Log("=== MOUSE CLICK ===");
            ProcessInput(pos);
        }
    }

    // ================= TOUCH =================
    void HandleTouch()
    {
        if (Touch.activeTouches.Count == 0) return;

        var touch = Touch.activeTouches[0];

        Debug.Log("Touch phase: " + touch.phase);

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            Debug.Log("=== TOUCH DETECTED ===");
            ProcessInput(touch.screenPosition);
        }
    }

    // ================= CORE =================
    void ProcessInput(Vector2 screenPos)
    {
        bool isOverUI = IsPointerOverUI(screenPos);

        Debug.Log("IsOverUI: " + isOverUI);

        if (isOverUI)
        {
            Debug.Log("➡ Klik kena UI");
            DebugUIRaycast(screenPos);
            return;
        }

        Debug.Log("➡ Klik bukan UI → lanjut 3D");

        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("3D Hit: " + hit.collider.name);

            HardwarePart part = hit.collider.GetComponentInParent<HardwarePart>();

            if (part != null)
            {
                Debug.Log("HardwarePart ditemukan → trigger OnClicked()");
                part.OnClicked();
            }
            else
            {
                Debug.Log("Tidak ada HardwarePart di object ini");
            }
        }
        else
        {
            Debug.Log("Raycast tidak kena apa-apa");
        }
    }

    // ================= UI CHECK =================
    bool IsPointerOverUI(Vector2 position)
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("EventSystem tidak ada!");
            return false;
        }

        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }

    // ================= UI DEBUG =================
    void DebugUIRaycast(Vector2 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        Debug.Log("=== UI RAYCAST RESULTS ===");

        foreach (var r in results)
        {
            Debug.Log($"UI Hit: {r.gameObject.name} | Depth: {r.depth}");
        }

        if (results.Count == 0)
        {
            Debug.Log("Tidak ada UI yang kena (aneh)");
        }
    }
}