using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TMP_Text _scoreText;

    private void Awake()
    {
        _scoreText = GetComponent<TMP_Text>();
        _scoreText.text = $"Score:0";
    }

    public void UpdateScore (ScoreController scoreController) 
    {
        _scoreText.text = $"Score:{scoreController.Score}"; 
    }
}
