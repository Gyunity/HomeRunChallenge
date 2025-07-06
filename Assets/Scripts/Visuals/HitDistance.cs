using UnityEngine;

public class HitDistance : MonoBehaviour
{
    public Transform targetPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            float dis = Vector3.Distance(targetPoint.position, other.transform.position);
            Debug.Log("타격 거리 : " + dis);
        }
    }
}
