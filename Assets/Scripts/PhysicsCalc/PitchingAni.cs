using UnityEngine;

public class PitchingAni : MonoBehaviour
{
    [SerializeField]
    private PitchingManager pitchingManager;

    private CurvePitchTrajectory.BallType currentType = CurvePitchTrajectory.BallType.FOURSEAM;

    private float currentSpeed = 100f;

    private void Pitching()
    {
        pitchingManager.PitchBall(currentType, currentSpeed);
        Debug.Log($"�� ���� : {currentType}, �� ���ǵ� : {currentSpeed}");
    }

    public void PichingSet(CurvePitchTrajectory.BallType type, float speedKmh)
    {
        currentType = type;
        currentSpeed = speedKmh;
    }


}
