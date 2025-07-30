using UnityEngine;

[System.Serializable]
public class RoundConfig
{
	[Tooltip("던질 구종들")]
	public CurvePitchTrajectory.BallType[] ballTypes;

	[Tooltip("속도 최소값")]
	public float minSpeedKmh;

    [Tooltip("속도 최대값")]
	public float maxSpeedKmh;
}
