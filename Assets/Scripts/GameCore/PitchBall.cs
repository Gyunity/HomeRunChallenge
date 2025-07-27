//using UnityEngine;

//public class PitchBall : MonoBehaviour
//{
//    public enum BallType
//    {
//        FOURSEAM,
//        TWOSEAM,
//        CURVE,
//        SLIDER
//    }
//    [Header("궤적 설정")]
//    public BallType ballType;
//    public Transform startPoint;
//    public Transform endPoint;
//    public float pitchDuration = 1.0f;

//    private AnimationCurve pitchCurveX;
//    private AnimationCurve pitchCurveY;
//    private float timeElapsed;
//    private Rigidbody rb;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        rb.useGravity = false;
//        rb.isKinematic = false;
//    }


//    public void Launch()
//    {
//        timeElapsed = 0f;
//        SetCurves(ballType);
//    }

//    private void FixedUpdate()
//    {
//        if (timeElapsed < pitchDuration) return;

//        timeElapsed += Time.deltaTime;
//        float t = Mathf.Clamp01(timeElapsed / pitchDuration);

//        // 1) 직선 보간
//        Vector3 basePos = Vector3.Lerp(startPoint.position, endPoint.position, t);

//        // 2) 커브 오프셋 계산
//        float offsetX = pitchCurveX.Evaluate(t);
//        float offsetY = pitchCurveY.Evaluate(t);
//        Vector3 targetPos = basePos + new Vector3(offsetX, offsetY, 0f);

//        // 3) RB이동
//        rb.MovePosition(targetPos);
//    }


//    /*  public void Pitch(Transform startPoint, Transform endPoint, BallType balltype)
//      {
//          if (timeElapsed < pitchDuration)
//          {
//              timeElapsed += Time.deltaTime;
//              float t = timeElapsed / pitchDuration;

//              //궤도 계산
//              float x = pitchCurveX.Evaluate(t);
//              float y = pitchCurveY.Evaluate(t);

//              Vector3 current = Vector3.Lerp(startPoint.position, endPoint.position, t);
//              current += new Vector3(x, y, 0f);

//              transform.position = current;
//          }
//      }*/

//    //변화구에 따른 공 변화
//    private void SetCurves(BallType ballType)
//    {
//        pitchCurveX = new AnimationCurve(
//            new Keyframe(0f, 0f),
//            new Keyframe(1f, 0f)
//            );
//        pitchCurveY = new AnimationCurve(
//            new Keyframe(0f, 0f),
//            new Keyframe(1f, 0f)
//            );

//        switch (ballType)
//        {
//            case BallType.FOURSEAM:
//                break;
//            case BallType.TWOSEAM:
//                pitchCurveX.AddKey(0.65f, -0.35f);
//                break;
//            case BallType.CURVE:
//                pitchCurveY.AddKey(0.65f, 0.65f);
//                break;
//            case BallType.SLIDER:
//                pitchCurveX.AddKey(0.65f, 0.5f);
//                pitchCurveY.AddKey(0.65f, 0.15f);

//                break;
//        }
//    }
//}
