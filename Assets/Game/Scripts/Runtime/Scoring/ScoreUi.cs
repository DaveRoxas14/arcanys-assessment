using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Runtime.Scoring
{
    public class ScoreUi : MonoBehaviour, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private int _gameToWin;
        
        private void Start()
        {
            ScoringManager.Instance.OnScoreChanged += OnScoreChanged;
            _gameToWin = GameManager.Instance.ScoreToWin;
            
            _scoreText.text = $"0/{_gameToWin}";
        }
        private void OnScoreChanged(int score)
        {
            _scoreText.text = $"{score}/{_gameToWin}";
        }

        public void Dispose()
        {
            ScoringManager.Instance.OnScoreChanged -= OnScoreChanged;
        }
    }
}