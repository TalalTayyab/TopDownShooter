using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScoreAllocator : MonoBehaviour
{
    [SerializeField]
    private int _killScore;
    private ScoreController _controller;

    private void Awake()
    {
        _controller = FindObjectOfType<ScoreController>();
    }

    public void AllocateScore()
    {
        _controller.AddScore(_killScore);
    }
}
