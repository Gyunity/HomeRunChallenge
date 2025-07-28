using System;
using UnityEngine;

public class PitchingManager : MonoBehaviour
{
    public static PitchingManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    [Header("Ball Setting")]
    [Tooltip("던질 공 프리팹 (Rigidbody 포함)")]
    public GameObject ballPrefab;

    [Tooltip("공이 생성될 위치")]
    public Transform spawnPoint;

    [Tooltip("타격 존으로 사용할 Transform")]
    public Transform targetPoint;

    [Tooltip("투구 후 공이 타격 존까지 도달하는데 걸리는 시간")]
    public float pitchDuration = 1.0f;


    // 타격 타이밍
    public float perfectHitTime { get; private set; }

    //마지막으로 생성된 공
    public GameObject CurrentBall { get; private set; }


    public void PitchBall()
    {
        if (ballPrefab == null || spawnPoint == null || targetPoint == null)
        {
            Debug.LogWarning("PitchingManager: ballPrefab, spawnPoint, or targetPoint not assigned.");
            return;
        }

        if (CurrentBall != null)
            Destroy(CurrentBall);

        //타겟포인트 랜덤 세팅
        TargetPointRandomSet();


        // 공 생성
        CurrentBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);

        //타격 점 계산
        perfectHitTime = Time.time + pitchDuration;

        // Rigidbody.Velocity로 공 던지기
        //Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        IPitchTrajectory traj = CurrentBall.GetComponent<IPitchTrajectory>();
        Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        if (traj != null)
        {
            //1).직구만 적용 코드
            //targetPoint 방향으로 일정 속도로 이동
            Vector3 dir = (targetPoint.position - spawnPoint.position).normalized;
            rb.linearVelocity = dir*0.001f;
            //float distance = Vector3.Distance(spawnPoint.position, targetPoint.position);

            //float speed = distance / pitchDuration;

            //2). 변화구 적용 코드
            if (traj is CurvePitchTrajectory curveTraj)
            {
                Array types = Enum.GetValues(typeof(CurvePitchTrajectory.BallType));
                int idx = UnityEngine.Random.Range(0, types.Length);
                curveTraj.ballType = (CurvePitchTrajectory.BallType)types.GetValue(idx);
                Debug.Log(curveTraj.ballType);
            
            }
            traj.Launch(spawnPoint.position, targetPoint.position, pitchDuration);
        }
        else
        {
            Debug.LogWarning("PitchingManager: Ball prefab is missing a Rigidbody.");
        }
    }

    private void TargetPointRandomSet()
    {
        //랜덤값 세팅
        float ranX = UnityEngine.Random.Range(-1.5f, 1.5f);
        float ranY = UnityEngine.Random.Range(0.8f, 4.2f);
        //targetpoint 랜덤 설정
        targetPoint.position = new Vector3(ranX, ranY, targetPoint.position.z);
    }
}
