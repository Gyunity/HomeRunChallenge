using UnityEngine;

public class BallRotation : MonoBehaviour
{

    void Update()
    {
        transform.Rotate(new Vector3(-1000f, 300f, 0)*Time.deltaTime);
    }
}
