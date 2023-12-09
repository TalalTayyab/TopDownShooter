using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAllocator : MonoBehaviour
{
    private CoinUI _coinUI;

    private void Awake()
    {
        _coinUI = FindObjectOfType<CoinUI>();
    }

    public void AddCoin()
    {
        _coinUI.AddCoins();

    }
}
