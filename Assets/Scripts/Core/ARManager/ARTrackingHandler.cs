using UnityEngine;

public class ARTrackingHandler : MonoBehaviour
{
    public float lostDelay = 3f;

    private float lostTimer = 0f;
    private bool pendingReset = false;

    public bool UpdateTracking(bool isVisible)
    {
        if (!isVisible)
        {
            lostTimer += Time.deltaTime;

            if(lostTimer >= lostDelay)
            {
                pendingReset = true;
            }
        }
        else
        {
            if (pendingReset)
            {
                pendingReset = false;
                lostTimer = 0f;
                return true;
            }
            
            lostTimer = 0f;
        }

        return false;
    }
}