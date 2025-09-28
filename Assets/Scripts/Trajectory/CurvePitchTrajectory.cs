using System.Timers;
using UnityEngine;

public class CurvePitchTrajectory : MonoBehaviour, IPitchTrajectory
{
    public enum BallType
    {
        FOURSEAM,
        TWOSEAM,
        CURVE,
        SLIDER
    }

    [Header("Trajectore Serrings")]
    public BallType ballType = BallType.FOURSEAM;
    private AnimationCurve curveX, curveY;

    private Transform startPoint;
    private Transform endPoint;
    private float duration;
    private float elapsed;
    private bool launched;
    private bool stopped;
    private Rigidbody rb;

    


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

    }
    public void Launch(Vector3 start, Vector3 end, float duration)
    {
        startPoint = new GameObject("start").transform;
        endPoint = new GameObject("end").transform;
        startPoint.position = start;
        endPoint.position = end;

        this.duration = duration;
        elapsed = 0;
        launched = true;
        stopped = false;
        enabled = true;
        // curve ÃÊ±âÈ­
        curveX = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));
        curveY = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 0f));
        switch (ballType)
        {
            case BallType.TWOSEAM:
                curveX.AddKey(0.65f, -0.35f);
                break;
            case BallType.CURVE:
                curveY.AddKey(0.65f, 0.65f);
                break;
            case BallType.SLIDER:
                curveX.AddKey(0.65f, 0.5f);
                curveY.AddKey(0.65f, 0.15f);
                break;
            case BallType.FOURSEAM:
            default: break;
        }

    }

    private void FixedUpdate()
    {
        if (IsFinished) return;

        elapsed += Time.fixedDeltaTime;

        rb.MovePosition(GetPosition(elapsed));
        if (elapsed > duration)
        {
            Stop();
            return;
        }
        
    }
    public Vector3 GetPosition(float elapsedTime)
    {
       float t = Mathf.Clamp01(elapsedTime / duration);
        Vector3 basePos = Vector3.Lerp(startPoint.position, endPoint.position, t);
        return basePos + new Vector3(curveX.Evaluate(t), curveY.Evaluate(t), 0f);
    }

    public void Stop()
    {
        stopped = true;
        launched = false;
        enabled = false;
    }


    public bool IsFinished => stopped ||  !launched;

}
