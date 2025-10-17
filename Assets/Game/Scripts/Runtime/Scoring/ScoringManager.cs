using System;
using UnityEngine;

public class ScoringManager : MonoBehaviour
{
    public static ScoringManager Instance { get; private set; }

    public event Action<int> OnScoreChanged;

    private int _score;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    #region Scoring Helpers

    public void AddScore(int amount)
    {
        _score += amount;
        OnScoreChanged?.Invoke(_score);
    }

    public void RemoveScore(int amount)
    {
        _score -= amount;
        OnScoreChanged?.Invoke(_score);
    }

    #endregion
}
