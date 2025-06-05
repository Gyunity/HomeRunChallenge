using UnityEngine;

public class PitchBall : MonoBehaviour
{
    public enum BallType
    {
        FOURSEAM,
        TWOSEAM,
        CURVE,
        SLIDER
    }

    public BallType balltype;
    private AnimationCurve pitchCurveX;
    private AnimationCurve pitchCurveY;
    public float pitchDuration = 1.0f;
    private float timeElapsed;

    public Transform startPoint;
    public Transform endPoint;
    void Start()
    {
        transform.position = startPoint.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = startPoint.position;
            timeElapsed = 0;
        }

        //SamplePitch();
        Pitch();
    }

    private void SamplePitch()
    {
        if (timeElapsed < pitchDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / pitchDuration;

            //±Ëµµ °è»ê
            float x = pitchCurveX.Evaluate(t);
            float y = pitchCurveY.Evaluate(t);

            Vector3 current = Vector3.Lerp(startPoint.position, endPoint.position, t);
            current += new Vector3(x, y, 0f);

            transform.position = current;
        }
    }

    private void Pitch()
    {
        if (timeElapsed < pitchDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / pitchDuration;

            //±Ëµµ °è»ê
            AnimationCurveSet(balltype);
            float x = pitchCurveX.Evaluate(t);
            float y = pitchCurveY.Evaluate(t);

            Vector3 current = Vector3.Lerp(startPoint.position, endPoint.position, t);
            current += new Vector3(x, y, 0f);

            transform.position = current;
        }
    }

    private void AnimationCurveSet(BallType ballType)
    {
        pitchCurveX = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(1f, 0f)
            );
        pitchCurveY = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(1f, 0f)
            );

        switch (ballType)
        {
            case BallType.FOURSEAM:
                break;
            case BallType.TWOSEAM:
                pitchCurveX.AddKey(0.65f, -0.35f);
                break;
            case BallType.CURVE:
                pitchCurveY.AddKey(0.65f, 0.65f);
                break;
            case BallType.SLIDER:
                pitchCurveX.AddKey(0.65f, 0.5f);
                pitchCurveY.AddKey(0.65f, 0.15f);

                break;
        }

    }
}
