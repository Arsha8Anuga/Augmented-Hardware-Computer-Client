using UnityEngine;
using System.Collections;

public class SimpleFadeMove : MonoBehaviour
{
    public float duration = 0.4f;
    public float moveOffset = 0.05f;

    Vector3 startPos;
    Vector3 targetPos;
    Renderer[] renderers;

    void Awake()
    {
        startPos = transform.localPosition;
        targetPos = startPos;
        startPos.y -= moveOffset;

        renderers = GetComponentsInChildren<Renderer>();
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

        SetAlpha(0f);

        while (t < duration)
        {
            t += Time.deltaTime;
            float p = t / duration;

            transform.localPosition = Vector3.Lerp(startPos, targetPos, p);
            SetAlpha(p);

            yield return null;
        }

        transform.localPosition = targetPos;
        SetAlpha(1f);
    }

    void SetAlpha(float a)
    {
        foreach (var r in renderers)
        {
            foreach (var mat in r.materials)
            {
                Color c = mat.color;
                c.a = a;
                mat.color = c;
            }
        }
    }
}