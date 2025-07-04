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

        // �� ����
        CurrentBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);

        //Ÿ�� �� ���
        perfectHitTime = Time.time + pitchDuration;

        // Rigidbody.Velocity�� �� ������
        Rigidbody rb = CurrentBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //targetPoint �������� ���� �ӵ��� �̵�
            Vector3 dir = (targetPoint.position - spawnPoint.position).normalized;
            float distance = Vector3.Distance(spawnPoint.position, targetPoint.position);

            float speed = distance / pitchDuration;
            rb.linearVelocity = dir * speed;
        }
        else
        {
            Debug.LogWarning("PitchingManager: Ball prefab is missing a Rigidbody.");
        }
    }
}
