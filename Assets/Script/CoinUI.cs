using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] TMP_Text _coinText;
    private int Coins;
    public int LevelCoins { get; private set; }
    public void AddCoins()
    {
        Coins++;
        LevelCoins++;
        _coinText.text = $"Coins:{Coins}";
    }

    public void NewLevel()
    {
        LevelCoins = 0;
    }
}
