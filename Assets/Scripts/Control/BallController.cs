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
    
    public void ApplyHit(GameObject ball, float speed, float vert, float horz, Vector3 forward)
    {
        var rb = ball.GetComponent<Rigidbody>();

        //반사 방향 계산
        Vector3 inDir = rb.linearVelocity.normalized;
        Vector3 baseDir = -inDir;

        //세로 회전 -> 가로 회전 순으로 방향 벡터 생성
        Quaternion rotVert = Quaternion.Euler(vert, 0, 0);
        Quaternion rotHorz = Quaternion.Euler(0, horz, 0);
       // Vector3 baseDir = -forward.normalized;
        
        Vector3 dir = rotHorz * rotVert * baseDir;
        //Vector3 dir =  forward.normalized;

        // ForceMode.VelocityChange 써서 속도를 직접 세팅
        rb.linearVelocity = dir * speed;
        // rb.AddForce(dir * speed, ForceMode.VelocityChange);
        
    }


}
