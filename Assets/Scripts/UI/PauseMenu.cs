using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    //��ü �Ͻ����� â
    [SerializeField]
    private GameObject pausePanel;

    [Header("Buttons")]
    [SerializeField]
    //�Ͻ����� ��ư
    private Button btnPause;
    [SerializeField]
    private Button btnResume;
    [SerializeField]
    private Button btnRetry;
    [SerializeField]
    private Button btnRetry2;
    [SerializeField]
    private Button btnBack;
    [SerializeField]
    private Button btnBack2;


    [Header("Text")]
    [SerializeField]
    private TMP_Text roundText;
    [SerializeField]
    private TMP_Text scoreText;


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

    [Header("Manager")]
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private GameFlowManager gameFlowManager;

    private bool _paused;

    private void Awake()
    {
        //�г� �ʱ����
        pausePanel.SetActive(false);

        //��ư����
        btnPause.onClick.AddListener(TogglePause);
        btnResume.onClick.AddListener(() => SetPause(false));
        btnRetry.onClick.AddListener(Retry);
        btnRetry2.onClick.AddListener(Retry);
        btnBack.onClick.AddListener(BackToLobby);
        btnBack2.onClick.AddListener(BackToLobby);

        // �����̴� �ʱ�ȭ & ������
        if (masterSlider)
        {
            masterSlider.value = PlayerPrefs.GetFloat("vol_master", 1f);
            masterSlider.onValueChanged.AddListener(v => {
                SoundManager.Instance.SetMaster(v);
                PlayerPrefs.SetFloat("vol_master", v);
            });
        }
        if (bgmSlider)
        {
            bgmSlider.value = PlayerPrefs.GetFloat("vol_bgm", 0.8f);
            bgmSlider.onValueChanged.AddListener(v => {
                SoundManager.Instance.SetBGM(v);
                PlayerPrefs.SetFloat("vol_bgm", v);
            });
        }
        if (sfxSlider)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("vol_sfx", 1f);
            sfxSlider.onValueChanged.AddListener(v => {
                SoundManager.Instance.SetSFX(v);
                PlayerPrefs.SetFloat("vol_sfx", v);
            });
        }

        // ����� ������ ��� ����
        SoundManager.Instance.SetMaster(masterSlider ? masterSlider.value : 1f);
        SoundManager.Instance.SetBGM(bgmSlider ? bgmSlider.value : 0.8f);
        SoundManager.Instance.SetSFX(sfxSlider ? sfxSlider.value : 1f);
    }

    public void TogglePause() => SetPause(!_paused);

    public void SetPause(bool pause)
    {
        _paused = pause;
        pausePanel.SetActive(pause);

        Debug.Log($"{pause},{scoreManager.totalScore}, {gameFlowManager.GetCurrentRound()}");
        scoreText.text = $"SCORE : {scoreManager.totalScore}";
        roundText.text = $"ROUND : {gameFlowManager.GetCurrentRound()}";

        //���� ����
       Time.timeScale = pause ? 0f : 1f;

        //pc ������ Ŀ�� ǥ��
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
