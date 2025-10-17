using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.Runtime.PauseMenu
{
    public class PauseMenuUi : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private GameObject _pauseMenuObject;
        [SerializeField] private Button _resumeBtn;
        [SerializeField] private Button _restartBtn;
        [SerializeField] private Button _quitBtn;
        
        private bool _isPaused;
        private bool isGameOver;

        public event Action<bool> OnGamePaused; 

        public bool IsPaused => _isPaused;

        #region Unity Functions

        private void OnEnable()
        {
            _resumeBtn.onClick.AddListener(ResumeGame);
            _restartBtn.onClick.AddListener(RestartGame);
            _quitBtn.onClick.AddListener(QuitGame);
            OnGamePaused += OnPauseEvent;
            
            _pauseMenuObject.SetActive(false);
        }

        private void OnDisable()
        {
            _resumeBtn.onClick.RemoveListener(ResumeGame);
            _restartBtn.onClick.RemoveListener(RestartGame);
            _quitBtn.onClick.RemoveListener(QuitGame);
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

        private void RestartGame() // todo - make this next level if will have other levels
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }

        private void ResumeGame()
        {
            if(GameManager.Instance.IsGameOver) return;
            HidePauseMenu();
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