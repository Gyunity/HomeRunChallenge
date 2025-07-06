using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [Tooltip("�⺻ ��Ʈ�� ����")]
    [SerializeField]
    private int baseHitScore = 1000;
    [Tooltip("Ȩ�� ���ʽ� ����")]
    [SerializeField]
    private int homeRunBonus = 2000;

    public int Score { get; private set; }
    
    public void ResetScore()
    {
        Score = 0;
        UpdateUI();
    }

    public void AddScore(AccuracyResult acc, bool isHomeRun)
    {
        int s = Mathf.RoundToInt(acc.TimingAccuray * acc.PositionAccuracy * baseHitScore);
        if (isHomeRun)
            s += homeRunBonus;
        Score += s;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score : {Score}";
    }
}
