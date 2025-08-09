using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Refrence")]
    public AudioLibrary library;
    public AudioMixer mixer;
    //BGM 그룹
    public AudioMixerGroup bgmGroup;
    //SFX 그룹
    public AudioMixerGroup sfxGroup;

    [Header("SFX Pool")]
    [SerializeField]
    private int sfxPoolSize = 16;

    //Mixer 파라미터명
    const string MASTER_PARM = "MasterVol";
    const string BGM_PARM = "BGMVol";
    const string SFX_PARM = "SFXVool";

    //BGM용 더블버퍼
    private AudioSource _bgmA;
    private AudioSource _bgmB;
    private bool _useA = true;
    private Coroutine _bgmCo;

    //SFX 풀
    private List<AudioSource> _sfxPool;
    private int _sfxIdx;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        //BGM소스 2개 크로스 페이드
        _bgmA = CreateSource("BGM_A", bgmGroup, loop: true, spatialBlend: 0f);
        _bgmB = CreateSource("BGM_B", bgmGroup, loop: true, spatialBlend: 0f);


        //SFX 풀
        _sfxPool = new List<AudioSource>(sfxPoolSize);
        for (int i = 0; i < sfxPoolSize; i++)
            _sfxPool.Add(CreateSource($"SFX_{i}", sfxGroup, loop: false, spatialBlend: 0f));
    }

    private AudioSource CreateSource(string name, AudioMixerGroup group, bool loop, float spatialBlend)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform);
        AudioSource src = go.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.loop = loop;
        src.outputAudioMixerGroup = group;
        src.spatialBlend = spatialBlend;
        return src;
    }
    //-----------------BGM-----------------

    public void PlayBGM(string key, float fade = 0.5f)
    {
        AudioClip clip = library.Get(key);
        if (!clip)
            return;
        if (_bgmCo != null)
            StopCoroutine(_bgmCo);
        _bgmCo = StartCoroutine(CoCrossfade(clip, fade));
    }

    public void StopBGM(float fade = 0.5f)
    {
        if (_bgmCo != null)
            StopCoroutine(_bgmCo);
        _bgmCo = StartCoroutine(CoFadeOut(fade));
    }

    private IEnumerator CoCrossfade(AudioClip next, float fade)
    {
        AudioSource from = _useA ? _bgmA : _bgmB;
        AudioSource to = _useA ? _bgmB : _bgmA;
        _useA = !_useA;

        to.clip = next;
        to.volume = 0f;
        to.Play();

        float t = 0f;
        float fromStart = from.isPlaying ? from.volume : 0f;
        while (t < fade)
        {
            t += Time.unscaledDeltaTime;
            float k = t / fade;
            to.volume = Mathf.Lerp(0f, 1f, k);
            from.volume = Mathf.Lerp(fromStart, 0f, k);
            yield return null;
        }
        to.volume = 1f;
        if (from.isPlaying)
            from.Stop();
    }

    private IEnumerator CoFadeOut(float fade)
    {
        AudioSource cur = _useA ? _bgmA : _bgmB;
        float start = cur.volume;
        float t = 0f;
        while (t < fade)
        {
            t += Time.unscaledDeltaTime;
            cur.volume = Mathf.Lerp(start, 1f, t / fade);
            yield return null;
        }
        cur.Stop();
        cur.volume = 1f;
    }
    // ----------------SFX-------------
    public void PlaySFX(string key, float pitchJitter = 0.0f)
    {
        AudioClip clip = library.Get(key);
        if (!clip)
            return;
        AudioSource src = NextSfxSource();
        src.transform.position = Vector3.zero;
        src.spatialBlend = 0f;
        src.pitch = 1f + Random.Range(-pitchJitter, pitchJitter);
        src.PlayOneShot(clip);
    }

    public void PlaySFX3D(string key, Vector3 pos, float pitchJitter = 0.0f)
    {
        AudioClip clip = library.Get(key);
        if(!clip) 
            return;
        AudioSource src = NextSfxSource();
        src.transform.position = pos;
        src.spatialBlend = 1f;
        src.pitch = 1f + Random.Range(-pitchJitter, pitchJitter);
        src.PlayOneShot(clip);
    }

    private AudioSource NextSfxSource()
    {
        _sfxIdx = (_sfxIdx + 1) % _sfxPool.Count;
        return _sfxPool[_sfxIdx];
    }
    //---------------Volume---------------
    public void SetMaster(float linear) => SetMixerLinear(MASTER_PARM, linear);
    public void SetBGM(float linear) => SetMixerLinear(BGM_PARM, linear);
    public void SetSFX(float linear) => SetMixerLinear(SFX_PARM, linear);

    private void SetMixerLinear(string param, float linear)
    {
        float db = (linear <= 0.0001f) ? -80f : Mathf.Log10(Mathf.Clamp01(linear));
        mixer.SetFloat(param, db);
    }
    

    /*// 사용법 예시
    // 로비 진입
    SoundManager.Instance.PlayBGM("BGM_Lobby", 0.8f);
                  
    // 라운드 시작
    SoundManager.Instance.PlayBGM("BGM_Round", 0.5f);
                  
    // 버튼 클릭
    SoundManager.Instance.PlaySFX("SFX_UI_Click", 0.03f);
                  
    // 타격     
    SoundManager.Instance.PlaySFX("SFX_Hit", 0.05f);
    SoundManager.Instance.DuckBGM(0.5f, 0.03f, 0.05f, 0.2f);
                 
    // 홈런       
    SoundManager.Instance.PlaySFX("SFX_Homerun");
    */
}
