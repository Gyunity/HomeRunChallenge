using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// 마우스/터치 입력을 받아 전체 타격 파이프라인 실행
/// </summary>
public class HitInputHandler : MonoBehaviour
{
    [Tooltip("타격존 중심 Transform")]
    public Transform batZoneTransform;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 1) 입력 정보 수집
            Vector3 screenPos = (Input.touchCount > 0)? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            //batzoneTransform과 같은 z깊이 값으로 변환
            Vector3 clickWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, batZoneTransform.position.z));

            //입력시점
            float clickTime = Time.time;

            // 2) Timing & Position 판정
            var tJudge = new TimingJudge(PitchingManager.Instance.perfectHitTime);
            var tRes = tJudge.Evaluate(clickTime);

            float pAcc = new PositionJudge(batZoneTransform.position).Evaluate(clickWorld);

            // 3) 속도, 수직/수평 각도 계산
            var (speed, vertAngle, horzAngle) = new HitPhysicsCalculator().Calculate(tRes, pAcc, tJudge.MaxWindow);

            // 4) 공 발사
            GameObject ball = PitchingManager.Instance.CurrentBall;
            // 1) 필수 참조 체크
            if (ball == null)
            {
                Debug.LogError("HitInputHandler: ball null입니다!");
                return;
            }
            if (speed == null)
            {
                Debug.LogError("HitInputHandler: speed null입니다!");
                return;
            }
            if (vertAngle == null)
            {
                Debug.LogError("HitInputHandler: vertAngle null입니다!");
                return;
            }
            if (horzAngle == null)
            {
                Debug.LogError("HitInputHandler: 아직 horzAngle 생성되지 않았습니다!");
                return;
            }
            if (batZoneTransform == null)
            {
                Debug.LogError("HitInputHandler: 아직 batZoneTransform 생성되지 않았습니다!");
                return;
            }
            BallController.Instance.ApplyHit(ball, speed, vertAngle, horzAngle, batZoneTransform.forward);
            // 5) 낙하지점 예측 & 표시
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
