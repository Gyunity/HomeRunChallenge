using UnityEngine;

public class AccuracyResult
{
    //Ÿ�̹� ��Ȯ�� �б� ������Ƽ
    public float TimingAccuray { get; }
    //��ġ ��Ȯ�� �б� ������Ƽ
    public float PositionAccuracy { get; }

    public AccuracyResult(float tAcc, float pAcc)
    {
        TimingAccuray = Mathf.Clamp01(tAcc);
        PositionAccuracy = Mathf.Clamp01(pAcc);
    }
}
