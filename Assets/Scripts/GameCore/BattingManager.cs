using Unity.VisualScripting;
using UnityEngine;

public class BattingManager : MonoBehaviour
{
    public static BattingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public enum HitResult
    {
        Perfect,
        Good,
        Miss
    }

    HitResult result = HitResult.Perfect;

    public float battingTime = 0;
    public bool canJudge = false;
    public float perfectTime = 0;

    void Update()
    {
        BattingTouch();
    }

    private void BattingTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            battingTime = Time.time;
            TryJudge();
        }
    }

    public void TryJudge()
    {
        if (!canJudge)
            return;

        float delta = perfectTime - battingTime;
        canJudge = false;

        if (delta > -0.1f && delta < 0.1f)
        {
            result = HitResult.Perfect;
            HirResultLog();
        }
        else if (delta > -0.2f && delta < 0.2f)
        {
            result = HitResult.Good;
            HirResultLog();
        }
        else
        {
            result = HitResult.Miss;
            HirResultLog();
        }
    }

    private void HirResultLog()
    {
        if(result == HitResult.Perfect)
            Debug.Log("Perfect!");
        else if (result == HitResult.Good)
            Debug.Log("Good!");
        else if (result == HitResult.Miss)
            Debug.Log("Bad¤Ğ");
    }
}
