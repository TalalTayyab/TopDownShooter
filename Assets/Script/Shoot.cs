using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _gunOffset;
    [SerializeField] private float _timeBetweenShots;
    [SerializeField] private bool _multiShot;
    [SerializeField] private GameObject _bombPrefab;
    [SerializeField] private float _bombSpeed;
    [SerializeField] private bool _throwBomb;
    [SerializeField] private bool _shootLaser;
    [SerializeField] LineRenderer _lineRender;
    [SerializeField] private float _laserLength;
    [SerializeField] private float _laserMaxCharge;
    [SerializeField] private float _laserDamage;
    public UnityEvent OnLaserChange;
    private float _lastfireTime;
    private bool _fireContiniously;
    private bool _fireSingle;
    private float _laserCurrentCharge;

    public float RemainingCharge => _laserCurrentCharge / _laserMaxCharge;

    private void Awake()
    {
        _laserCurrentCharge = _laserMaxCharge;
    }

    public void SetMultiShot(bool multiShot)
    {
        _multiShot = multiShot;
    }

    public void SetTimeBetweenShots(float time)
    {
        _timeBetweenShots = time;
    }

    public void SetBomb(bool throwBomb)
    {
        _throwBomb = throwBomb;
    }

    public void SetLaser(bool shootLaser)
    {
        _shootLaser = shootLaser;
    }

    // Update is called once per frame
    void FixedUpdate()
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
                else
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

    }

    private void HideLaser()
    {
        _lineRender.SetPosition(0, Vector3.zero);
        _lineRender.SetPosition(1, Vector3.zero);
    }

    private void ShootLaser(bool drawLaser)
    {
        if (!drawLaser)
        {
            HideLaser();

            _laserCurrentCharge += Time.deltaTime;
            if (_laserCurrentCharge > _laserMaxCharge)
            {
                _laserCurrentCharge = _laserMaxCharge;
            }

        }
        else
        {
            _laserCurrentCharge -= Time.deltaTime;

            if (_laserCurrentCharge <= 0)
            {
                _laserCurrentCharge = 0;
                HideLaser();
                return;
            }

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
                    health.TakeDamage(_laserDamage);

                }

            }
            // Debug.DrawRay(_gunOffset.transform.position, endPoint, Color.green);
            //Debug.DrawLine(_gunOffset.transform.position, endPoint, Color.yellow);
            _lineRender.SetPosition(0, _gunOffset.transform.position);
            _lineRender.SetPosition(1, endPoint);

        }
    }

    private void ThrowBomb()
    {
        var bomb = Instantiate(_bombPrefab, _gunOffset.position, transform.rotation);
        //bomb.GetComponent<Rigidbody2D>().AddForce(transform.up * _bombSpeed, ForceMode2D.Impulse);
        var rigidBody = bomb.GetComponent<Rigidbody2D>();
        rigidBody.velocity = _bulletSpeed * transform.up;

    }

    private void FireBullet()
    {
        InstantitateBullet(0f);
        if (_multiShot)
        {
            InstantitateBullet(15f);
            InstantitateBullet(-15f);
        }
    }

    private void InstantitateBullet(float angle)
    {

        var rotation = Quaternion.Euler(0, 0, angle);
        rotation = transform.rotation * rotation;
        var direction = rotation * Vector3.up;

        var bulletRoation = Quaternion.LookRotation(_gunOffset.forward, direction);

        var bullet = Instantiate(_bulletPrefab, _gunOffset.position, bulletRoation);
        var rigidBody = bullet.GetComponent<Rigidbody2D>();
        rigidBody.velocity = _bulletSpeed * direction;

        //var bullet = Instantiate(_bulletPrefab, _gunOffset.position, bulletRoation);
        //var rigidBody = bullet.GetComponent<Rigidbody2D>();
        //rigidBody.AddForce(_gunOffset.up * _bulletSpeed, ForceMode2D.Impulse);

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
