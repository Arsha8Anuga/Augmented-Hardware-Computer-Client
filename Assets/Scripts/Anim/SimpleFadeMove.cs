using UnityEngine;
using System.Collections;

public class SimpleFadeMove : MonoBehaviour
{
    public float duration = 0.4f;
    public float moveOffset = 0.05f;

    Vector3 startPos;
    Vector3 targetPos;
    CanvasGroup canvasGroup;

    void Awake()
    {
        startPos = transform.localPosition;
        targetPos = startPos;
        startPos.y -= moveOffset;

        canvasGroup = GetComponent<CanvasGroup>();
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

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            transform.localPosition = Vector3.Lerp(startPos, targetPos, p);

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, p);

            yield return null;
        }

        transform.localPosition = targetPos;

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;
    }
}