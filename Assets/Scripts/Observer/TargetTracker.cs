using UnityEngine;
using Vuforia;

public class TargetTracker : MonoBehaviour
{
    ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        observer.OnTargetStatusChanged += OnStatusChanged;
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            Debug.Log("Target terdeteksi");
        }
        else
        {
            Debug.Log("Target belum/kehilangan tracking");
        }
    }
}