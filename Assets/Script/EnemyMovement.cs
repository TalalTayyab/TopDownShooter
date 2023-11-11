using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _rotationSpeed = 100;
    [SerializeField] private float _screenBorder;

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _controller;
    private Vector2 _targetDirection;
    private float _changeDirectionCoolDown;
    private Camera _camera;
    
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity(); 
    }

    void UpdateTargetDirection()
    {
        //HandleRandomDirectionChange();
        HandlePlayerTargetting();
        HandleEnemyOffScreen();
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
        var targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        _rigidbody.SetRotation(rotation);
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
