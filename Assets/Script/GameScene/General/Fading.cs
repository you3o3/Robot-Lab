using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : SingletonWMonoBehaviour<Fading>
{
    public static Fading Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, true);
    }

    // Source: https://www.youtube.com/watch?v=oNz4I0RfsEg

    public void StartFadeOut(SpriteRenderer srend, float fadeOutTime, System.Action onComplete)
    {
        float fadeOutGap = fadeOutTime / 20;
        StartCoroutine(FadeOut(srend, fadeOutGap, onComplete));
    }

    private IEnumerator FadeOut(SpriteRenderer srend, float fadeOutGap, System.Action onComplete)
    {
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            if (srend == null || srend.gameObject == null) yield return new WaitForSeconds(fadeOutGap);
            Color c = srend.material.color;
            c.a = f;
            srend.material.color = c;
            yield return new WaitForSeconds(fadeOutGap);
        }
        onComplete();
    }

    public void StartFadeIn(SpriteRenderer srend, float fadeInTime, System.Action onComplete)
    {
        float fadeInGap = fadeInTime / 20;
        StartCoroutine(FadeIn(srend, fadeInGap, onComplete));
    }

    private IEnumerator FadeIn(SpriteRenderer srend, float fadeInGap, System.Action onComplete)
    {
        for (float f = -0.05f; f <= 1f; f += 0.05f)
        {
            if (srend == null || srend.gameObject == null) yield return new WaitForSeconds(fadeInGap);
            Color c = srend.material.color;
            c.a = f;
            srend.material.color = c;
            yield return new WaitForSeconds(fadeInGap);
        }
        onComplete();
    }
}
