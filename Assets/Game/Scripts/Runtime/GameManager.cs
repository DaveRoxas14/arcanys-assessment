using System;
using Game.Scripts.Runtime.GameOver;
using Game.Scripts.Runtime.GameTimer;
using UnityEngine;

namespace Game.Scripts.Runtime
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        #region Members

        [Header("References")] 
        [SerializeField]
        private GameOverUI _gameOverUI;

        [SerializeField] private GameTime _time;
        
        [Header("Settings")]
        [SerializeField] private int _scoreToWin = 100;
        [SerializeField] private int _scoreToLose;

        [SerializeField] private float _gameTime;

        private bool _isGameOver;

        public bool IsGameOver => _isGameOver;

        public GameTime GameTime => _time;

        public int ScoreToWin => _scoreToWin;

        public int ScoreToLose => _scoreToLose;

        #endregion

        #region Unity Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void Start()
        {
            ScoringManager.Instance.OnScoreChanged += OnScoreChanged;
            _time.OnTimerEnd += OnTimerFinished;
            GameTime.StartTimer(_gameTime);
        }

        #endregion

        private void OnScoreChanged(int score)
        {
            if (score >= ScoreToWin)
            {
                WinGame();
            }
            else if (score <= ScoreToLose)
            {
                LoseGame();
            }
        }

        private void OnTimerFinished()
        {
            LoseGame();
        }

        private void WinGame()
        {
            // win
            // Show Game Over Screen
            _isGameOver = true;
            InputManager.Instance.SwitchToUIControls();
            _gameOverUI.GameOver(true);
        }

        private void LoseGame()
        {
            // lose
            // Show Game Over Screen
            _isGameOver = true;
            InputManager.Instance.SwitchToUIControls();
            _gameOverUI.GameOver(false);
        }
    }
}