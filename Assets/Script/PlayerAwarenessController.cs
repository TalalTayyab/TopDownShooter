using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer {get; private set;}
    
    public Vector3 DirectionToPlayer { get; private set;}
    [SerializeField] private float _playerAwarenessDistance;
    private Transform _player;

    public Transform Player => _player;

    private void Awake()
    {
        _player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        var distanceToPlayer = _player.position - transform.position;
        DirectionToPlayer = distanceToPlayer;

        if (distanceToPlayer.magnitude <= _playerAwarenessDistance)
        {
            AwareOfPlayer = true;
        }
        else
        {
            AwareOfPlayer = false;
        }
    }
}
