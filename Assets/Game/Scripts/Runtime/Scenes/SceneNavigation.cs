using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneNavigation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button _playGameBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _quitBtn;

    [Header("Settings")]
    [SerializeField] private int _sceneIndex;

    private void OnEnable()
    {
        _playGameBtn.onClick.AddListener(LoadMainLevel);
        _quitBtn.onClick.AddListener(QuitGame);
    }

    private void OnDisable()
    {
        _playGameBtn.onClick.RemoveListener(LoadMainLevel);
        _quitBtn.onClick.RemoveListener(QuitGame);
    }

    private void LoadMainLevel()
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    
}