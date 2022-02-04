using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    private const int AUDIO_SOURCE_COUNT = 24;
    public bool IsMusicOn => DataManager.OptionData.BGM;
    public bool IsSfxOn => DataManager.OptionData.SFX; 
    

    [SerializeField] [Header("배경음 소스")]
    private AudioSource m_BackgroundMusic;
    private List<AudioSource> m_SFXSourcePool = new List<AudioSource>(AUDIO_SOURCE_COUNT);
    
    [Header("배경음 클립 리스트")] public List<AudioClip> m_BackgroundClipList;
    private Dictionary<string, AudioClip> m_BackgoundClips = new Dictionary<string, AudioClip>();
    
    [Header("효과음 클립 리스트")] public List<AudioClip> m_SFXAudioClipList;
    private Dictionary<string, AudioClip> m_SFXClips = new Dictionary<string, AudioClip>();

    
    protected override void Awake()
    {
        base.Awake();
        
        Init();
    }
    
    private void Init()
    {
        if (!m_BackgroundMusic)
        {
            var newObject = new GameObject("BackgroundMusic");
            newObject.transform.parent = transform;
            m_BackgroundMusic = newObject.AddComponent<AudioSource>();
            m_BackgroundMusic.loop = true;
            m_BackgroundMusic.playOnAwake = false;
        }
        
        // 오디오 소스 초기화
        for (int i = 0; i < AUDIO_SOURCE_COUNT; ++i)
        {
            GameObject audio = new GameObject($"AudioSource_{i + 1}");
            audio.transform.parent = transform;

            m_SFXSourcePool.Add(audio.AddComponent<AudioSource>());
        }

        // 배경음악
        foreach (var clip in m_BackgroundClipList)
        {
            if (!m_BackgoundClips.ContainsKey(clip.name))
            {
                m_BackgoundClips.Add(clip.name, clip);
            }
            else
            {
                Debug.LogError($"[배경음] {clip.name} 중복!");
            }
        }
        
        // 효과음
        foreach (var clip in m_SFXAudioClipList)
        {
            if (clip == null || string.IsNullOrEmpty(clip.name))
            {
                continue;
            }
            
            if (!m_SFXClips.ContainsKey(clip.name))
            {
                m_SFXClips.Add(clip.name, clip);
            }
            else
            {
                Debug.LogError($"[효과음] {clip.name} 중복!");
            }
        }
    }
    
    public void PlayBackground(string soundName, float volumeFactor = 1f, bool loop = true , bool force = false)
    {
        AudioClip audioClip = null;
        
        if (!m_BackgoundClips.TryGetValue(soundName, out  audioClip))
        //if (!m_BackgoundClips.TryGetValue(soundName, out  audioClip))
        {
            Debug.LogError($"[배경음] {soundName}이 없습니다.");
            return;
        }

        if (m_BackgroundMusic.clip == audioClip && !force)
        {
            return;
        }
        
        if(m_BackgroundMusic.clip != audioClip && m_BackgroundMusic)
        {
            m_BackgroundMusic.Stop();
        }

        m_BackgroundMusic.clip = audioClip;
        m_BackgroundMusic.volume = IsMusicOn ? volumeFactor : 0;
        m_BackgroundMusic.loop = loop;

        m_BackgroundMusic.Play();
    }

    public void PlaySoundDelay(float delayTime, string soundName, float volumeFactor = 1f)
    {
        if (!IsSfxOn) return;
        
        StartCoroutine(_PlaySoundDelay(delayTime, soundName, volumeFactor));
    }

    private IEnumerator _PlaySoundDelay(float delayTime, string soundName, float volumeFactor = 1f)
    {
        yield return new WaitForSeconds(delayTime);
        
        PlaySound(soundName, volumeFactor);
    }
    
    public void PlaySound(string soundName, float volumeFactor = 1f)
    {
        if(!IsSfxOn)
        {
            return;
        }
        
        AudioClip audioClip = null;
        
        if (!m_SFXClips.TryGetValue(soundName, out audioClip))
       // if (!m_SFXClips.TryGetValue(soundName, out audioClip))
        {
            Debug.LogError($"[효과음] {soundName}이 없습니다.");
            return;
        }
        
        AudioSource audioSource = null;

        foreach (var source in m_SFXSourcePool)
        {
            if (source.isPlaying)
            {
                continue;
            }

            audioSource = source;
            break;
        }
        
        if (audioSource == null)
        {
            return;
        }

        audioSource.clip = audioClip;
        audioSource.volume = volumeFactor;
        audioSource.loop = false;

        audioSource.PlayOneShot(audioClip);
    }
    
    public void ToggleMusic()
    {
        m_BackgroundMusic.volume = IsMusicOn ? 1 : 0;
    }
    
    
    public void PlayPopupOpen()
    {
        PlaySound("ui_pop_in");
    }

    public void PlayPopupClose()
    {
        PlaySound("ui_pop_out");
    }
}