using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [Tooltip("기본 히트당 만점")]
    [SerializeField]
    private int baseHitScore = 1000;
    [Tooltip("홈런 보너스 점수")]
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
