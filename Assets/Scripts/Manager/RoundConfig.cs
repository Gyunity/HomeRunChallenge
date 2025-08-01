using UnityEngine;

[System.Serializable]
public class RoundConfig
{
	[Tooltip("던질 구종들")]
	public CurvePitchTrajectory.BallType[] ballTypes;

	[Tooltip("패스트볼 속도 최소값")]
	public float minSpeedKmhFast;

    [Tooltip("패스트볼 속도 최대값")]
	public float maxSpeedKmhFast;

    [Tooltip("변화구 속도 최소값")]
    public float minSpeedKmhBreaking;

    [Tooltip("변화구 속도 최대값")]
    public float maxSpeedKmhBreaking;
}
