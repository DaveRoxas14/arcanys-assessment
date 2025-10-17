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

        [Header("References")] 
        [SerializeField]
        private GameObject _gameOverScreenObject;

        [SerializeField] private Button _restartBtn;
        [SerializeField] private Button _quitGameBtn;
        [SerializeField] private TextMeshProUGUI _text;

        [Header("Settings")] 
        [SerializeField] private string _winString;

        [SerializeField] private string _loseString;

        #endregion

        private bool _gameOver;

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

        private void RestartGame() // todo - make this next level if will have other levels
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        }
        
        private IEnumerator SelectDefaultButton()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(_restartBtn.gameObject);
        }

        #endregion
    }
}