using UnityEngine;
/// <summary>
/// 3D 투척체 궤적 예측 유틸리티.
/// 초기 속도 벡터로부터
/// 1) 비행 시간(Time of Flight),
/// 2) 수평 이동 거리(Horizontal Range),
/// 3) 착지 위치(Landing Position)
/// 를 계산합니다.
/// 가정: 공이 출발 높이(y₀)와 같은 높이로 착지./// </summary>
public class TrajectoryPredictor
{
    // 중력 가속도 벡터
    private static float g => Physics.gravity.y;

    // 초기 속도 벡터로부터 비행 시간을 계산 (t = -2 * vᵧ / g)
    public static float GetFlightTime(Vector3 initialVelocity)
    {
        return -2f * initialVelocity.y / g;
    }

    //초기 속도 벡터로부터 수평 이동거리 계산 (distance = |vₕ| * t)
    public static float GetHorizontalDistance(Vector3 initialVelocity)
    {
        Vector3 vH = new Vector3(initialVelocity.x, 0f, initialVelocity.z);
        float t = GetFlightTime(initialVelocity);
        return vH.magnitude * t;
    }

    // 초기 위치(origin)에서 공이 landingHeight (= origin.y)로 돌아올 때의 월드좌표를 계산
    public static Vector3 GetLandingPosition(Vector3 origin, Vector3 initialVelocity)
    {
        Vector3 vH = new Vector3(initialVelocity.x, 0f, initialVelocity.z);
        float t = GetFlightTime(initialVelocity);
        return origin + vH * t;
    }

    
}
