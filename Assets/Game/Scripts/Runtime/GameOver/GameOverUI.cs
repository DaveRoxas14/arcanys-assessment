using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.Runtime.GameOver
{
    public class GameOverUI : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField]
        private GameObject _gameOverScreenObject;

        [SerializeField] private Button _restartBtn;
        [SerializeField] private Button _quitGameBtn;

        private bool _gameOver;

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

        public void GameOver(bool win)
        {
            if(_gameOver) return;
            
            _gameOver = true;
            _gameOverScreenObject.SetActive(true);
            Time.timeScale = 0;
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
    }
}