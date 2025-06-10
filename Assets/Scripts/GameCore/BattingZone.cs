using System.Collections;
using UnityEngine;

public class BattingZone : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BattingManager.Instance.perfectTime = Time.time;
            Debug.Log("��Ʈ����ũ!!");
            StartCoroutine(Judge());
        }
    }

    private IEnumerator Judge()
    {
        yield return new WaitForSeconds(0.25f);
        BattingManager.Instance.TryJudge();
        BattingManager.Instance.canJudge = false;
    }
}
