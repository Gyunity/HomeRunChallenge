using UnityEngine;

public class PositionJudge
{
    //타격 위치와 공 도달 위치의 거리차이로 정확도(0~1)를 계산

    private readonly Vector3 expectedPoint;
    //허용 최대 거리 (m)
    private readonly float maxDistance = 1.5f;

    public PositionJudge(Vector3 expected) => expectedPoint = expected;

    public float Evaluate(Vector3 inputPoint)
    {
        float delta = Vector3.Distance(inputPoint, expectedPoint);
        return Mathf.Clamp01(1f - delta / maxDistance);
    }


}
