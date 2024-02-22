using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class TimeUIScript : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    private GameTimeScript _gameTimeScript;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameTimeScript = Camera.main.GetComponent<GameTimeScript>();        
    }

    // Update is called once per frame
    void Update()
    {
        var time = TimeSpan.FromSeconds(_gameTimeScript.Seconds);
        string str = time.ToString(@"mm\:ss");
        _text.text = $"Time: {str}";
    }
}
