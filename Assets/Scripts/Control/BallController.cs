using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// ������ Ball �������� Rigidbody�� Calculate�� �ӵ� ���� ����
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
    [SerializeField]
    private Transform ballDir;

    public bool hitDisHomerun;

    /// <param name="ball">�� GameObject</param>
    /// <param name="speed">�ʱ� �ӵ�(km/h)</param>
    /// <param name="vert">���� ����(��)</param>
    /// <param name="horz">���� ����(��)</param>
    /// <param name="forward">Ÿ�� ���� ����</param>

    public void ApplyHit(GameObject ball, float speed, float vert, float horz, Vector3 forward, Vector3 batHitPoint)
    {
        var rb = ball.GetComponent<Rigidbody>();
        var traj = ball.GetComponent<CurvePitchTrajectory>();
        if (traj != null)
        {
            traj.Stop();
            rb.useGravity = true;
        }

        //�ݻ� ���� ���
        Vector3 baseDir = -ballDir.forward;

        //���� ȸ�� -> ���� ȸ�� ������ ���� ���� ����
        Quaternion look = Quaternion.LookRotation(baseDir, -Vector3.up);
        Quaternion localRot = Quaternion.Euler(vert, horz, 0f);
        Vector3 dir = look * localRot * Vector3.forward;

        //���ǵ� �ּҰ� ����
        float outSpeed = Mathf.Max(speed, 50f);


        // ForceMode.VelocityChange �Ἥ �ӵ��� ���� ����
        rb.linearVelocity = dir.normalized * outSpeed;

        // --- ���� ���� (batHitPoint.y �� ���� ���̿� ������ ��) ---
        if (BallRangeUtil.RangeAtHeight(ball.transform.position,
                                        rb.linearVelocity,
                                        batHitPoint.y,
                                        out float tHit,
                                        out float rangeXZ,
                                        out Vector3 land))
        {
            if (rangeXZ > 250f)
            {
                hitDisHomerun = true;
                EffectManager.Instance.PlayEffect(EffectType.PerfectHit, ball.transform.position);
                SoundManager.Instance.PlaySFX("SFX_Perfect", 0f);
                VibrationManager.Vibrate(300);
            }
            else
            {
                hitDisHomerun = false;
                SoundManager.Instance.PlaySFX("SFX_Good", 0f);
            }
            Debug.Log($"�������� �ð� {tHit:F2}s, ����Ÿ� {rangeXZ:F2}m, ������ {land}");


        }
    }


}
public static class BallRangeUtil
{
    /// <summary>
    /// ���� ��ġ p0���� �ʱ�ӵ� v0�� ���� ��,
    /// y = yTarget�� �����ϴ� �ð�/����Ÿ�/������(XZ)�� ����Ѵ�. (�������� ����)
    /// </summary>
    public static bool RangeAtHeight(Vector3 p0, Vector3 v0, float yTarget,
                                     out float tHit, out float rangeXZ, out Vector3 landingPoint)
    {
        float gy = Physics.gravity.y;                 // ���� -9.81
        float a = 0.5f * gy;
        float b = v0.y;
        float c = p0.y - yTarget;

        float D = b * b - 4f * a * c;                 // �Ǻ���
        if (D < 0f || Mathf.Abs(a) < 1e-6f)
        {
            tHit = 0f; rangeXZ = 0f; landingPoint = p0;
            return false; // �ش� ���̿� �������� ����(Ȥ�� �߷� 0)
        }

        // �������� yTarget�� ������ �� ����
        float sqrtD = Mathf.Sqrt(D);
        float t1 = (-b + sqrtD) / (2f * a);
        float t2 = (-b - sqrtD) / (2f * a);
        // ��� �� ���� ���� ���� ��� �� ����, ū ���� �ϰ� �� ����.
        tHit = Mathf.Max(t1, t2);
        if (tHit <= 0f)
        {
            tHit = Mathf.Max(t1, t2);
            if (tHit <= 0f) { rangeXZ = 0f; landingPoint = p0; return false; }
        }

        Vector2 vXZ = new Vector2(v0.x, v0.z);
        rangeXZ = vXZ.magnitude * tHit;

        landingPoint = new Vector3(
            p0.x + v0.x * tHit,
            yTarget,                            // ��ǥ ����
            p0.z + v0.z * tHit
        );
        return true;
    }
}
