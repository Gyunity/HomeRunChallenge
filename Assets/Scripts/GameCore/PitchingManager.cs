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
    [Tooltip("���� �� ������ (Rigidbody ����)")]
    public GameObject ballPrefab;

    [Tooltip("���� ������ ��ġ")]
    public Transform spawnPoint;

    [Tooltip("Ÿ�� ������ ����� Transform")]
    public Transform targetPoint;

    //[Tooltip("���� �� ���� Ÿ�� ������ �����ϴµ� �ɸ��� �ð�")]
    //public float pitchDuration = 1.0f;
    [SerializeField]
    private HitInputHandler hitInputHandler;

    // Ÿ�� Ÿ�̹�
    public float perfectHitTime { get; private set; }

    //���������� ������ ��
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

        //Ÿ������Ʈ ���� ����
        TargetPointRandomSet();


        // �� ����
        CurrentBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);

        //Ÿ�� �� ���
        float distance = Vector3.Distance(spawnPoint.position, targetPoint.position);
        float duration = distance / (speedKmh * 0.2777f);
        perfectHitTime = Time.time + duration;
        hitInputHandler.ResetHitFlag();

        // Rigidbody.Velocity�� �� ������
        //Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        CurvePitchTrajectory traj = CurrentBall.GetComponent<CurvePitchTrajectory>();
        Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        if (traj != null)
        {
            // �� ���� ����
            Vector3 dir = (targetPoint.position - spawnPoint.position).normalized;
            rb.linearVelocity = dir * 0.001f;

            //���� ����
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
        //������ ����
        float ranX = UnityEngine.Random.Range(-1.25f, 1.25f);
        float ranY = UnityEngine.Random.Range(1.4f, 4.3f);
        //targetpoint ���� ����
        targetPoint.position = new Vector3(ranX, ranY, targetPoint.position.z);
    }
}
