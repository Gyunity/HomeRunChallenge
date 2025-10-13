using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
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
    [SerializeField]
    private Transform ballDir;

    public bool hitDisHomerun;

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
        Vector3 baseDir = -ballDir.forward;

        //세로 회전 -> 가로 회전 순으로 방향 벡터 생성
        Quaternion look = Quaternion.LookRotation(baseDir, -Vector3.up);
        Quaternion localRot = Quaternion.Euler(vert, horz, 0f);
        Vector3 dir = look * localRot * Vector3.forward;

        //스피드 최소값 세팅
        float outSpeed = Mathf.Max(speed, 50f);


        // ForceMode.VelocityChange 써서 속도를 직접 세팅
        rb.linearVelocity = dir.normalized * outSpeed;

        // --- 착지 예측 (batHitPoint.y 와 같은 높이에 도달할 때) ---
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
            Debug.Log($"착지까지 시간 {tHit:F2}s, 수평거리 {rangeXZ:F2}m, 착지점 {land}");


        }
    }


}
public static class BallRangeUtil
{
    /// <summary>
    /// 현재 위치 p0에서 초기속도 v0로 쐈을 때,
    /// y = yTarget에 도달하는 시간/수평거리/착지점(XZ)을 계산한다. (공기저항 무시)
    /// </summary>
    public static bool RangeAtHeight(Vector3 p0, Vector3 v0, float yTarget,
                                     out float tHit, out float rangeXZ, out Vector3 landingPoint)
    {
        float gy = Physics.gravity.y;                 // 보통 -9.81
        float a = 0.5f * gy;
        float b = v0.y;
        float c = p0.y - yTarget;

        float D = b * b - 4f * a * c;                 // 판별식
        if (D < 0f || Mathf.Abs(a) < 1e-6f)
        {
            tHit = 0f; rangeXZ = 0f; landingPoint = p0;
            return false; // 해당 높이에 도달하지 않음(혹은 중력 0)
        }

        // 내려가며 yTarget을 지나는 해 선택
        float sqrtD = Mathf.Sqrt(D);
        float t1 = (-b + sqrtD) / (2f * a);
        float t2 = (-b - sqrtD) / (2f * a);
        // 양수 중 작은 값이 보통 상승 중 교차, 큰 값이 하강 중 교차.
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
            yTarget,                            // 목표 높이
            p0.z + v0.z * tHit
        );
        return true;
    }
}
