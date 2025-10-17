using Game.Scripts.Runtime.PauseMenu;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PauseMenuUi _pauseMenuUi;

    #region Unity Functions

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
    
    private void OnEnable()
    {
        _inputReader.PauseEvent += OnPause;
        _pauseMenuUi.OnGamePaused += OnPauseEvent;
        
        _inputReader.SwitchToPlayer();
    }

    private void OnDisable()
    {
        _inputReader.PauseEvent -= OnPause;
        _pauseMenuUi.OnGamePaused -= OnPauseEvent;
    }

    #endregion

    private void OnPause()
    {
        if (_pauseMenuUi.IsPaused)
        {
            Debug.Log("Game unpaused!");
            _pauseMenuUi.HidePauseMenu();
        }
        else
        {
            Debug.Log("Game paused!");
            _pauseMenuUi.ShowPauseMenu();
        }
        
    }

    #region Pause Events

    private void OnPauseEvent(bool isPaused)
    {
        if (isPaused)
        {
            _inputReader.SwitchToUI();
        }
        else
        {
            _inputReader.SwitchToPlayer();
        }
    }

    #endregion

    #region Input Reader

    public void SwitchToUIControls()
    {
        _inputReader.SwitchToUI();
    }

    public void SwitchToPlayerControls()
    {
        _inputReader.SwitchToPlayer();
    }

    #endregion
}
