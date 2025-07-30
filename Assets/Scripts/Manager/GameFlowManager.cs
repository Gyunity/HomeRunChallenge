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
        while (!isGameOver && currentRound < rounds.Length)
        {
            var cfg = rounds[currentRound];
            ballsThrown = 0;
            roundText.text = $"ROUND {currentRound + 1}";

            while (ballsThrown < ballsPerRound)
            {
                // ���� ���� �� �ӵ� ����
                var types = cfg.ballTypes;
                var type = types[Random.Range(0, types.Length)];
                float speedKmh = Random.Range(cfg.minSpeedKmh, cfg.maxSpeedKmh);
                // ����
                pitchingManager.PitchBall(type, speedKmh);

                ballsThrown++;
                UpdateUI();
                hitInputHandler.ResetHitFlag();

                //yield return new WaitForSeconds(pitchingManager.pitchDuration + 0.2f);
                hitInputHandler.NoSwingCheck();
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
            if(scoreManager.currentScore < stageClearScore)
            {
                isGameOver =true;
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
        Debug.Log("10�� �Ϸ�. ���� ���ھ�: " + scoreManager.currentScore);
        // ���� �Ǵ� ��Ʈ���� UI ó��
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
