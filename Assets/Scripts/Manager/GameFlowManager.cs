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
    [SerializeField]
    public TMP_Text ballCountText;
    [SerializeField]
    public TMP_Text roundText;


    [Header("Settings")]
    [SerializeField]
    private RoundConfig[] rounds;
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

    private int currentRound;
    private int ballsThrown;
    private bool isGameOver;

    void Start()
    {
        hitInputHandler.OnHit += HandleHit;
        hitInputHandler.OnMiss += HandleMiss;
        UpdateUI();
        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        scoreManager.ResetScore();
        currentRound = 0;
        isGameOver = false;
        while (!isGameOver && currentRound < rounds.Length)
        {
            var cfg = rounds[currentRound];
            ballsThrown = 0;
            roundText.text = $"ROUND {currentRound + 1}";

            while (ballsThrown < ballsPerRound)
            {
                // 랜덤 구종 및 속도 선택
                var types = cfg.ballTypes;
                var type = types[Random.Range(0, types.Length)];
                float speedKmh = Random.Range(cfg.minSpeedKmh, cfg.maxSpeedKmh);
                // 투구
                pitchingManager.PitchBall(type, speedKmh);

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
                    if (scoreManager.currentScore >= stageClearScore)
                    {
                        StageClear();
                    }
                    yield break;
                }

                //재투구 전 대기
                float delay = hitInputHandler.lastHitSuccessful ? hitDelay : missDelay;
                yield return new WaitForSeconds(delay);
            }

            //라운드 종료 후
            scoreManager.AddScore();
            if(scoreManager.currentScore < stageClearScore)
            {
                isGameOver =true;
                GameOver();
                break;
            }

            currentRound++;
        }
        // 전체 종료
        Debug.Log($"Game End! Final Score: {scoreManager.totalScore}");
        // TODO: 결과 UI 띄우기
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
        Debug.Log($"Round {currentRound + 1} Cleared!");
        // 클리어 UI, 다음 스테이지 로딩 등 처리
    }

    private void StageEnd()
    {
        Debug.Log("10구 완료. 최종 스코어: " + scoreManager.currentScore);
        // 실패 또는 리트라이 UI 처리
    }

    private void GameOver()
    {
        Debug.Log("게임 종료!");
    }

    private void UpdateUI()
    {
        if (ballCountText != null)
            ballCountText.text = $"{ballsThrown} / 10";
    }
}
