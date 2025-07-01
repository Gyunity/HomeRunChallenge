using UnityEngine;
/// <summary>
/// �ʱ� �ӵ��� �߻� �����κ��� �߷¸� ����� ���� �Ÿ� ����
/// </summary>
public class TrajectoryPredictor
{
    /// <param name="speedKmh">km/h</param>
    /// <param name="angleDeg">�߻� ����(��)</param>

    public float PredictDistance(float speedKmh, float angleDeg)
    {
        // km/h -> m/s
        float v = speedKmh * 0.2777f;
        // �߷� ���ӵ�
        float g = 9.81f;
        float rad = angleDeg * Mathf.Deg2Rad;
        float flightTime = 2 * v * Mathf.Sin(rad) / g;
        return v * Mathf.Cos(rad) * flightTime;
    }
}
