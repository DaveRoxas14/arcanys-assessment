using System;
using UnityEngine;

namespace Game.Scripts.Runtime.GameTimer
{
    public class GameTime : MonoBehaviour
    {
        public event Action<float> OnTimerTick;
        public event Action OnTimerEnd;
        
        
        private float _timeRemaining;
        private bool _isRunning;
        private float _duration;
        
        public bool IsRunning => _isRunning;
        
        public float TimeRemaining => _timeRemaining;
        
        public float Progress => 1f - (_timeRemaining / _duration);

        private void Update()
        {
            if(!IsRunning) return;
            
            _timeRemaining -= Time.deltaTime;
            OnTimerTick?.Invoke(_timeRemaining);

            if (_timeRemaining <= 0f)
            {
                Stop();
                OnTimerEnd?.Invoke();
            }
        }

        public void StartTimer(float newDuration = -1f)
        {
            if (newDuration > 0f) _duration = newDuration;
            
            _timeRemaining = _duration;
            _isRunning = true;
        }
        
        
        public void Pause() => _isRunning = false;

        public void Resume() => _isRunning = true;
        
        public void Stop()
        {
            _isRunning = false;
            _timeRemaining = 0f;
        }
    }
}