using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// ���콺/��ġ �Է��� �޾� ��ü Ÿ�� ���������� ����
/// </summary>
public class HitInputHandler : MonoBehaviour
{


    [Tooltip("Ÿ���� �߽� Transform")]
    public Transform batZoneTransform;

    [SerializeField] private LayerMask hitLayerMask;

    //Ÿ�� Ƚ�� ����
    private bool battingCan = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (!battingCan)
                return;
            battingCan = false;

            // 1) �Է� ���� ����
            Vector3 screenPos = (Input.touchCount > 0) ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayerMask))
            {
                Debug.LogWarning("HitInputHandler : �������� �ƴմϴ�.");
                return;
            }

            //batzoneTransform�� ���� z���� ������ ��ȯ
            //Vector3 clickWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, batZoneTransform.position.z));
            Vector3 clickWorld = hit.point;

            //�Է½���
            float clickTime = Time.time;

            // 2) Timing & Position ����
            var tJudge = new TimingJudge(PitchingManager.Instance.perfectHitTime);
            TimingResult tRes = tJudge.Evaluate(clickTime);
            Debug.Log("���� ���" + tRes.Accuracy + "  " + tRes.Offset);


            float pAcc = new PositionJudge(PitchingManager.Instance.targetPoint.position).Evaluate(clickWorld);
            Debug.Log($"Ŭ�� ��ġ x : {clickWorld.x} y : {clickWorld.y} z : {clickWorld.z} <{pAcc}>");

            //Ÿ�� ���� 0.2�ʰų� Ÿ�� ������ ���� ������꽺��
            if (tRes.Accuracy < 0 || pAcc == 0)
            {
                Debug.Log("�꽺��!");
                return;
            }
            // 3) �ӵ�, ����/���� ���� ���
            var (speed, vertAngle, horzAngle) = new HitPhysicsCalculator().Calculate(tRes, pAcc, tJudge.MaxWindow);

            // 4) �� �߻�
            GameObject ball = PitchingManager.Instance.CurrentBall;
            ball.GetComponent<Rigidbody>().useGravity = true;
            BallController.Instance.ApplyHit(ball, speed, vertAngle, horzAngle, PitchingManager.Instance.spawnPoint.position - PitchingManager.Instance.targetPoint.position);
            // 5) �������� ���� & ǥ��
            //float distance = new TrajectoryPredictor().PredictDistance(speed, vertAngle);
            //LandingVisualizer.Instance.ShowLandingSpot(distance, batZoneTransform.position, batZoneTransform.forward);


        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PitchingManager.Instance.PitchBall();
            battingCan = true;
        }
    }
}
public static class Vector3Extensions
{
    public static Vector3 WithZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }
}
