using UnityEngine;
// ���� ������ ����ϴ� �������̽�
public interface IPitchTrajectory
{
    //���� ����
    void Launch(Vector3 start, Vector3 end, float duration);

    //��� �ð� ��ŭ ����� ���� ��ġ ��� �� ��ȯ
    Vector3 GetPosition(float elapsedTime);

    //������ ������ true
    bool IsFinished { get; }
}
