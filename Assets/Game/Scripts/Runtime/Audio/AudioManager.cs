using UnityEngine;

namespace Game.Scripts.Runtime.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        #region Members

        [Header(ArcanysConstants.INSPECTOR.AUDIO_SOURCES)]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _footstepSource;
        
        [Header(ArcanysConstants.INSPECTOR.SETTINGS)]
        [Range(0f, 1f)] public float MasterVolume = 1f;
        [Range(0f, 1f)] public float BgmVolume = 1f;
        [Range(0f, 1f)] public float SfxVolume = 1f;
        [Range(0f, 1f)] public float FootstepVolume = 0.5f;

        #endregion
        
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

        #region Play Audio Helpers

        public void PlayBGM(AudioClip clip, bool loop = false)
        {
            if (clip == null) return;
            _bgmSource.Stop();

            _bgmSource.volume = BgmVolume * MasterVolume;
            _bgmSource.loop = loop;
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (clip == null) return;
            _sfxSource.PlayOneShot(clip, SfxVolume * MasterVolume);
        }

        public void PlayFootsteps(AudioClip clip)
        {
            if (clip == null) return;
            _footstepSource.PlayOneShot(clip, FootstepVolume * MasterVolume);
        }

        #endregion

        #region Helpers

        public void ChangeBgmVolume(float value)
        {
            BgmVolume = value;
            _bgmSource.volume = value * MasterVolume;
        }
        
        public void ChangeSfxVolume(float value)
        {
            SfxVolume = value;
            _sfxSource.volume = value * MasterVolume;
        }
        
        public void ChangeFootstepVolume(float value)
        {
            FootstepVolume = value;
            _footstepSource.volume = value * MasterVolume;
        }

        #endregion
        
    }
}