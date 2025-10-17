using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Runtime.Scoring
{
    public class ScoreUi : MonoBehaviour, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private void Start()
        {
            ScoringManager.Instance.OnScoreChanged += OnScoreChanged;
        }
        private void OnScoreChanged(int score)
        {
            _scoreText.text = $"{score}";
        }

        public void Dispose()
        {
            ScoringManager.Instance.OnScoreChanged -= OnScoreChanged;
        }
    }
}