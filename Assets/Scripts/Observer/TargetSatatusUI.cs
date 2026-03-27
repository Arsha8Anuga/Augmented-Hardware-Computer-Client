using UnityEngine;
using Vuforia;
using TMPro;


public class TargetStatusUI : MonoBehaviour
{
    public TMP_Text statusText;
    ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();

        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnStatusChanged;
        }
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            statusText.text = "TARGET TERDETEKSI";
        }
        else
        {
            statusText.text = "MENUNGGU TARGET...";
        }
    }
}