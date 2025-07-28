using UnityEngine;
/// <summary>
/// 생성된 Ball 프리팹의 Rigidbody에 Calculate된 속도 방향 적용
/// </summary>

public class BallController : MonoBehaviour
{
    public static BallController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    /// <param name="ball">공 GameObject</param>
    /// <param name="speed">초기 속도(km/h)</param>
    /// <param name="vert">수직 각도(도)</param>
    /// <param name="horz">수평 각도(도)</param>
    /// <param name="forward">타격 방향 벡터</param>

    public void ApplyHit(GameObject ball, float speed, float vert, float horz, Vector3 forward, Vector3 batHitPoint)
    {
        var rb = ball.GetComponent<Rigidbody>();
        var traj = ball.GetComponent<CurvePitchTrajectory>();
        if (traj != null)
        {
            traj.Stop();
            rb.useGravity = true;
        }

        //반사 방향 계산
        Vector3 inDir = rb.linearVelocity.normalized;
        Vector3 baseDir = -inDir;

        //세로 회전 -> 가로 회전 순으로 방향 벡터 생성
        Quaternion rotVert = Quaternion.Euler(vert, 0, 0);
        Quaternion rotHorz = Quaternion.Euler(0, horz, 0);
        Vector3 dir = rotHorz * rotVert * baseDir;

        //스피드 최소값 세팅
        if (speed < 50)
            speed = 50;


        // ForceMode.VelocityChange 써서 속도를 직접 세팅
        rb.linearVelocity = dir * speed;

        ////랜딩 위치 구하기
        //Vector3 initialVel = dir * (speed * 0.2777f); // km/h → m/s

        //// 2) 비행 시간, 수평 거리, 착지 위치
        //float flightTime = TrajectoryPredictor.GetFlightTime(initialVel);
        //float distance = TrajectoryPredictor.GetHorizontalDistance(initialVel);
        //Vector3 landing = TrajectoryPredictor.GetLandingPosition(
        //                        batHitPoint, initialVel);

        //// 3) 디버그/시각화
        //Debug.Log($"FlightTime={flightTime:F2}s, Distance={distance:F1}m");
        //LandingVisualizer.Instance.ShowLandingSpot(
        //    distance, batHitPoint, dir);
    }


}
