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
            //1��
            pitchingManager.PitchBall();
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
                if (scoreManager.Score >= stageClearScore)
                {
                    StageClear();
                }
                yield break;
            }

            //������ �� ���
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
        // ���� �� Ư�� ó�� �ʿ������ �� ����
    }

    private void StageClear()
    {
        Debug.Log("�������� Ŭ����!");
        // Ŭ���� UI, ���� �������� �ε� �� ó��
    }

    private void StageEnd()
    {
        Debug.Log("10�� �Ϸ�. ���� ���ھ�: " + scoreManager.Score);
        // ���� �Ǵ� ��Ʈ���� UI ó��
    }

    private void UpdateUI()
    {
        if (ballCount != null)
            ballCount.text = $"{ballsThrown} / 10";
    }
}
