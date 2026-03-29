using UnityEngine;

public class RotateFocus : MonoBehaviour
{
    public Vector3 startRotation = new Vector3(15f, 25f, 0f);
    public float introSpeed = 4f;
    public float spinSpeed = 20f;

    bool introDone = false;

    void OnEnable()
    {
        introDone = false;
    }

    void Update()
    {
        if (!introDone)
        {
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                Quaternion.Euler(startRotation),
                Time.deltaTime * introSpeed
            );

            if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(startRotation)) < 0.1f)
            {
                introDone = true;
            }
        }
        else
        {
            transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.Self);
        }
    }
}