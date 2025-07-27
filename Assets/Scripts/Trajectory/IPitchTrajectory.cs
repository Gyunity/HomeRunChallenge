using UnityEngine;
// 투수 궤적을 담당하는 인터페이스
public interface IPitchTrajectory
{
    //궤적 연산
    void Launch(Vector3 start, Vector3 end, float duration);

    //결과 시간 만큼 진행된 후의 위치 계산 후 반환
    Vector3 GetPosition(float elapsedTime);

    //궤적이 끝나면 true
    bool IsFinished { get; }
}
