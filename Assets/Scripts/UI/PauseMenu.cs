using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    //��ü �Ͻ����� â
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField]
    //�Ͻ����� ��ư
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
        //�г� �ʱ����
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        
    }

    public void TogglePause() => SetPause(!_paused);

    public void SetPause(bool pause)
    {
        _paused = pause;
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);

        //���� ����
       Time.timeScale = pause ? 0f : 1f;

        //pc ������ Ŀ�� ǥ��
        Cursor.visible = pause;
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void Retry()
    {
        //����� �ٽ� �ε�
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
