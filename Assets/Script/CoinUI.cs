using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    [SerializeField] TMP_Text _coinText;
    private int _coins;
    public void AddCoins()
    {
        _coins++;
        _coinText.text = $"Coin: {_coins}";

    }
}
