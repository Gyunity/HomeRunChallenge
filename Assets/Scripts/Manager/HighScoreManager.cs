using UnityEngine;

public class HighScoreManager
{
    //키 규칙 : 버전별로 뒤에 식별자 붙이기
    private static string Key(string suffix = "Defailt") => $"HIGH_SCORE_{suffix}";

    //현재 최고점 가져오기
    public static int GetHighScore(string keySuffix = "Default")
    {
        return PlayerPrefs.GetInt(Key(keySuffix), 0);
    }

    //점수를 반영해 최고점 갱신 시 true반환
    public static bool TrySetHighScore(int score, string keySuffix = "Default")
    {
        int cur = GetHighScore(keySuffix);
        if (score > cur)
        {
            PlayerPrefs.SetInt(Key(keySuffix), score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    //최고점 리셋
    public static void ResetHighScore(string keySuffix = "Default")
    {
        PlayerPrefs.DeleteKey(Key(keySuffix));
        PlayerPrefs.Save();
    }

    //bestscore결과
    // 로비나 결과 씬에서 최고점 표시
//    bestScoreText.text = HighScoreManager.GetHighScore("Default").ToString("N0");


}
