using UnityEngine;

public class PitchingAni : MonoBehaviour
{
    [SerializeField]
    private PitchingManager pitchingManager;
    [SerializeField]
    private GameFlowManager gameFlowManager;

    private CurvePitchTrajectory.BallType currentType = CurvePitchTrajectory.BallType.FOURSEAM;

    private float currentSpeed = 100f;

    private void Pitching()
    {
        pitchingManager.PitchBall(currentType, currentSpeed);
        gameFlowManager.ThorwTrue();
        Debug.Log($"볼 종류 : {currentType}, 볼 스피드 : {currentSpeed}");
    }

    public void PichingSet(CurvePitchTrajectory.BallType type, float speedKmh)
    {
        currentType = type;
        currentSpeed = speedKmh;
    }


}
