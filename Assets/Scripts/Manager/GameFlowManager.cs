using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

// 홈런 챌린지라는 1인 프로젝트의 GmaeFlowManager입니다.


public class GameFlowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator pitchingAnimator;
    [SerializeField]
    private PitchingAni pitcherAni;
    [SerializeField]
    private HitInputHandler hitInputHandler;
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    public TMP_Text ballCountText;
    [SerializeField]
    public TMP_Text roundText;
    [SerializeField]
    private RoundTransitionUI transitionUI;

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

    private int currentRound = 0;
    private int ballsThrown;
    private bool isGameOver;
    private bool isGameClear;

    private bool isThrow = false;

    void Start()
    {
        hitInputHandler.OnHit += HandleHit;
        hitInputHandler.OnMiss += HandleMiss;
        UpdateUI();
        StartCoroutine(RoundRoutine());
        SoundManager.Instance.PlayBGM("BGM_PLAY");
    }

    public void ThorwTrue()
    {
        isThrow = true;
    }

    private IEnumerator RoundRoutine()
    {
        scoreManager.ResetScore();
        //currentRound = 0;
        isGameOver = false;
        while (!isGameOver && currentRound <= rounds.Length)
        {

            var cfg = rounds[currentRound];
            string typesStr = string.Join(", ", cfg.ballTypes.Select(t => t.ToString()));
            yield return StartCoroutine(transitionUI.Show(currentRound, scoreManager.totalScore, typesStr, cfg.maxSpeedKmhFast, isGameOver));


            if (!transitionUI.ContinueChosen)
            {
                Debug.Log("Player stopped at round " + (currentRound + 1));
                isGameOver = true;
                yield break;
            }

            ballsThrown = 0;
            roundText.text = $"ROUND {currentRound + 1}";

            while (ballsThrown < ballsPerRound)
            {

                // 랜덤 구종 및 속도 선택
                CurvePitchTrajectory.BallType[] types = cfg.ballTypes;
                CurvePitchTrajectory.BallType type = types[Random.Range(0, types.Length)];

                float speedKmh;
                if (type == CurvePitchTrajectory.BallType.FOURSEAM || type == CurvePitchTrajectory.BallType.TWOSEAM)
                {
                    speedKmh = Random.Range(cfg.minSpeedKmhFast, cfg.maxSpeedKmhFast);
                }
                else
                {
                    speedKmh = Random.Range(cfg.minSpeedKmhBreaking, cfg.maxSpeedKmhBreaking);

                }
                // 투구

                pitcherAni.PichingSet(type, speedKmh);
                pitchingAnimator.SetTrigger("Piching");
                yield return new WaitUntil(() => isThrow);
                isThrow = false;
                ballsThrown++;
                UpdateUI();


                //타격 또는 헛스윙 이벤트가 올때까지 대기
                yield return new WaitUntil(() => hitInputHandler.hitProcessed);

                if (ballsThrown >= 10)
                {
                    scoreManager.AddScore();
                    StageEnd();
                    if(currentRound == rounds.Length)
                    {
                        GameClear();
                        StartCoroutine(transitionUI.Show(currentRound, scoreManager.totalScore, typesStr, cfg.maxSpeedKmhFast, isGameOver, isGameClear));
                        yield break;
                    }

                    if (scoreManager.currentScore >= stageClearScore)
                    {
                        StageClear();
                        currentRound++;
                        StartCoroutine(RoundRoutine());
                        yield break;
                    }
                    else
                    {
                        GameOver();
                        StartCoroutine(transitionUI.Show(currentRound, scoreManager.totalScore, typesStr, cfg.maxSpeedKmhFast, isGameOver));
                        yield break;

                    }

                }

                //재투구 전 대기
                float delay = hitInputHandler.lastHitSuccessful ? hitDelay : missDelay;
                yield return new WaitForSeconds(delay);
            }

        }
        // 전체 종료
        GameOver();
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
        int total = scoreManager.totalScore;
        bool newRecord = HighScoreManager.TrySetHighScore(total, "Default");
        Debug.Log("10구 완료. 최종 스코어: " + scoreManager.currentScore);
        ballsThrown = 0;

    }

    private void GameOver()
    {
        Debug.Log($"Game End! Final Score: {scoreManager.totalScore}");
        isGameOver = true;

        Debug.Log("게임 종료!");
    }
    private void GameClear()
    {
        Debug.Log($"Game End! Final Score: {scoreManager.totalScore}");
        isGameOver = false;
        isGameClear = true;
        Debug.Log("게임 클리어!");
    }

    private void UpdateUI()
    {
        if (ballCountText != null)
            ballCountText.text = $"{ballsThrown} / 10";
    }
}
