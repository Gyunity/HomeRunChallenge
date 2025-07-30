using UnityEngine;

[System.Serializable]
public class RoundConfig
{
	[Tooltip("���� ������")]
	public CurvePitchTrajectory.BallType[] ballTypes;

	[Tooltip("�ӵ� �ּҰ�")]
	public float minSpeedKmh;

    [Tooltip("�ӵ� �ִ밪")]
	public float maxSpeedKmh;
}
