using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// ���콺/��ġ �Է��� �޾� ��ü Ÿ�� ���������� ����
/// </summary>
public class HitInputHandler : MonoBehaviour
{
    [Tooltip("Ÿ���� �߽� Transform")]
    public Transform batZoneTransform;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 1) �Է� ���� ����
            Vector3 screenPos = (Input.touchCount > 0)? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            //batzoneTransform�� ���� z���� ������ ��ȯ
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, batZoneTransform.position.z));

            //�Է½���
            float clickTime = Time.time;

            // 2) Timing & Position ����
            var tJudge = new TimingJudge(PitchingManager.Instance.perfectHitTime);
            var tRes = tJudge.Evaluate(clickTime);

            float pAcc = new PositionJudge(batZoneTransform.position).Evaluate(clickWorld);

            // 3) �ӵ�, ����/���� ���� ���
            var (speed, vertAngle, horzAngle) = new HitPhysicsCalculator().Calculate(tRes, pAcc, tJudge.MaxWindow);

            // 4) �� �߻�
            GameObject ball = PitchingManager.Instance.CurrentBall;
            // 1) �ʼ� ���� üũ
            if (ball == null)
            {
                Debug.LogError("HitInputHandler: ball null�Դϴ�!");
                return;
            }
            if (speed == null)
            {
                Debug.LogError("HitInputHandler: speed null�Դϴ�!");
                return;
            }
            if (vertAngle == null)
            {
                Debug.LogError("HitInputHandler: vertAngle null�Դϴ�!");
                return;
            }
            if (horzAngle == null)
            {
                Debug.LogError("HitInputHandler: ���� horzAngle �������� �ʾҽ��ϴ�!");
                return;
            }
            if (batZoneTransform == null)
            {
                Debug.LogError("HitInputHandler: ���� batZoneTransform �������� �ʾҽ��ϴ�!");
                return;
            }
            BallController.Instance.ApplyHit(ball, speed, vertAngle, horzAngle, batZoneTransform.forward);
            // 5) �������� ���� & ǥ��
            float distance = new TrajectoryPredictor().PredictDistance(speed, vertAngle);
            LandingVisualizer.Instance.ShowLandingSpot(distance, batZoneTransform.position, batZoneTransform.forward);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PitchingManager.Instance.PitchBall();
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
