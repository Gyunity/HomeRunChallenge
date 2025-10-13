using UnityEngine;

    /// <summary>
    /// Ÿ�̹�, ��ġ ��Ȯ���� ���� ���� �ʱ� �ӵ�, ���ΰ� ���ΰ� ���
    /// </summary>
public class HitPhysicsCalculator
{

    // km/h ���� �ӵ�
    private readonly float basePower = 400f;
    //�ּ� ����
    private readonly float minVertAngle = 0f;
    //�ִ� ����
    private readonly float maxVertAngle = 30f;
    // �¿� �ִ� ����
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

        // �� ���� : Ÿ�̹�x��ġ ��Ȯ��
        float kmh = basePower * acc * posAcc;
        float speed = kmh * kMH2MPS;

        // ���� ���� : ��Ȯ�� ����
        float vert = Mathf.Lerp(minVertAngle, maxVertAngle, tRes.Accuracy);
        // ���� ���� : offset ��ȣ�� ��/����Ʈ ����
        float normOffset = Mathf.Clamp(tRes.Offset / timingWindow, -1f, 1f);
        float horz = normOffset * maxHorzAngle;

        return (speed, vert, horz);
    }

}
