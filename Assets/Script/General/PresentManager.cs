using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// use for presentation/ trailer purpose. Will not appear in game.
public class PresentManager : MonoBehaviour
{
    public GameObject text;
    private CanvasRenderer textCrend;

    private void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            text.SetActive(true);
            textCrend = text.GetComponent<CanvasRenderer>();
            FadeInText(1);
        }
    }

    public void FadeInText(float fadeInTime)
    {
        StartFadeIn(textCrend, fadeInTime, () => { });
    }

    private void StartFadeIn(CanvasRenderer crend, float fadeInTime, System.Action onComplete)
    {
        float fadeInGap = fadeInTime / 20;
        StartCoroutine(FadeIn(crend, fadeInGap, onComplete));
    }

    private IEnumerator FadeIn(CanvasRenderer crend, float fadeInGap, System.Action onComplete)
    {
        for (float f = -0.05f; f <= 1f; f += 0.05f)
        {
            if (crend == null || crend.gameObject == null) yield return new WaitForSeconds(fadeInGap);
            Color c = crend.GetColor();
            c.a = f;
            crend.SetColor(c);
            yield return new WaitForSeconds(fadeInGap);
        }
        onComplete();
    }
}
