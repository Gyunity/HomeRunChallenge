using UnityEngine;

public class BattingZone : MonoBehaviour
{
    private float perfectTime;
    private bool canJudge = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            perfectTime = Time.time;
            canJudge = true;
        }
    }

}
