using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] public float _speed = 1;
    [SerializeField] private float _rotationSpeed = 100;
    [SerializeField] private float _screenBorder;
    [SerializeField] private GameObject _enemyHealthBar;
    [SerializeField] private bool _animateDirection;
    [SerializeField] private GameObject _graphics;
    [SerializeField] private bool _flip;
    [SerializeField] private SpriteRenderer _spriteRender;
    [SerializeField] private bool _dontRoateGraphics;
    [SerializeField] private bool _shoot;
    
    [SerializeField] private GameObject _enemyBullet;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _firingDelay;
    [SerializeField] private float _movingDelay;
    [SerializeField] private int _damageAmount;
    [SerializeField] private bool _shootMissle;
    

    private Rigidbody2D _rigidbody;
    private PlayerAwarenessController _controller;
    private Vector2 _targetDirection;
    private float _changeDirectionCoolDown;
    private Camera _camera;
    private Animator _animator;
    private Quaternion _rotation;
    private bool rightFacing = true;

    private float _firingDelayCurrentValue;
    private float _movingDelayCurrentValue;
    enum EnemyState { Moving = 0, Firing = 1 };
    private EnemyState _state = EnemyState.Moving;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerAwarenessController>();
        _targetDirection = transform.up;
        _camera = Camera.main;
        _animator = GetComponentInChildren<Animator>();

        _firingDelayCurrentValue = _firingDelay;
        _movingDelayCurrentValue = _movingDelay;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (_shoot)
        {
            MoveAndShoot();
            return;
        }

        Flip();

        if (_speed == 0) return;

        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
    }

    private void MoveAndShoot()
    {
        switch (_state)
        {
            case EnemyState.Firing:
                Fire();
                break;

            case EnemyState.Moving:
                Move();
                break;
        }
    }

    void ShootMissle()
    {
        var prefab = _enemyBullet;
        var position = transform.position;
        var go=  Instantiate(prefab, position, Quaternion.identity);
        var playerPos = _controller.Player.position;

        go.GetComponent<MissleScript>().Setup(playerPos);

        playerPos = new Vector3(playerPos.x + 1.5f, playerPos.y + 1.5f, playerPos.z);
        go = Instantiate(prefab, position, Quaternion.identity);
        go.GetComponent<MissleScript>().Setup(playerPos);

        playerPos = new Vector3(playerPos.x - 3, playerPos.y - 3, playerPos.z);
        go = Instantiate(prefab, position, Quaternion.identity);
        go.GetComponent<MissleScript>().Setup(playerPos);
    }

    void ShootBullet()
    {
        var bullet = Instantiate(_enemyBullet, transform.position, transform.rotation);
        var ridigBody2d = bullet.GetComponent<Rigidbody2D>();
        ridigBody2d.velocity = transform.up * _bulletSpeed;
    }

    private void Move()
    {
        _movingDelayCurrentValue -= Time.deltaTime;

        if (_movingDelayCurrentValue < 0)
        {
            _state = EnemyState.Firing;
            _movingDelayCurrentValue = _movingDelay;
            return;

        }

        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
        //_rigidbody.velocity = transform.up * _enemySpeed;
        //_rigidbody.velocity = transform.up * _enemySpeed * (_controller.DirectionToPlayer.magnitude);
        //_enemyHealthBar.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Fire()
    {
        if (_firingDelayCurrentValue == _firingDelay)
        {
            if (_shootMissle)
            {
                ShootMissle();
            }
            else
            {
                ShootBullet();
            }
           
        }

        _firingDelayCurrentValue -= Time.deltaTime;

        if (_firingDelayCurrentValue <= 0)
        {
            _state = EnemyState.Moving;
            _firingDelayCurrentValue = _firingDelay;
        }

    }


    private void Flip()
    {
        if (!_flip) return;
        var dot = Vector3.Dot(transform.right, _controller.DirectionToPlayer);
        if (dot < 0)
        {
            // left
            if (rightFacing)
            {
                _spriteRender.flipX = !_spriteRender.flipX;
                rightFacing = false;
            }
        } else
        {
            //right
            if (!rightFacing)
            {
                _spriteRender.flipX = !_spriteRender.flipX;
                rightFacing = true;
            }
        }
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


        if (_dontRoateGraphics)
        {
            _graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

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
