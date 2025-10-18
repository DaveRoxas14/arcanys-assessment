using System;
using System.Collections;
using System.Threading;
using Game.Scripts.Runtime.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneNavigation : MonoBehaviour
{
    #region Members

    [Header("References")]
    [SerializeField] private Button _playGameBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _quitBtn;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Fader _gameStartFade;

    [Header("Settings")]
    [SerializeField] private int _sceneIndex;

    #endregion

    #region Unity Functions

    private async void Start()
    {
        try
        {
            _inputReader.SwitchToUI();
            StartCoroutine(SelectDefaultButton());
            
            var cts = new CancellationTokenSource();
            await _gameStartFade.FadeImage(1f, 0f, 1, cts.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

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

    #endregion

    #region Button Events

    private IEnumerator SelectDefaultButton()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(_playGameBtn.gameObject);
    }

    private async void LoadMainLevel()
    {
        try
        {
            var cts = new CancellationTokenSource();
            await _gameStartFade.FadeImage(0f, 1f, 1f, cts.Token);
            
            SceneManager.LoadScene(_sceneIndex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    #endregion
    
}