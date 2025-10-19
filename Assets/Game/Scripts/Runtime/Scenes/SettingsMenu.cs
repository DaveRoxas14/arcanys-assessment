using System;
using System.Collections;
using Game.Scripts.Runtime.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.Runtime.Scenes
{
    public class SettingsMenu : MonoBehaviour
    {
        public static SettingsMenu Instance { get; private set; }

        #region Members

        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] 
        [SerializeField] private Button _menuDefaultButton;
        [SerializeField] private Button _backBtn;

        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _menuPanel;
        
        [FormerlySerializedAs("_defaultButton")] 
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;
        [SerializeField] private Slider _footstepSlider;

        [SerializeField] private SoundEffect _sfxSample;
        [SerializeField] private SoundEffect _footstepSample;

        private AudioManager _am;

        private AudioManager AudioManager
        {
            get
            {
                if(!_am)
                    _am = AudioManager.Instance;
                return _am;
            }
        }
        
        #endregion

        #region Unity Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        private void Start()
        {
            _backBtn.onClick.AddListener(HideSettingsMenu);
        }

        private void OnEnable()
        {
            _bgmSlider.onValueChanged.AddListener(OnBgmSliderValueChanged);
            _sfxSlider.onValueChanged.AddListener(OnSfxSliderValueChanged);
            _footstepSlider.onValueChanged.AddListener(OnFootstepSliderValueChanged);
        }

        private void OnDisable()
        {
            _bgmSlider.onValueChanged.RemoveListener(OnBgmSliderValueChanged);
            _sfxSlider.onValueChanged.RemoveListener(OnSfxSliderValueChanged);
            _footstepSlider.onValueChanged.RemoveListener(OnFootstepSliderValueChanged);
        }

        #endregion

        #region Helpers

        public void ShowSettingsMenu()
        {
            _settingsPanel.SetActive(true);
            _menuPanel.SetActive(false);
            if(_bgmSlider)
                StartCoroutine(SelectDefaultButton(_bgmSlider));

            LoadAudioSettings();
        }
        
        public void HideSettingsMenu()
        {
            _settingsPanel.SetActive(false);
            _menuPanel.SetActive(true);
            
            if(_menuDefaultButton)
                StartCoroutine(SelectDefaultButton(_menuDefaultButton));
        }
        
        private IEnumerator SelectDefaultButton(Selectable button)
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }

        #endregion

        #region Settings Load

        private void LoadAudioSettings()
        {
            _bgmSlider.value = AudioManager.BgmVolume;
            _sfxSlider.value = AudioManager.SfxVolume;
            _footstepSlider.value = AudioManager.FootstepVolume;
        }

        #endregion

        #region On Settings Changed Events

        private void OnSfxSliderValueChanged(float value)
        {
            AudioManager.ChangeSfxVolume(value);
            
            if(_sfxSample)
                AudioManager.PlaySFX(_sfxSample.clip);
        }
        
        private void OnBgmSliderValueChanged(float value)
        {
            AudioManager.ChangeBgmVolume(value);
        }
        
        private void OnFootstepSliderValueChanged(float value)
        {
            AudioManager.ChangeFootstepVolume(value);
            
            if(_footstepSample)
                AudioManager.PlayFootsteps(_footstepSample.clip);
        }

        #endregion
    }
}