using System;
using System.Threading;
using System.Threading.Tasks;
using Arcanys.Collectibles;
using Game.Scripts.Runtime.Audio;
using Game.Scripts.Runtime.GameOver;
using Game.Scripts.Runtime.GameTimer;
using Game.Scripts.Runtime.UI;
using UnityEngine;

namespace Game.Scripts.Runtime
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        #region Members

        [Header(ArcanysConstants.INSPECTOR.REFERENCES)] 
        [SerializeField]
        private GameOverUI _gameOverUI;

        [SerializeField] private GameTime _time;
        [SerializeField] private Fader _gameStartFade;
        [SerializeField] private GemSpawnManager _gemSpawnManager;
        [SerializeField] private SoundEffect _bgm;
        [SerializeField] private SoundEffect _winBgm;
        [SerializeField] private SoundEffect _loseBgm;
        
        [Header(ArcanysConstants.INSPECTOR.SETTINGS)]
        [SerializeField] private int _scoreToWin = 100;
        [SerializeField] private int _scoreToLose;

        [SerializeField] private float _gameTime;

        private bool _isGameOver;
        private bool _isRestarting;
        private PlayerController _player;

        public bool IsGameOver => _isGameOver;

        public GameTime GameTime => _time;

        public int ScoreToWin => _scoreToWin;

        public int ScoreToLose => _scoreToLose;

        public bool IsRestarting
        {
            get => _isRestarting;
            set => _isRestarting = value;
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public async void Start()
        {
            ScoringManager.Instance.OnScoreChanged += OnScoreChanged;
            _time.OnTimerEnd += OnTimerFinished;

            await _gemSpawnManager.StartSpawningGems();

            await FadeIn();
            
            GameTime.StartTimer(_gameTime);
            AudioManager.Instance.PlayBGM(_bgm.clip, true);
        }

        #endregion

        private void OnScoreChanged(int score)
        {
            if(IsRestarting) return;
            
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
            AudioManager.Instance.PlayBGM(_winBgm.clip);
        }

        private void LoseGame()
        {
            // lose
            // Show Game Over Screen
            _isGameOver = true;
            InputManager.Instance.SwitchToUIControls();
            _gameOverUI.GameOver(false);
            AudioManager.Instance.PlayBGM(_loseBgm.clip);
        }

        #region Helpers

        public void RegisterPlayer(PlayerController player)
        {
            _player = player;
        }

        public PlayerController GetPlayer()
        {
            return _player;
        }

        public bool IsPlayerGrounded()
        {
            return _player.IsGrounded;
        }
        
        public async Task FadeIn()
        {
            try
            {
                var cts = new CancellationTokenSource();
                await _gameStartFade.FadeImage(1f, 0f, 1, cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public async Task FadeOut()
        {
            try
            {
                var cts = new CancellationTokenSource();
                await _gameStartFade.FadeImage(0f, 1f, 1, cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion
    }
}