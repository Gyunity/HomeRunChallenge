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

    public int currentScore { get; private set; }
    public int totalScore { get; private set; }
    
    public void ResetScore()
    {
        currentScore = 0;
        UpdateUI();
    }


    public void AddScore()
    {
        totalScore += currentScore;

    }

    public void AddScore(AccuracyResult acc, bool isHomeRun)
    {
        int s = Mathf.RoundToInt(acc.TimingAccuray * acc.PositionAccuracy * baseHitScore);
        if (isHomeRun)
            s += homeRunBonus;
        currentScore += s;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score : {currentScore}";
    }
}
