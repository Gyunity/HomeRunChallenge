using UnityEngine;

public static class VibrationManager
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject unityActivity;
    private static AndroidJavaObject vibrator;

    static VibrationManager()
    {
        //�ȵ���̵� ���� ���� �ʱ�ȭ
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        vibrator = unityActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

    }
#endif
    //�⺻���� (100)
    public static void Vibrate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vibrator != null)
            vibrator.Call("vibrate", 100);
#elif UNITY_IOS && !UNITY_EDITOR
Handheld.Vibrate();
#else
        Debug.Log("Vibrate called (Editor)!");
#endif
    }

    //�ð�(ms) ����
    public static void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (vibrator != null)
            vibrator.Call("vibrate", milliseconds);
#elif UNITY_IOS && !UNITY_EDITOR
Handheld.Vibrate();
#else
        Debug.Log($"Vibrate {milliseconds}ms called (Editor)!");
#endif
    }

}
