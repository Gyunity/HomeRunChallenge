using UnityEngine;
/// <summary>
/// ������ ���������� �ϳ��� ��Ŀ�� ���� �̵����� ǥ��
/// </summary>
public class LandingVisualizer : MonoBehaviour
{
    public static LandingVisualizer Instance {  get; private set; }
    [Tooltip("���� ���� ǥ�ÿ� ������")]
    public GameObject landingMarkerPrefab;
    [Tooltip("��Ŀ�� ��¦ ���� ���� ������")]
    public float heightoffset = 0.1f;

    private GameObject marker;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ShowLandingSpot(float dist, Vector3 origin, Vector3 forward)
    {
        Vector3 pos = origin + forward.normalized * dist;
        pos.y += heightoffset;

        if (marker != null)
            marker = Instantiate(landingMarkerPrefab, pos, Quaternion.identity);
        else
        {
            marker.transform.position = pos;
            marker.SetActive(true);
        }
    }
    public void HideLandingSpot()
    {
        if(marker != null)
            marker.SetActive(false);
    }
}
