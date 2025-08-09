using System;
using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// ���콺/��ġ �Է��� �޾� ��ü Ÿ�� ���������� ����
/// </summary>
public class HitInputHandler : MonoBehaviour
{
    public event Action<AccuracyResult, bool> OnHit;
    public event Action OnMiss;


    [Tooltip("Ÿ�̹� ���� �ִ� ���� ������ (��)")]
    [SerializeField]
    private float maxWindow = 3f;
    private bool _hitProcessed;
    public bool hitProcessed => _hitProcessed;
    public bool lastHitSuccessful { get; private set; }


    [Tooltip("Ÿ���� �߽� Transform")]
    public Transform batZoneTransform;

    [SerializeField]
    private LayerMask hitLayerMask;

    [SerializeField]
    private Animator BattingAni;

    //Ÿ�� Ƚ�� ����
    public void ResetHitFlag()
    {
        _hitProcessed = false;
        lastHitSuccessful = false;
    }

    void Update()
    {
        if (_hitProcessed) return;  // �̹� ó���� ���� ����

        float now = Time.time;
        float perfect = PitchingManager.Instance.perfectHitTime;

        //�꽺��
        if (PitchingManager.Instance.CurrentBall != null && now > perfect + maxWindow)
        {
            Debug.Log("�ڵ� �꽺��");
            lastHitSuccessful = false;
            _hitProcessed = true;
            OnMiss?.Invoke();
            return;
        }

        bool clicked = Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);

        if (!clicked)
            return;

        BattingAni.SetTrigger("Batting");

        Vector3 screenPos = (Input.touchCount > 0) ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, hitLayerMask))
        {
            Debug.LogWarning("HitInputHandler : �������� �ƴմϴ�.");
            return;
        }

        Vector3 clickWorld = hit.point;

        //�Է½���
        float clickTime = Time.time;

        // 2) Timing & Position ����
        TimingJudge tJudge = new TimingJudge(PitchingManager.Instance.perfectHitTime);
        TimingResult tRes = tJudge.Evaluate(clickTime);
        //Debug.Log("���� ���" + tRes.Accuracy + "  " + tRes.Offset);


        float pAcc = new PositionJudge(PitchingManager.Instance.targetPoint.position).Evaluate(clickWorld);
        //Debug.Log($"Ŭ�� ��ġ x : {clickWorld.x} y : {clickWorld.y} z : {clickWorld.z} <{pAcc}>");

        //Ÿ�� ���� 0.2�ʰų� Ÿ�� ������ ���� ������꽺��
        if (tRes.Accuracy < 0 || pAcc == 0)
        {
            Debug.Log("�꽺��!");
            OnMiss?.Invoke();
            lastHitSuccessful = false;
        }
        else
        {
            bool isHomeRun = false;
            OnHit?.Invoke(new AccuracyResult(tRes.Accuracy, pAcc), isHomeRun);
            lastHitSuccessful = true;

            // 3) �ӵ�, ����/���� ���� ���
            var (speed, vertAngle, horzAngle) = new HitPhysicsCalculator().Calculate(tRes, pAcc, tJudge.MaxWindow);

            // 4) �� �߻�
            GameObject ball = PitchingManager.Instance.CurrentBall;
            BallController.Instance.ApplyHit(ball, speed, vertAngle, horzAngle, PitchingManager.Instance.spawnPoint.position - PitchingManager.Instance.targetPoint.position, PitchingManager.Instance.targetPoint.position);
        }
        _hitProcessed = true;


    }
}

