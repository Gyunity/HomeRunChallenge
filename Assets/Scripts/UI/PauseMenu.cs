using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    //전체 일시정지 창
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField]
    //일시정지 버튼
    private Button btnPause;
    [SerializeField]
    private Button btnResume;
    [SerializeField]
    private Button btnRetry;
    [SerializeField]
    private Button butBack;
    [SerializeField]
    private Button butSettings;
    [SerializeField]
    private Button butSettingClose;

    [Header("Volume Sliders")]
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;

    [Header("Scenes")]
    [SerializeField]
    private string backSceneName = "LobbyScene";

    private bool _paused;

    private void Awake()
    {
        //패널 초기상태
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        
    }

    public void TogglePause() => SetPause(!_paused);

    public void SetPause(bool pause)
    {
        _paused = pause;
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);

        //게임 정지
       Time.timeScale = pause ? 0f : 1f;

        //pc 디버깅용 커서 표시
        Cursor.visible = pause;
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void Retry()
    {
        //현재신 다시 로드
        SetPause(false);
        string name = SceneManager.GetActiveScene().name;
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(name);
        else
            SceneManager.LoadScene(name);
    }

    private void BackToLobby()
    {
        SetPause(false);
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.LoadScene(backSceneName);
        else
            SceneManager.LoadScene(backSceneName);

    }


}
