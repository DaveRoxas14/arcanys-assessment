using System;
using Game.Scripts.Runtime.GameOver;
using UnityEngine;

namespace Game.Scripts.Runtime
{
    public class GameManager : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField]
        private GameOverUI _gameOverUI;
        
        [Header("Settings")]
        [SerializeField] private int _scoreToWin = 100;
        [SerializeField] private int _scoreToLose;
        
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
                _gameOverUI.GameOver(true);
            }
            else if (score <= _scoreToLose)
            {
                // lose
                // Show Game Over Screen
                _gameOverUI.GameOver(false);
            }
        }
    }
}