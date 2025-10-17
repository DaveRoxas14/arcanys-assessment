using System;
using Game.Scripts.Runtime.GameOver;
using UnityEngine;

namespace Game.Scripts.Runtime
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("References")] 
        [SerializeField]
        private GameOverUI _gameOverUI;
        
        [Header("Settings")]
        [SerializeField] private int _scoreToWin = 100;
        [SerializeField] private int _scoreToLose;

        private bool _isGameOver;

        public bool IsGameOver => _isGameOver;

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
        }

        private void OnScoreChanged(int score)
        {
            if (score >= _scoreToWin)
            {
                // win
                // Show Game Over Screen
                _isGameOver = true;
                InputManager.Instance.SwitchToUIControls();
                _gameOverUI.GameOver(true);
            }
            else if (score <= _scoreToLose)
            {
                // lose
                // Show Game Over Screen
                _isGameOver = true;
                InputManager.Instance.SwitchToUIControls();
                _gameOverUI.GameOver(false);
            }
        }
    }
}