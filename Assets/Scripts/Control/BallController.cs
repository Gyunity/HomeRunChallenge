using UnityEngine;
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
    /// <param name="ball">�� GameObject</param>
    /// <param name="speed">�ʱ� �ӵ�(km/h)</param>
    /// <param name="vert">���� ����(��)</param>
    /// <param name="horz">���� ����(��)</param>
    /// <param name="forward">Ÿ�� ���� ����</param>
    
    public void ApplyHit(GameObject ball, float speed, float vert, float horz, Vector3 forward)
    {
        var rb = ball.GetComponent<Rigidbody>();

        //���� ȸ�� -> ���� ȸ�� ������ ���� ���� ����
        Quaternion rotVert = Quaternion.Euler(-vert, 0, 0);
        Quaternion rotHorz = Quaternion.Euler(0, horz, 0);
        Vector3 dir = rotHorz * rotVert * forward.normalized;

        // ForceMode.VelocityChange �Ἥ �ӵ��� ���� ����
        rb.AddForce(dir * speed * 0.2777f, ForceMode.VelocityChange);
    }


}
