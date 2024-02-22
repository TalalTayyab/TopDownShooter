using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeScript : MonoBehaviour
{
    private int _noOfSeconds;

    public int Seconds => _noOfSeconds;

    private void Start()
    {
        StartCoroutine(CalculateGameTime());
    }


    IEnumerator CalculateGameTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _noOfSeconds++;
        }
    }
}
