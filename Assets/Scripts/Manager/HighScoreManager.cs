using UnityEngine;

public class HighScoreManager
{
    //Ű ��Ģ : �������� �ڿ� �ĺ��� ���̱�
    private static string Key(string suffix = "Defailt") => $"HIGH_SCORE_{suffix}";

    //���� �ְ��� ��������
    public static int GetHighScore(string keySuffix = "Default")
    {
        return PlayerPrefs.GetInt(Key(keySuffix), 0);
    }

    //������ �ݿ��� �ְ��� ���� �� true��ȯ
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

    //�ְ��� ����
    public static void ResetHighScore(string keySuffix = "Default")
    {
        PlayerPrefs.DeleteKey(Key(keySuffix));
        PlayerPrefs.Save();
    }

    //bestscore���
    // �κ� ��� ������ �ְ��� ǥ��
//    bestScoreText.text = HighScoreManager.GetHighScore("Default").ToString("N0");


}
