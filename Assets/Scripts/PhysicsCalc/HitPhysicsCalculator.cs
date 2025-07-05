using UnityEngine;

    /// <summary>
    /// 타이밍, 위치 정확도에 따라 공의 초기 속도, 세로각 가로각 계산
    /// </summary>
public class HitPhysicsCalculator
{

    // km/h 기준 속도
    private readonly float basePower = 120f;
    //최소 각도
    private readonly float minVertAngle = 5f;
    //최대 각도
    private readonly float maxVertAngle = 35f;
    // 좌우 최대 편차
    private readonly float maxHorzAngle = 60f;

    /// <param name="tRes">TimingResult (Offset, Accuracy)</param>
    /// <param name="positionAccuracy">0~1</param>
    /// <param name="timingWindow">TimingJudge.MaxWindow</param>
    public (float speed, float vertAngle, float horzAngle)
        Calculate(TimingResult tRes, float positionAccuracy, float timingWindow)
    {
        // 힘 보정 : 타이밍x위치 정확도
        float power = basePower * tRes.Accuracy * positionAccuracy;
        // 수직 각도 : 정확도 보정
        float vert = Mathf.Lerp(minVertAngle, maxVertAngle, tRes.Accuracy);
        // 수평 각도 : offset 부호로 얼리/레이트 구분
        float normOffset = Mathf.Clamp(tRes.Offset / timingWindow, -1f, 1f);
        float horz = normOffset * maxHorzAngle;

        return (power, vert, horz);
    }

}
