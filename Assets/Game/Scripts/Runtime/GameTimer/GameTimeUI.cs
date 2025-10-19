using System;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Runtime.GameTimer
{
    public class GameTimeUI : MonoBehaviour
    {
        #region Members

        [Header(ArcanysConstants.INSPECTOR.REFERENCES)]
        [SerializeField]
        private TextMeshProUGUI _timerText;

        #endregion

        #region Unity Functions

        private void Start()
        {
            var gameTimer = GameManager.Instance.GameTime;

            gameTimer.OnTimerTick += OnTimerTick;
            gameTimer.OnTimerEnd += OnTimerFinished;
        }

        private void OnDisable()
        {
            var gameTimer = GameManager.Instance.GameTime;
            
            gameTimer.OnTimerTick -= OnTimerTick;
            gameTimer.OnTimerEnd -= OnTimerFinished;
        }

        #endregion

        #region Timer Functions

        private void OnTimerTick(float timeLeft)
        {
            float minutes = Mathf.FloorToInt(timeLeft / 60);
            float seconds = Mathf.FloorToInt(timeLeft % 60);
            
            _timerText.text = $"{minutes:00}:{seconds:00}";

            if (timeLeft <= 5f)
            {
                _timerText.color = Color.red;
            }
        }

        private void OnTimerFinished()
        {
            _timerText.text = "";
        }

        #endregion
    }
}