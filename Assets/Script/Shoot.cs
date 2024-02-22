using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    //[SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _gunOffset;
    //[SerializeField] private float _timeBetweenShots;
    [SerializeField] private bool _multiShot;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _bombSpeed;
    [SerializeField] private bool _throwBomb;
    [SerializeField] private GameObject _dashCDUI;

    // [SerializeField] private bool _shootLaser;
    //  [SerializeField] private bool _fireGun;
    // [SerializeField] LineRenderer _lineRender;
    // [SerializeField] private float _laserLength;
    // [SerializeField] private float _laserMaxCharge;
    // [SerializeField] private int _laserDamage;
    //[SerializeField] private float _shootDelay;
    [SerializeField] private float _multiShootDelay;
    [SerializeField] private float _throwBombDelay;
    [SerializeField] private GameObject _playerShieldPrefab;
    [SerializeField] private int _maxShieldBalls;
    [SerializeField] private int _maxShieldAvailableTime;
    [SerializeField] private int _maxShieldRechargeTime;
  //  public UnityEvent OnLaserChange;
  //  private float _lastfireTime;
    private bool _fireContiniously;
    private bool _fireSingle;
  //  private float _laserCurrentCharge;
    private float _timeUntilShoot;
    private float _timeUntilLastMultiShootDelay;
    private float _timeUntilLastThrowBomb;
    private int _currentShieldBalls;
    private List<PlayerShieldScript> _shieldBalls = new List<PlayerShieldScript>();
    private float _shieldAvailableTime;
    private float _shieldRechargeTime;
    private bool _isShieldAvailable;
    private Rigidbody2D _rigidbody2D;
    

    //public float RemainingCharge => _laserCurrentCharge / _laserMaxCharge;

    private void Awake()
    {
        //_laserCurrentCharge = _laserMaxCharge;
        //_fireGun = true;
        _timeUntilShoot = PowerUpManagerFactory.PowerUpManager.BulletCD;
        _timeUntilLastMultiShootDelay = _multiShootDelay;
        _timeUntilLastThrowBomb = _throwBombDelay;
        _shieldAvailableTime = _maxShieldAvailableTime;
        _shieldRechargeTime = _maxShieldRechargeTime;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetMultiShot(bool multiShot)
    {
        _multiShot = multiShot;
    }

    public void SetTimeBetweenShots(float time)
    {
        //_timeBetweenShots = time;
    }

    public void SetBomb(bool throwBomb)
    {
        _throwBomb = throwBomb;
    }

    public void SetFireGun(bool fireGun)
    {
        //_fireGun = fireGun;
    }

    public void SetLaser(bool shootLaser)
    {
       // _shootLaser = shootLaser;
       // _laserCurrentCharge = _laserMaxCharge;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RotateTowardsEnemy();
        FireBullet();
        FireMultiBullet();
        ThrowBomb();
        PlayerShield();
    }

    /*private void ButtonFire()
    {
        if (_fireContiniously || _fireSingle)
        {
            if (_shootLaser)
            {
                ShootLaser(true);
            }

            float timeSinceLastFire = Time.time - _lastfireTime;

            if (timeSinceLastFire >= _timeBetweenShots)
            {

                if (_throwBomb)
                {
                    ThrowBomb();
                }
                else if (_fireGun)
                {
                    FireBullet();
                }

                _lastfireTime = Time.time;
                _fireSingle = false;
            }
        }
        else
        {
            if (_shootLaser)
            {
                ShootLaser(false);
            }
        }

        OnLaserChange.Invoke();
    }*/

  /*  private void HideLaser()
    {
        _lineRender.SetPosition(0, Vector3.zero);
        _lineRender.SetPosition(1, Vector3.zero);
    }*/

    /*private void ShootLaser()
    {
        if (!_shootLaser) return;

        if (!drawLaser)
        {
            HideLaser();

            _laserCurrentCharge += Time.deltaTime;
            if (_laserCurrentCharge > _laserMaxCharge)
            {
                _laserCurrentCharge = _laserMaxCharge;
            }

        }
        

        _laserCurrentCharge -= Time.deltaTime;
        Debug.Log(_laserCurrentCharge);
        if (_laserCurrentCharge <= 0)
        {
            var layerMask = LayerMask.GetMask("Enemy");
            var hitPoint = Physics2D.Raycast(_gunOffset.transform.position, _gunOffset.transform.up, _laserLength, layerMask);
            var endPoint = _gunOffset.transform.position + (_gunOffset.transform.up * _laserLength);
            if (hitPoint.collider != null)
            {
                var enemy = hitPoint.collider.gameObject.GetComponent<EnemyMovement>();
                if (enemy != null)
                {
                    var health = hitPoint.collider.gameObject.GetComponent<HealthController>();
                    endPoint = hitPoint.point;
                    health.TakeDamage(_laserDamage, false);

                }
            }
            _lineRender.SetPosition(0, _gunOffset.transform.position);
            _lineRender.SetPosition(1, endPoint);

            _laserCurrentCharge = _laserMaxCharge;
            HideLaser();
        }

       
        // Debug.DrawRay(_gunOffset.transform.position, endPoint, Color.green);
        //Debug.DrawLine(_gunOffset.transform.position, endPoint, Color.yellow);
        
    }*/

    private void ThrowBomb()
    {
        if (!_throwBomb) return;

        _timeUntilLastThrowBomb -= Time.deltaTime;

        if (_timeUntilLastThrowBomb <0)
        {
            var bomb = Instantiate(_bombPrefab, _gunOffset.position, transform.rotation);
            //bomb.GetComponent<Rigidbody2D>().AddForce(transform.up * _bombSpeed, ForceMode2D.Impulse);
            _rigidbody2D.velocity = _bombSpeed * transform.up;

            _timeUntilLastThrowBomb = _throwBombDelay;
        }
       

    }

    private void FireBullet()
    {
        _timeUntilShoot -= Time.deltaTime;

        if (_timeUntilShoot <= 0)
        {
            InstantitateBullet(0f);
            _timeUntilShoot = PowerUpManagerFactory.PowerUpManager.BulletCD;
        }
    }

    private void FireMultiBullet()
    {
        if (!_multiShot) return;

        _timeUntilLastMultiShootDelay -= Time.deltaTime;

        if (_timeUntilLastMultiShootDelay <= 0)
        {
            for (var i = 15; i < 360; i += 15)
            {
                InstantitateBullet(i);
            }
            _timeUntilLastMultiShootDelay = _multiShootDelay;
        }
    }

    private void PlayerShield()
    {
        if (_isShieldAvailable)
        {
            _shieldAvailableTime -= Time.deltaTime;

            if (_shieldAvailableTime <= 0)
            {
                //hide shield
                _shieldBalls.ForEach(x => Destroy(x.gameObject));
                _shieldBalls.Clear();
                _shieldRechargeTime = _maxShieldRechargeTime;
                _isShieldAvailable = false;
            }
        }
        else
        {
            _shieldRechargeTime -= Time.deltaTime;

            if (_shieldRechargeTime <= 0)
            {
                //display shield
                _currentShieldBalls = 0;
                _shieldAvailableTime = _maxShieldAvailableTime;
                _isShieldAvailable = true;
            }
        }

        if (_currentShieldBalls >= _maxShieldBalls) return;

        if (_isShieldAvailable)
        {
            _shieldBalls.ForEach(x => Destroy(x.gameObject));
            _shieldBalls.Clear();

            _currentShieldBalls++;

            InstantitatePlayerShield();
        }
    }

    private void InstantitatePlayerShield()
    {
        int delta = 360 / _currentShieldBalls;
        int start = 0;

        for (var i = 0; i < _currentShieldBalls; i++)
        {
            var shield = Instantiate(_playerShieldPrefab, transform);
            var script = shield.GetComponent<PlayerShieldScript>();
            _shieldBalls.Add(script);

            script.Angle = Mathf.Deg2Rad * start;
            start = start + delta;
            
        }
    }

    private void InstantitateBullet(float angle)
    {
        var rotation = Quaternion.Euler(0, 0, angle);
        rotation = transform.rotation * rotation;
        var direction = rotation * Vector3.up;

        var bulletRoation = Quaternion.LookRotation(_gunOffset.forward, direction);

        var bullet = Instantiate(_bulletPrefab, _gunOffset.position, bulletRoation);
        bullet.GetComponent<Rigidbody2D>().velocity = PowerUpManagerFactory.PowerUpManager.BulletSpeed * direction;
    }

    private void RotateTowardsEnemy()
    {
        var enemy = FindNearestEnemy();

        if (enemy == null) return;

        var distanceToPlayer = enemy.gameObject.transform.position - transform.position;
        var direction = distanceToPlayer.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        _rigidbody2D.rotation = angle;

        _dashCDUI.transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    private GameObject FindNearestEnemy()
    {
        int layerMask = LayerMask.GetMask("Enemy");
        var rs = Physics2D.OverlapCircleAll(transform.position, 10, layerMask);
        float closest = Mathf.Infinity;
        GameObject go = null;
        foreach (var r in rs)
        {
            var distance = r.gameObject.transform.position - transform.position;
            float d = distance.sqrMagnitude;
            if (d < closest)
            {
                closest = d;
                go = r.gameObject;
            }
        }
        return go;
    }

    private void OnFire(InputValue inputValue)
    {
        _fireContiniously = inputValue.isPressed;
        if (inputValue.isPressed)
        {
            _fireSingle = true;
        }
    }
}
