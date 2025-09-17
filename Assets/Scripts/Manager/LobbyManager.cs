using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Button playBut;

    [SerializeField]
    private TMP_Text bestScoreText;

    [Header("HowToPlay")]
    [SerializeField]
    private GameObject howToPlayPanel;
    [SerializeField]
    private Button howToPlayBut; 
    [SerializeField]
    private Button okHowToPlayBut;


    [Header("Volume Sliders")]
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider bgmSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private GameObject setPanel;
    [SerializeField]
    private Button settingBut; 
    [SerializeField]
    private Button okSettingBut;

    void Start()
    {
        // 슬라이더 초기화 & 리스너
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
            bgmSlider.value = PlayerPrefs.GetFloat("vol_bgm", 1f);
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

        // 저장된 볼륨을 즉시 적용
        SoundManager.Instance.SetMaster(masterSlider ? masterSlider.value : 1f);
        SoundManager.Instance.SetBGM(bgmSlider ? bgmSlider.value : 0.8f);
        SoundManager.Instance.SetSFX(sfxSlider ? sfxSlider.value : 1f);

        SoundManager.Instance.PlayBGM("BGM_Lobby", 0.5f);
        playBut.onClick.AddListener(playClick);
        bestScoreText.text = $"Best Score : {HighScoreManager.GetHighScore("Default").ToString("N0")}";
        howToPlayPanel.SetActive(false);
        howToPlayBut.onClick.AddListener(()=>howToPlayPanel.SetActive(true));
        okHowToPlayBut.onClick.AddListener(()=>howToPlayPanel.SetActive(false));

        setPanel.SetActive(false);
        settingBut.onClick.AddListener(() => setPanel.SetActive(true));
        okSettingBut.onClick.AddListener(() => setPanel.SetActive(false));
    }
    private void playClick()
    {
        SceneLoader.Instance.LoadScene("PlayScene");
        SoundManager.Instance.StopBGM();

    }
}
