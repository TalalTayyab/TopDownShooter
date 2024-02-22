using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExperienceUI : MonoBehaviour
{
    TMP_Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void UpdateExperience(int current, int required)
    {
        _text.text = $"EXP:{current}/{required}";
    }
}
