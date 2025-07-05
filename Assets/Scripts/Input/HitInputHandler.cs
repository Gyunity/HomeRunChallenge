using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// 마우스/터치 입력을 받아 전체 타격 파이프라인 실행
/// </summary>
public class HitInputHandler : MonoBehaviour
{


    [Tooltip("타격존 중심 Transform")]
    public Transform batZoneTransform;

    [SerializeField] private LayerMask hitLayerMask;

    //타격 횟수 제한
    private bool battingCan = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            if (!battingCan)
                return;
            battingCan = false;

            // 1) 입력 정보 수집
            Vector3 screenPos = (Input.touchCount > 0) ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;

            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayerMask))
            {
                Debug.LogWarning("HitInputHandler : 베팅존이 아닙니다.");
                return;
            }

            //batzoneTransform과 같은 z깊이 값으로 변환
            //Vector3 clickWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, batZoneTransform.position.z));
            Vector3 clickWorld = hit.point;

            //입력시점
            float clickTime = Time.time;

            // 2) Timing & Position 판정
            var tJudge = new TimingJudge(PitchingManager.Instance.perfectHitTime);
            TimingResult tRes = tJudge.Evaluate(clickTime);
            Debug.Log("판정 결과" + tRes.Accuracy + "  " + tRes.Offset);


            float pAcc = new PositionJudge(PitchingManager.Instance.targetPoint.position).Evaluate(clickWorld);
            Debug.Log($"클릭 위치 x : {clickWorld.x} y : {clickWorld.y} z : {clickWorld.z} <{pAcc}>");

            //타격 범위 0.2초거나 타격 범위가 많이 벗어나면헛스윙
            if (tRes.Accuracy < 0 || pAcc == 0)
            {
                Debug.Log("헛스윙!");
                return;
            }
            // 3) 속도, 수직/수평 각도 계산
            var (speed, vertAngle, horzAngle) = new HitPhysicsCalculator().Calculate(tRes, pAcc, tJudge.MaxWindow);

            // 4) 공 발사
            GameObject ball = PitchingManager.Instance.CurrentBall;
            ball.GetComponent<Rigidbody>().useGravity = true;
            BallController.Instance.ApplyHit(ball, speed, vertAngle, horzAngle, PitchingManager.Instance.spawnPoint.position - PitchingManager.Instance.targetPoint.position);
            // 5) 낙하지점 예측 & 표시
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
