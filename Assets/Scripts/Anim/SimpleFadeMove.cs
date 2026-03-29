using UnityEngine;
using System.Collections;

public class SimpleFadeMove : MonoBehaviour
{
    public float duration = 0.4f;
    public float moveOffset = 0.05f;
    public bool scaleXOnly = true;

    Vector3 startPos;
    Vector3 targetPos;

    Vector3 startScale;
    Vector3 targetScale;

    void Awake()
    {
        targetPos = transform.localPosition;
        startPos = targetPos;
        startPos.y -= moveOffset;

        targetScale = transform.localScale;

        if(scaleXOnly)
            startScale = new Vector3(0f, targetScale.y, targetScale.z);
        else
            startScale = Vector3.zero;
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float t = 0f;

        transform.localPosition = startPos;
        transform.localScale = startScale;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            transform.localPosition = Vector3.Lerp(startPos, targetPos, p);
            transform.localScale = Vector3.Lerp(startScale, targetScale, p);

            yield return null;
        }

        transform.localPosition = targetPos;
        transform.localScale = targetScale;
    }
}