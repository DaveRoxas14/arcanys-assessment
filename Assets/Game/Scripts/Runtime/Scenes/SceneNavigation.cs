using System;
using System.Collections;
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

    [Header("Settings")]
    [SerializeField] private int _sceneIndex;

    #endregion

    #region Unity Functions

    private void Start()
    {
        _inputReader.SwitchToUI();
        StartCoroutine(SelectDefaultButton());
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

    #endregion
    
}