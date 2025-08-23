using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

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
        while (!isGameOver && currentRound <= rounds.Length)
        {

            var cfg = rounds[currentRound];
            string typesStr = string.Join(", ", cfg.ballTypes.Select(t => t.ToString()));
            yield return StartCoroutine(transitionUI.Show(currentRound, scoreManager.totalScore, typesStr, cfg.maxSpeedKmhFast));


            if (!transitionUI.ContinueChosen)
            {
                Debug.Log("Player stopped at round " + (currentRound + 1));
                isGameOver = true;
                break;
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

                ballsThrown++;
                UpdateUI();

               
                //Ÿ�� �Ǵ� �꽺�� �̺�Ʈ�� �ö����� ���
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

                //������ �� ���
                float delay = hitInputHandler.lastHitSuccessful ? hitDelay : missDelay;
                yield return new WaitForSeconds(delay);
            }

            //���� ���� ��
            scoreManager.AddScore();
            if (scoreManager.currentScore < stageClearScore)
            {
                isGameOver = true;
                GameOver();
                break;
            }

            currentRound++;
        }
        // ��ü ����
        Debug.Log($"Game End! Final Score: {scoreManager.totalScore}");
        // TODO: ��� UI ����
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
     
    }

    private void GameOver()
    {
        Debug.Log("���� ����!");
    }

    private void UpdateUI()
    {
        if (ballCountText != null)
            ballCountText.text = $"{ballsThrown} / 10";
    }
}
