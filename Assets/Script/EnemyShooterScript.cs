using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyShooterScript : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _enemyBullet;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _enemySpeed;
    [SerializeField] private float _firingDelay;
    [SerializeField] private float _movingDelay;
    [SerializeField] private float _damageAmount;
    [SerializeField] private GameObject _enemyHealthBar;

    private Rigidbody2D _rigidbody;
    private float _firingDelayCurrentValue;
    private float _movingDelayCurrentValue;
    private PlayerAwarenessController _controller;
    enum EnemyState { Moving = 0, Firing = 1  };
    private EnemyState _state = EnemyState.Moving;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            var healthController = collision.gameObject.GetComponent<HealthController>();
            healthController.TakeDamage(_damageAmount);
        }
    }
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _firingDelayCurrentValue = _firingDelay;
        _movingDelayCurrentValue = _movingDelay;
        _controller = GetComponent<PlayerAwarenessController>();
    }


    private void FixedUpdate()
    {

        switch (_state)
        {
            case EnemyState.Firing:
                Fire();
                break;

            case EnemyState.Moving:
                MoveAndRotate();
                break;
        }

    }

    private void MoveAndRotate()
    {
        _movingDelayCurrentValue -= Time.deltaTime;

        if (_movingDelayCurrentValue < 0)
        {
            _state = EnemyState.Firing;
            _movingDelayCurrentValue = _movingDelay;
            return;

        }
        RotateTowardsPlayer();
        _rigidbody.velocity = transform.up * _enemySpeed;
        //_rigidbody.velocity = transform.up * _enemySpeed * (_controller.DirectionToPlayer.magnitude);
        _enemyHealthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    private void Fire()
    {
        if (_firingDelayCurrentValue == _firingDelay)
        {
            var bullet = Instantiate(_enemyBullet, transform.position, transform.rotation);
            var ridigBody2d = bullet.GetComponent<Rigidbody2D>();
            ridigBody2d.velocity = transform.up * _bulletSpeed;
        }

        _firingDelayCurrentValue -= Time.deltaTime;

        if (_firingDelayCurrentValue <= 0)
        {
            _state = EnemyState.Moving;
            _firingDelayCurrentValue = _firingDelay;
        }

    }

    private void RotateTowardsPlayer()
    {
        var direction = _controller.DirectionToPlayer.normalized;
        var targetRoation = Quaternion.LookRotation(transform.forward, direction);
        targetRoation = Quaternion.RotateTowards(transform.rotation, targetRoation, _rotationSpeed * Time.deltaTime);
        _rigidbody.SetRotation(targetRoation);
    }
}
