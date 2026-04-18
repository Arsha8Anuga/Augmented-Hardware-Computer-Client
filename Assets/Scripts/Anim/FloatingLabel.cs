using UnityEngine;

public class FloatingLabel : MonoBehaviour
{
    public float amplitude = 0.005f;
    public float speed = 2f;

    Vector3 basePos;

    void OnEnable()
    {
        basePos = transform.localPosition;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = basePos + new Vector3(0, y, 0);
    }
}