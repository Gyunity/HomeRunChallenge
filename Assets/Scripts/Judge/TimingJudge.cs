using UnityEngine;

public struct TimingResult
{
    //(타격 방향을 정하기 위한 음수 = 왼쪽 양수 = 오른쪽)
    public float Offset;
    public float Accuracy;

    public TimingResult(float offset, float accuracy)
    {
        Offset = offset;
        Accuracy = accuracy;
    }
}

public class TimingJudge
{
    private readonly float perfectTime;
    //오차 허용 최대치
    private readonly float maxWindow = 0.1f;

    //공이 도달해야 할 시점
    public TimingJudge(float expectedTime) => perfectTime = expectedTime;

    public float MaxWindow => maxWindow;

    //타격시간과 공 도착 시간 차이를 계산 후 정확도(0~1) 산출
    public TimingResult Evaluate(float inputTime)
    {
        float rawDelta = inputTime - perfectTime;
        float accuracy = 1f - Mathf.Abs(rawDelta)/ maxWindow;
        return new TimingResult(rawDelta, accuracy);
    }
}
