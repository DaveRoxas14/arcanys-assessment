using System;
using System.Collections;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.Runtime.GameOver
{
    public class GameOverUI : MonoBehaviour
    {
        #region Members

        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] 
        [SerializeField]
        private GameObject _gameOverScreenObject;

        [SerializeField] private Button _restartBtn;
        [SerializeField] private Button _quitGameBtn;
        [SerializeField] private TextMeshProUGUI _text;

        [Header(ArcanysConstants.INSPECTOR.SETTINGS)] 
        [SerializeField] private string _winString;

        [SerializeField] private string _loseString;
        
        private bool _gameOver;

        #endregion
        
        #region Unity Functions

        public void OnEnable()
        {
            _restartBtn.onClick.AddListener(RestartGame);
            _quitGameBtn.onClick.AddListener(QuitGame);
            _gameOverScreenObject.SetActive(false);
        }

        public void OnDisable()
        {
            _restartBtn.onClick.RemoveListener(RestartGame);
            _quitGameBtn.onClick.RemoveListener(QuitGame);
        }

        #endregion

        public void GameOver(bool win)
        {
            if(_gameOver) return;

            _text.text = win ? _winString : _loseString;
            
            // todo - SFX? VFX?
            
            _gameOver = true;
            _gameOverScreenObject.SetActive(true);
            StartCoroutine(SelectDefaultButton());
            Time.timeScale = 0;
        }

        #region Button Events

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
        
        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_restartBtn.gameObject);
        }

        #endregion
    }
}