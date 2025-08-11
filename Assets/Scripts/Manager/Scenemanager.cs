using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("Transition")]
    [SerializeField]
    private CanvasGroup fadeCanvas;
    [SerializeField]
    private float fadeDuration = 0.5f;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (fadeCanvas != null) 
        {
            fadeCanvas.alpha = 1f;
        }

        StartCoroutine(Fade(0f));
    }

    //�� ��ȯ
    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        //���̵� �ƿ�
        yield return Fade(1f);
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while(!op.isDone)
            yield return null;

        yield return Fade(0f);
    }

    private IEnumerator Fade(float target)
    {
        if(fadeCanvas == null)
            yield break;
        float start = fadeCanvas.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(start, target, t/fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = target;
    }

    //��� ����
    //SceneLoader.Instance.LoadScene("PlayScene");
}
