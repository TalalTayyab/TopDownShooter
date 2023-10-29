using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreController : MonoBehaviour
{
    public int Score { get; private set; }
    public UnityEvent OnScoreChanged;
    public void AddScore(int amount)
    {
        Score += amount;
        OnScoreChanged.Invoke();
    }
    
}
