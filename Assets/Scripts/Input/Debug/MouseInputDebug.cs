using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MouseInputDebug : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Script jalan");
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            return;

            Debug.Log("Click detected");

            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hit: " + hit.collider.name);

                HardwarePart part = hit.collider.GetComponentInParent<HardwarePart>();

                if (part != null)
                {
                    Debug.Log("HardwarePart ditemukan");
                    part.OnClicked();
                }
                else
                {
                    Debug.Log("Tidak ada HardwarePart");
                }
            }
            else
            {
                Debug.Log("Raycast tidak kena apa-apa");
            }
        }

        // TOUCH (untuk simulator / mobile)
        if (Touch.activeTouches.Count > 0)
        {
            var touch = Touch.activeTouches[0];

            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                Debug.Log("Touch detected");

                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                Ray ray = Camera.main.ScreenPointToRay(touch.screenPosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Hit: " + hit.collider.name);

                    HardwarePart part = hit.collider.GetComponentInParent<HardwarePart>();

                    if (part != null)
                    {
                        Debug.Log("HardwarePart ditemukan (touch)");
                        part.OnClicked();
                    }
                }
            }
        }
    }
}