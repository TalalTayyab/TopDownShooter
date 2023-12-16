using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _rotationSpeed = 100;
    [SerializeField] private float _screenBorder;
    [SerializeField] private GameObject _enemyHealthBar;
    [SerializeField] private bool _animateDirection;
    [SerializeField] private GameObject _graphics;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _controller;
    private Vector2 _targetDirection;
    private float _changeDirectionCoolDown;
    private Camera _camera;
    private Animator _animator;
    private Quaternion _rotation;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void LateUpdate()
    {
        if (_animateDirection)
        {
            _graphics.transform.rotation = _rotation;

        }
    }

    void UpdateTargetDirection()
    {
        //HandleRandomDirectionChange();
        HandlePlayerTargetting();
      //  HandleEnemyOffScreen();
    }

    public void Attack(bool attacking)
    {
        if (_animateDirection)
        {
            _animator.SetBool("Attacking", attacking);
        }
    }

    private void HandleEnemyOffScreen()
    {
        Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

        if ((screenPosition.x < _screenBorder && _targetDirection.x < 0) ||
                (screenPosition.x > _camera.pixelWidth - _screenBorder && _targetDirection.x > 0))
        {
            _targetDirection = new Vector2(-_targetDirection.x, _targetDirection.y);
        }
        if ((screenPosition.y < _screenBorder && _targetDirection.y < 0) ||
                (screenPosition.y > _camera.pixelHeight - _screenBorder && _targetDirection.y > 0))
        {
            _targetDirection = new Vector2(_targetDirection.x, -_targetDirection.y);
        }
    }

    private void RotateTowardsTarget()
    {
       
        if (_animateDirection)
        {
            if (_controller.DirectionToPlayer.x != 0 && _controller.DirectionToPlayer.y != 0)
            {
                _animator.SetFloat("MovementX", _controller.DirectionToPlayer.x);
                _animator.SetFloat("MovementY", _controller.DirectionToPlayer.y);
            }

            _rotation = _graphics.transform.rotation;
        }

        var targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _rigidbody.SetRotation(rotation);
        _enemyHealthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void SetVelocity()
    {
        _rigidbody.velocity = transform.up * _speed;// * (_controller.DirectionToPlayer.magnitude);
    }

    private void HandleRandomDirectionChange()
    {
        _changeDirectionCoolDown -= Time.deltaTime;
        if (_changeDirectionCoolDown <= 0)
        {
            float angleChange = UnityEngine.Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;
            _changeDirectionCoolDown = UnityEngine.Random.Range(1f, 5f);
        }
    }

    private void HandlePlayerTargetting()
    {
      //  if (_controller.AwareOfPlayer)
        {
            _targetDirection = _controller.DirectionToPlayer.normalized;
        }
    }
}
