using UnityEngine;

public static class VibrationManager
{
    //기본진동 (100)
    public static void Vibrate()
    {
#if UNITY_ANDROID && UNITY_IOS
        Handheld.Vibrate();
#else
        Debug.Log("Vibrate called (Editor)!");
#endif
        
    }

    //시간(ms) 진동
    public static void Vibrate(long milliseconds)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
#elif UNITY_IOS && !UNITY_EDITOR
Handheld.Vibrate();
#else
        Debug.Log($"Vibrate {milliseconds}ms called (Editor)!");
#endif
    }

}
