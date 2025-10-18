using UnityEngine;

namespace Game.Scripts.Runtime.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [Header(ArcanysConstants.INSPECTOR.AUDIO_SOURCES)]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;
        
        [Header(ArcanysConstants.INSPECTOR.SETTINGS)]
        [Range(0f, 1f)] public float _masterVolume = 1f;
        [Range(0f, 1f)] public float _bgmVolume = 1f;
        [Range(0f, 1f)] public float _sfxVolume = 1f;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PlayBGM(AudioClip clip, bool loop = false)
        {
            if (clip == null) return;
            _bgmSource.Stop();

            _bgmSource.volume = _bgmVolume;
            _bgmSource.loop = loop;
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip, _sfxVolume * _masterVolume);
        }
        
    }
}