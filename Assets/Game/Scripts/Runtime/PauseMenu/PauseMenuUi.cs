using System;
using System.Collections;
using Game.Scripts.Runtime.Scenes;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.Runtime.PauseMenu
{
    public class PauseMenuUi : MonoBehaviour
    {
        #region Members

        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] 
        [SerializeField] private GameObject _pauseMenuObject;
        [SerializeField] private Button _resumeBtn;
        [SerializeField] private Button _settingsBtn;
        [SerializeField] private Button _restartBtn;
        [SerializeField] private Button _quitBtn;
        
        private bool _isPaused;
        public event Action<bool> OnGamePaused; 

        public bool IsPaused => _isPaused;

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            _resumeBtn.onClick.AddListener(ResumeGame);
            _restartBtn.onClick.AddListener(RestartGame);
            _quitBtn.onClick.AddListener(QuitGame);
            _settingsBtn.onClick.AddListener(Settings);
            OnGamePaused += OnPauseEvent;
            
            _pauseMenuObject.SetActive(false);
        }

        private void OnDisable()
        {
            _resumeBtn.onClick.RemoveListener(ResumeGame);
            _restartBtn.onClick.RemoveListener(RestartGame);
            _quitBtn.onClick.RemoveListener(QuitGame);
            _settingsBtn.onClick.RemoveListener(Settings);
            OnGamePaused -= OnPauseEvent;
        }

        #endregion

        #region Pause Menu

        public void ShowPauseMenu()
        {
            if(GameManager.Instance.IsGameOver) return;
            
            OnGamePaused?.Invoke(true);
            _pauseMenuObject.SetActive(true);
            StartCoroutine(SelectDefaultButton());
            Time.timeScale = 0;
        }

        public void HidePauseMenu()
        {
            if(GameManager.Instance.IsGameOver) return;
            _pauseMenuObject.SetActive(false);
            Time.timeScale = 1;
            OnGamePaused?.Invoke(false);
        }

        #endregion

        #region Button Events
        
        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_resumeBtn.gameObject);
        }

        private void QuitGame()
        {
            Time.timeScale = 1;
            
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }

        private async void RestartGame() // todo - make this next level if will have other levels
        {
            _restartBtn.interactable = false;
            GameManager.Instance.IsRestarting = true;
            Time.timeScale = 1;
            
            await GameManager.Instance.FadeOut();
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }

        private void ResumeGame()
        {
            if(GameManager.Instance.IsGameOver) return;
            HidePauseMenu();
        }

        private void Settings()
        {
            if(GameManager.Instance.IsGameOver) return;
            SettingsMenu.Instance.ShowSettingsMenu();
        }

        #endregion

        #region Events

        private void OnPauseEvent(bool isPaused)
        {
            _isPaused = isPaused;
        }

        #endregion
    }
}