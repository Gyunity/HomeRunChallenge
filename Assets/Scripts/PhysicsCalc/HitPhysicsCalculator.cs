using UnityEngine;

    /// <summary>
    /// 타이밍, 위치 정확도에 따라 공의 초기 속도, 세로각 가로각 계산
    /// </summary>
public class HitPhysicsCalculator
{

    // km/h 기준 속도
    private readonly float basePower = 400f;
    //최소 각도
    private readonly float minVertAngle = 0f;
    //최대 각도
    private readonly float maxVertAngle = 30f;
    // 좌우 최대 편차
    private readonly float maxHorzAngle = 40f;
    // km/h -> m/s
    private const float kMH2MPS = 0.2777778f;
    /// <param name="tRes">TimingResult (Offset, Accuracy)</param>
    /// <param name="positionAccuracy">0~1</param>
    /// <param name="timingWindow">TimingJudge.MaxWindow</param>
    public (float speed, float vertAngle, float horzAngle)
        Calculate(TimingResult tRes, float positionAccuracy, float timingWindow)
    {
        float acc = Mathf.Clamp01(tRes.Accuracy);
        float posAcc = Mathf.Clamp01(positionAccuracy);

        // 힘 보정 : 타이밍x위치 정확도
        float kmh = basePower * acc * posAcc;
        float speed = kmh * kMH2MPS;

        // 수직 각도 : 정확도 보정
        float vert = Mathf.Lerp(minVertAngle, maxVertAngle, tRes.Accuracy);
        // 수평 각도 : offset 부호로 얼리/레이트 구분
        float normOffset = Mathf.Clamp(tRes.Offset / timingWindow, -1f, 1f);
        float horz = normOffset * maxHorzAngle;

        return (speed, vert, horz);
    }

}
