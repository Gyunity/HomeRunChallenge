using UnityEngine;

public class AccuracyResult
{
    //타이밍 정확도 읽기 프로퍼티
    public float TimingAccuray { get; }
    //위치 정확도 읽기 프로퍼티
    public float PositionAccuracy { get; }

    public AccuracyResult(float tAcc, float pAcc)
    {
        TimingAccuray = Mathf.Clamp01(tAcc);
        PositionAccuracy = Mathf.Clamp01(pAcc);
    }
}
