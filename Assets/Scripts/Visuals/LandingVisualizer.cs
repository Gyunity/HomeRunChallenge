using UnityEngine;
/// <summary>
/// 예측된 낙하지점에 하나의 마커만 생성 이동시켜 표시
/// </summary>
public class LandingVisualizer : MonoBehaviour
{
    public static LandingVisualizer Instance {  get; private set; }
    [Tooltip("예측 지점 표시용 프리팹")]
    public GameObject landingMarkerPrefab;
    [Tooltip("마커를 살짝 띄우는 높이 오프셋")]
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
