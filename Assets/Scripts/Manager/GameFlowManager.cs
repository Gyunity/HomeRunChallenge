using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameFlowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PitchingManager pitchingManager;
    [SerializeField]
    private HitInputHandler hitInputHandler;
    [SerializeField]
    private ScoreManager scoreManager;

    [Header("Settings")]
    [Tooltip("한 라운드당 공 투구 수")]
    [SerializeField]
    private int ballsPerRound = 10;
    [Tooltip("실패 후 재투구 지연 시간 (초)")]
    [SerializeField]
    private float missDelay = 2f;
    [Tooltip("성공 후 재투구 지연 시간 (초)")]
    [SerializeField]
    private float hitDelay = 2f;
    [Tooltip("클리어 조건 점수")]
    [SerializeField]
    private int stageClearScore = 10000;

    private int ballsThrown;
    public TMP_Text ballCount;

    void Start()
    {
        hitInputHandler.OnHit += HandleHit;
        hitInputHandler.OnMiss += HandleMiss;
        UpdateUI();
        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        ballsThrown = 0;
        scoreManager.ResetScore();

        while (ballsThrown < ballsPerRound)
        {
            //1구
            pitchingManager.PitchBall();
            ballsThrown++;
            UpdateUI();
            hitInputHandler.ResetHitFlag();

            //yield return new WaitForSeconds(pitchingManager.pitchDuration + 0.2f);
            hitInputHandler.NoSwingCheck();
            //타격 또는 헛스윙 이벤트가 올때까지 대기
            yield return new WaitUntil(() => hitInputHandler.hitProcessed);

            if (ballsThrown >= 10)
            {
                StageEnd();
                if (scoreManager.Score >= stageClearScore)
                {
                    StageClear();
                }
                yield break;
            }

            //재투구 전 대기
            float delay = hitInputHandler.lastHitSuccessful ? hitDelay : missDelay;
            yield return new WaitForSeconds(delay);
        }
    }

    private void HandleHit(AccuracyResult acc, bool isHomeRun)
    {
        scoreManager.AddScore(acc, isHomeRun);
    }

    private void HandleMiss()
    {
        // 실패 시 특별 처리 필요없으면 빈 상태
    }

    private void StageClear()
    {
        Debug.Log("스테이지 클리어!");
        // 클리어 UI, 다음 스테이지 로딩 등 처리
    }

    private void StageEnd()
    {
        Debug.Log("10구 완료. 최종 스코어: " + scoreManager.Score);
        // 실패 또는 리트라이 UI 처리
    }

    private void UpdateUI()
    {
        if (ballCount != null)
            ballCount.text = $"{ballsThrown} / 10";
    }
}
