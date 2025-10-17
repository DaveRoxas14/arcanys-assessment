using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneNavigation : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Button _playGameBtn;

    [Header("Settings")]
    [SerializeField] private int _sceneIndex;

    private void OnEnable()
    {
        _playGameBtn.onClick.AddListener(LoadMainLevel);
    }

    private void LoadMainLevel()
    {
        SceneManager.LoadScene(_sceneIndex);
    }
    
}
