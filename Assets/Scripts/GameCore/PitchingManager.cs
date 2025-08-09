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

    //[Tooltip("투구 후 공이 타격 존까지 도달하는데 걸리는 시간")]
    //public float pitchDuration = 1.0f;
    [SerializeField]
    private HitInputHandler hitInputHandler;

    // 타격 타이밍
    public float perfectHitTime { get; private set; }

    //마지막으로 생성된 공
    public GameObject CurrentBall { get; private set; }


    public void PitchBall(CurvePitchTrajectory.BallType type, float speedKmh)
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
        float distance = Vector3.Distance(spawnPoint.position, targetPoint.position);
        float duration = distance / (speedKmh * 0.2777f);
        perfectHitTime = Time.time + duration;
        hitInputHandler.ResetHitFlag();

        // Rigidbody.Velocity로 공 던지기
        //Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        CurvePitchTrajectory traj = CurrentBall.GetComponent<CurvePitchTrajectory>();
        Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        if (traj != null)
        {
            // 공 방향 설정
            Vector3 dir = (targetPoint.position - spawnPoint.position).normalized;
            rb.linearVelocity = dir * 0.001f;

            //구종 설정
            traj.ballType = type;
            Debug.Log(type);
            traj.Launch(spawnPoint.position, targetPoint.position, duration);
        }
        else
        {
            Debug.LogWarning("PitchingManager: Ball prefab is missing a CurvePitchTrajectory.");
        }
    }

    private void TargetPointRandomSet()
    {
        //랜덤값 세팅
        float ranX = UnityEngine.Random.Range(-1.25f, 1.25f);
        float ranY = UnityEngine.Random.Range(1.4f, 4.3f);
        //targetpoint 랜덤 설정
        targetPoint.position = new Vector3(ranX, ranY, targetPoint.position.z);
    }
}
