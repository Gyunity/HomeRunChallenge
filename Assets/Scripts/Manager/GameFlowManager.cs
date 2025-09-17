using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

// Ȩ�� ç������� 1�� ������Ʈ�� GmaeFlowManager�Դϴ�.


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
    [Tooltip("�� ����� �� ���� ��")]
    [SerializeField]
    private int ballsPerRound = 10;
    [Tooltip("���� �� ������ ���� �ð� (��)")]
    [SerializeField]
    private float missDelay = 2f;
    [Tooltip("���� �� ������ ���� �ð� (��)")]
    [SerializeField]
    private float hitDelay = 2f;
    [Tooltip("Ŭ���� ���� ����")]
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

                // ���� ���� �� �ӵ� ����
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
                // ����

                pitcherAni.PichingSet(type, speedKmh);
                pitchingAnimator.SetTrigger("Piching");
                yield return new WaitUntil(() => isThrow);
                isThrow = false;
                ballsThrown++;
                UpdateUI();


                //Ÿ�� �Ǵ� �꽺�� �̺�Ʈ�� �ö����� ���
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

                //������ �� ���
                float delay = hitInputHandler.lastHitSuccessful ? hitDelay : missDelay;
                yield return new WaitForSeconds(delay);
            }

        }
        // ��ü ����
        GameOver();
    }

    private void HandleHit(AccuracyResult acc, bool isHomeRun)
    {
        scoreManager.AddScore(acc, isHomeRun);
    }

    private void HandleMiss()
    {
        // ���� �� Ư�� ó�� �ʿ������ �� ����
    }

    private void StageClear()
    {
        Debug.Log($"Round {currentRound + 1} Cleared!");
        // Ŭ���� UI, ���� �������� �ε� �� ó��
    }

    private void StageEnd()
    {
        int total = scoreManager.totalScore;
        bool newRecord = HighScoreManager.TrySetHighScore(total, "Default");
        Debug.Log("10�� �Ϸ�. ���� ���ھ�: " + scoreManager.currentScore);
        ballsThrown = 0;

    }

    private void GameOver()
    {
        Debug.Log($"Game End! Final Score: {scoreManager.totalScore}");
        isGameOver = true;

        Debug.Log("���� ����!");
    }
    private void GameClear()
    {
        Debug.Log($"Game End! Final Score: {scoreManager.totalScore}");
        isGameOver = false;
        isGameClear = true;
        Debug.Log("���� Ŭ����!");
    }

    private void UpdateUI()
    {
        if (ballCountText != null)
            ballCountText.text = $"{ballsThrown} / 10";
    }
}
