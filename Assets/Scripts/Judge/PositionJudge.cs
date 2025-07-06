using UnityEngine;

public class PositionJudge
{
    //Ÿ�� ��ġ�� �� ���� ��ġ�� �Ÿ����̷� ��Ȯ��(0~1)�� ���

    private readonly Vector3 expectedPoint;
    //��� �ִ� �Ÿ� (m)
    private readonly float maxDistance = 1.5f;

    public PositionJudge(Vector3 expected) => expectedPoint = expected;

    public float Evaluate(Vector3 inputPoint)
    {
        float delta = Vector3.Distance(inputPoint, expectedPoint);
        return Mathf.Clamp01(1f - delta / maxDistance);
    }


}
