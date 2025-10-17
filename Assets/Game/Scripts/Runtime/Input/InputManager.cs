using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private void OnEnable()
    {
        _inputReader.PauseEvent += OnPause;
    }

    private void OnDisable()
    {
        _inputReader.PauseEvent -= OnPause;
    }

    private void OnPause()
    {
        Debug.Log("Game paused!");
        // _inputReader.SwitchToUI();
    }
}
