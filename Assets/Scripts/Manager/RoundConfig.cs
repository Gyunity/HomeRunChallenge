using UnityEngine;

[System.Serializable]
public class RoundConfig
{
	[Tooltip("���� ������")]
	public CurvePitchTrajectory.BallType[] ballTypes;

	[Tooltip("�н�Ʈ�� �ӵ� �ּҰ�")]
	public float minSpeedKmhFast;

    [Tooltip("�н�Ʈ�� �ӵ� �ִ밪")]
	public float maxSpeedKmhFast;

    [Tooltip("��ȭ�� �ӵ� �ּҰ�")]
    public float minSpeedKmhBreaking;

    [Tooltip("��ȭ�� �ӵ� �ִ밪")]
    public float maxSpeedKmhBreaking;
}
