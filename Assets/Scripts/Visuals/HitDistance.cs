using UnityEngine;

public class HitDistance : MonoBehaviour
{
    public Transform targetPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            float dis = Vector3.Distance(targetPoint.position, other.transform.position);
            Debug.Log("Ÿ�� �Ÿ� : " + dis);
        }
    }
}
