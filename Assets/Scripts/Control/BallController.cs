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

    [SerializeField]
    private SlideBanner homeRunBanner;
    public bool hitDisHomerun;

    /// <param name="ball">공 GameObject</param>
    /// <param name="speed">초기 속도(km/h)</param>
    /// <param name="vert">수직 각도(도)</param>
    /// <param name="horz">수평 각도(도)</param>
    /// <param name="forward">타격 방향 벡터</param>

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)){
            homeRunBanner.PlayOnce();

        }
    }
    public void ApplyHit(GameObject ball, float speed, float vert, float horz, Vector3 batHitPoint)
    {
        var rb = ball.GetComponent<Rigidbody>();
        var traj = ball.GetComponent<CurvePitchTrajectory>();
        if (traj != null)
        {
            traj.Stop();
            rb.useGravity = true;
        }

        //반사 방향 계산
        Vector3 baseDir = ballDir.forward;


        // 수평 평면으로 납작하게 (y=0인 기준 방향)
        Vector3 flatFwd = Vector3.ProjectOnPlane(baseDir, Vector3.up);
        if (flatFwd.sqrMagnitude < 1e-6f) flatFwd = Vector3.forward; // 안전장치
        flatFwd.Normalize();

        // 좌우 각도
        Quaternion yaw = Quaternion.AngleAxis(horz, Vector3.up);
        Vector3 yawedFwd = (yaw * flatFwd).normalized;

        // 위아래
        Vector3 rightAxis = Vector3.Cross(Vector3.up, yawedFwd).normalized;
        Quaternion pitch = Quaternion.AngleAxis(-vert, rightAxis);
        
        Vector3 dir = (pitch * yawedFwd).normalized;

        //스피드 최소값 세팅
        float outSpeed = Mathf.Max(speed, 50f);


        rb.linearVelocity = dir * outSpeed;

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
                VibrationManager.Vibrate();
                homeRunBanner.PlayOnce();
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
    /// p0에서 v0로 발사 → y=yTarget에 도달하는 "하강" 시점의 시간/수평거리/착지점(XZ) 반환.
    /// 공기저항 무시, 중력: Physics.gravity.y 사용.
    /// </summary>
    public static bool RangeAtHeight(
        Vector3 p0, Vector3 v0, float yTarget,
        out float tHit, out float rangeXZ, out Vector3 landingPoint)
    {
        float gy = Physics.gravity.y;            // 보통 -9.81
        float a = 0.5f * gy;
        float b = v0.y;
        float c = p0.y - yTarget;

        // 특이 케이스: 중력 0
        if (Mathf.Abs(a) < 1e-6f)
        {
            tHit = 0f; rangeXZ = 0f; landingPoint = p0;
            return false;
        }

        // 판별식
        float D = b * b - 4f * a * c;
        if (D < 0f)
        {
            tHit = 0f; rangeXZ = 0f; landingPoint = p0;
            return false; // 해당 높이에 닿지 않음
        }

        float sqrtD = Mathf.Sqrt(D);
        float t1 = (-b + sqrtD) / (2f * a);
        float t2 = (-b - sqrtD) / (2f * a);

        // 양의 해만 후보로
        const float EPS = 1e-5f;
        float[] candidates = new float[2];
        int n = 0;
        if (t1 > EPS) candidates[n++] = t1;
        if (t2 > EPS) candidates[n++] = t2;
        if (n == 0)
        {
            tHit = 0f; rangeXZ = 0f; landingPoint = p0;
            return false;
        }

        // 1순위: 하강 중인 해(vy<0) 선택, 없으면 가장 큰 양수(보통 하강) 선택
        float ChooseTHit(float t)
        {
            float vy = b + 2f * a * t; // 그 시점의 수직 속도
            return vy < 0f ? 2f : 1f;  // 큰 가중치(우선순위)
        }
        System.Array.Sort(candidates, (tA, tB) =>
        {
            int pA = (ChooseTHit(tA), tA).GetHashCode(); // dummy
            // 커스텀 정렬 대신 아래 로직으로 간단 정렬
            float vyA = b + 2f * a * tA;
            float vyB = b + 2f * a * tB;
            // 하강(t with vy<0) 우선, 같다면 큰 t(보통 하강)을 선택하게
            if ((vyA < 0f) != (vyB < 0f)) return (vyB < 0f) ? 1 : -1;
            return tA.CompareTo(tB);
        });

        // 위 정렬로 인해 마지막 원소가 가장 바람직한 후보가 되게끔 선택
        tHit = candidates[n - 1];

        // 착지점(XZ) & 수평거리
        Vector3 land = new Vector3(
            p0.x + v0.x * tHit,
            yTarget,
            p0.z + v0.z * tHit
        );
        landingPoint = land;

        Vector2 p0xz = new Vector2(p0.x, p0.z);
        Vector2 lxz = new Vector2(land.x, land.z);
        rangeXZ = Vector2.Distance(p0xz, lxz);

        return true;
    }
}