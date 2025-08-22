using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    private Button playBut;
    void Start()
    {
        SoundManager.Instance.PlayBGM("BGM_Lobby", 0.5f);
        playBut.onClick.AddListener(playClick);
    }
    private void playClick()
    {
        SceneLoader.Instance.LoadScene("PlayScene");
        SoundManager.Instance.StopBGM();

    }
}
