using UnityEngine;
/// <summary>
/// 초기 속도와 발사 각도로부터 중력만 고려한 비행 거리 예측
/// </summary>
public class TrajectoryPredictor
{
    /// <param name="speedKmh">km/h</param>
    /// <param name="angleDeg">발사 각도(도)</param>

    public float PredictDistance(float speedKmh, float angleDeg)
    {
        // km/h -> m/s
        float v = speedKmh * 0.2777f;
        // 중력 가속도
        float g = 9.81f;
        float rad = angleDeg * Mathf.Deg2Rad;
        float flightTime = 2 * v * Mathf.Sin(rad) / g;
        return v * Mathf.Cos(rad) * flightTime;
    }
}
