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
    [SerializeField] private bool _useAwarenessToMove;


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
    private Vector3 _currentVelocity;
    // private Vector3 _playerPos;
    private Renderer _renderer;
    private float notVisibleTime;

    public bool UseFixedPositionForMovement;
    public Vector3 FixedPositionForTarget;
    public bool ReachedPosition;
    public bool DontUseTargetCD;
    public bool IsVisible => notVisibleTime <= 2f || _renderer.isVisible;

    
    public void SetShoot(bool shoot, bool shootMissle)
    {
        _shoot = shoot;
        _shootMissle = shootMissle;
       // _playerPos = Vector3.zero;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerAwarenessController>();
        _camera = Camera.main;
        _animator = GetComponentInChildren<Animator>();

        _firingDelayCurrentValue = _firingDelay;
        _movingDelayCurrentValue = _movingDelay;
        _currentVelocity = transform.up * _speed;

        _changeDirectionCoolDown = 0;

        _renderer = GetComponentInChildren<Renderer>();
        notVisibleTime = 0;

        //  _targetDirection = transform.up;
        HandlePlayerTargetting(true);

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

        if (UseFixedPositionForMovement)
        {
            var diff = Vector3.Distance(transform.position,FixedPositionForTarget);
            if (diff < 0.5f)
            {
                ReachedPosition = true;
                return;
            }
        }

        if (_speed == 0) return;

        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
        VisibilityCheck();

    }

    private void VisibilityCheck()
    {
        if (!_renderer.isVisible)
        {
            notVisibleTime += Time.deltaTime;
        }
        else
        {
            notVisibleTime -= Time.deltaTime;
        }

        if (notVisibleTime <= 0) notVisibleTime = 0;
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
        var playerPos = TargetPosition();

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
        HandlePlayerTargetting(true);
        RotateTowardsTarget(true);
        var bullet = Instantiate(_enemyBullet, transform.position, transform.rotation);
        bullet.GetComponent<EnemyBullet>().Setup(TargetPosition(), _bulletSpeed);
        //var ridigBody2d = bullet.GetComponent<Rigidbody2D>();
        //ridigBody2d.velocity = transform.up * _bulletSpeed;

    }

    private void Move()
    {
        _movingDelayCurrentValue -= Time.deltaTime;

        if (!_useAwarenessToMove && _movingDelayCurrentValue < 0)
        {
            _state = EnemyState.Firing;
            _movingDelayCurrentValue = _movingDelay;
            return;
        }

        if (_useAwarenessToMove && _controller.AwareOfPlayer)
        {
            _state = EnemyState.Firing;
            _movingDelayCurrentValue = _movingDelay;
            return;
        }


        
        UpdateTargetDirection();
        RotateTowardsTarget();
        SetVelocity();
        //_rigidbody.velocity = transform.up * _enemySpeed;
        //_rigidbody.velocity = transform.up * _enemySpeed * (TargetDirection().magnitude);
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
        var dot = Vector3.Dot(transform.right, TargetDirection());
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
      //  Spread();
    }

    public void Attack(bool attacking)
    {
        if (_animateDirection)
        {
            _animator.SetBool("Attacking", attacking);
        }
    }

    /*private void HandleEnemyOffScreen()
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
    }*/

    private void RotateTowardsTarget(bool setImmediately = false)
    {
       
        if (_animateDirection)
        {
            if (TargetDirection().x != 0 && TargetDirection().y != 0)
            {
                _animator.SetFloat("MovementX", TargetDirection().x);
                _animator.SetFloat("MovementY", TargetDirection().y);
            }

            _rotation = _graphics.transform.rotation;
        }

        var rotationSpeed = _rotationSpeed * Time.deltaTime;
        
        var targetRotation = Quaternion.LookRotation(transform.forward, _targetDirection);
        //var rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
        //_rigidbody.SetRotation(rotation);
        transform.rotation = targetRotation;
        
        _enemyHealthBar.transform.rotation = Quaternion.Euler(0, 0, 0);


        if (_dontRoateGraphics)
        {
            _graphics.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    private void SetVelocity()
    {
        //var desiredV = transform.up * _speed;
        //var _steeringV = (desiredV - _currentVelocity) / _rigidbody.mass;
        //_currentVelocity += _steeringV;
        _rigidbody.velocity = transform.up * _speed;// * (TargetDirection().magnitude);
        //_rigidbody.velocity = _currentVelocity;
    }

    /*private void HandleRandomDirectionChange()
    {
        _changeDirectionCoolDown -= Time.deltaTime;
        if (_changeDirectionCoolDown <= 0)
        {
            float angleChange = UnityEngine.Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            _targetDirection = rotation * _targetDirection;
            _changeDirectionCoolDown = UnityEngine.Random.Range(1f, 5f);
        }
    }*/


    private Vector3 TargetDirection()
    {
        if (UseFixedPositionForMovement)
        {
            return FixedPositionForTarget - transform.position; 
        }

        return _controller.DirectionToPlayer;
    }

    private Vector3 TargetPosition()
    {
        if (UseFixedPositionForMovement)
        {
            return FixedPositionForTarget;
        }

        return _controller.Player.position;
    }

    private void HandlePlayerTargetting(bool immediate = false)
    {
        if (UseFixedPositionForMovement) immediate = true;

        //  if (_controller.AwareOfPlayer)
        _changeDirectionCoolDown -= Time.deltaTime;

        //if (_changeDirectionCoolDown<=0 || immediate || DontUseTargetCD)
        {
            //_playerPos = TargetPosition();
            _targetDirection = TargetDirection().normalized;
            _changeDirectionCoolDown = _changeDirectionCoolDown = UnityEngine.Random.Range(0f, 2f);
        }
    }

    private void Spread()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            if (enemy == gameObject) continue;

            var distance = Vector2.Distance(enemy.transform.position, gameObject.transform.position);

            if (distance < 1.5f)
            {
                var direction =  transform.position - enemy.transform.position;
                _targetDirection = direction.normalized;
            }
        }
    }


}
