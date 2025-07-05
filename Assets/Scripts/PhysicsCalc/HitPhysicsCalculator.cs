using UnityEngine;

    /// <summary>
    /// Ÿ�̹�, ��ġ ��Ȯ���� ���� ���� �ʱ� �ӵ�, ���ΰ� ���ΰ� ���
    /// </summary>
public class HitPhysicsCalculator
{

    // km/h ���� �ӵ�
    private readonly float basePower = 120f;
    //�ּ� ����
    private readonly float minVertAngle = 5f;
    //�ִ� ����
    private readonly float maxVertAngle = 35f;
    // �¿� �ִ� ����
    private readonly float maxHorzAngle = 60f;

    /// <param name="tRes">TimingResult (Offset, Accuracy)</param>
    /// <param name="positionAccuracy">0~1</param>
    /// <param name="timingWindow">TimingJudge.MaxWindow</param>
    public (float speed, float vertAngle, float horzAngle)
        Calculate(TimingResult tRes, float positionAccuracy, float timingWindow)
    {
        // �� ���� : Ÿ�̹�x��ġ ��Ȯ��
        float power = basePower * tRes.Accuracy * positionAccuracy;
        // ���� ���� : ��Ȯ�� ����
        float vert = Mathf.Lerp(minVertAngle, maxVertAngle, tRes.Accuracy);
        // ���� ���� : offset ��ȣ�� ��/����Ʈ ����
        float normOffset = Mathf.Clamp(tRes.Offset / timingWindow, -1f, 1f);
        float horz = normOffset * maxHorzAngle;

        return (power, vert, horz);
    }

}
