using UnityEngine;

public struct TimingResult
{
    //(Ÿ�� ������ ���ϱ� ���� ���� = ���� ��� = ������)
    public float Offset;
    public float Accuracy;

    public TimingResult(float offset, float accuracy)
    {
        Offset = offset;
        Accuracy = accuracy;
    }
}

public class TimingJudge
{
    private float perfectTime;
    //���� ��� �ִ�ġ
    private float maxWindow = 0.3f;

    //���� �����ؾ� �� ����
    public TimingJudge(float expectedTime) => perfectTime = expectedTime;

    //Ÿ�ݽð��� �� ���� �ð� ���̸� ��� �� ��Ȯ��(0~1) ����
    public TimingResult Evaluate(float inputTime)
    {
        float rawDelta = inputTime - perfectTime;
        float accuracy = 1f - Mathf.Abs(rawDelta)/ maxWindow;
        return new TimingResult(rawDelta, accuracy);
    }
}
