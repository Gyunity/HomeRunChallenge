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

    [Tooltip("���� �� ���� Ÿ�� ������ �����ϴµ� �ɸ��� �ð�")]
    public float pitchDuration = 1.0f;


    // Ÿ�� Ÿ�̹�
    public float perfectHitTime { get; private set; }

    //���������� ������ ��
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

        //Ÿ������Ʈ ���� ����
        TargetPointRandomSet();


        // �� ����
        CurrentBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);

        //Ÿ�� �� ���
        perfectHitTime = Time.time + pitchDuration;

        // Rigidbody.Velocity�� �� ������
        //Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        IPitchTrajectory traj = CurrentBall.GetComponent<IPitchTrajectory>();
        Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        if (traj != null)
        {
            //1).������ ���� �ڵ�
            //targetPoint �������� ���� �ӵ��� �̵�
            Vector3 dir = (targetPoint.position - spawnPoint.position).normalized;
            rb.linearVelocity = dir*0.001f;
            //float distance = Vector3.Distance(spawnPoint.position, targetPoint.position);

            //float speed = distance / pitchDuration;

            //2). ��ȭ�� ���� �ڵ�
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
        //������ ����
        float ranX = UnityEngine.Random.Range(-1.5f, 1.5f);
        float ranY = UnityEngine.Random.Range(0.8f, 4.2f);
        //targetpoint ���� ����
        targetPoint.position = new Vector3(ranX, ranY, targetPoint.position.z);
    }
}
