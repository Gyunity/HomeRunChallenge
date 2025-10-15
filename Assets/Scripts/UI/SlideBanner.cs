using System.Collections;
using UnityEngine;

public class SlideBanner : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField]
    private RectTransform rect;
    [SerializeField]
    private CanvasGroup cg;

    [Header("Positions")]
    [SerializeField]
    private Vector2 showPos;
    [SerializeField]
    private Vector2 hiddenPos;
    [SerializeField]
    private Vector2 startPos;

    [Header("Timing")]
    [SerializeField, Min(0f)]
    private float sliderDuration = 0.35f;
    [SerializeField, Min(0f)]
    private float holdTime = 0.8f;

    [Header("Easing")]
    public AnimationCurve easeIn = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve easeOut = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine routine;

    private void Reset()
    {
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
    }

    public void PlayOnce()
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        rect.anchoredPosition = startPos;
        if (cg)
            cg.alpha = 0f;
        Debug.Log($"{startPos}, {hiddenPos}, {showPos}");
        yield return Slide(startPos, showPos, sliderDuration, easeIn, fadeFrom: 0f, fadeTo: 1f);
        SoundManager.Instance.PlaySFX("SFX_HomeRunVoice", 0f);

        yield return new WaitForSecondsRealtime(holdTime);

        yield return Slide(showPos, hiddenPos, sliderDuration, easeOut, fadeFrom: 1f, fadeTo: 0f);

        routine = null;
    }

    private IEnumerator Slide(Vector2 from, Vector2 to, float dur, AnimationCurve curve, float fadeFrom,float fadeTo)
    {
        float t = 0f;
        while ( t < dur)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / dur);
            float e = curve.Evaluate(p);
            rect.anchoredPosition = Vector2.LerpUnclamped(from, to, e);
            if (cg)
                cg.alpha = Mathf.Lerp(fadeFrom, fadeTo, e);
            yield return null;
        }
        rect.anchoredPosition = to;
        if (cg)
            cg.alpha = fadeTo;
    }
}
