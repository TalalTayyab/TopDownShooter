using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Transform _gunOffset;
    [SerializeField] private float _timeBetweenShots;
    [SerializeField] private bool _multiShot;
    private float _lastfireTime;
    private bool _fireContiniously;
    private bool _fireSingle;


    public void SetMultiShot(bool multiShot)
    {
        _multiShot = multiShot;
    }

    public void SetTimeBetweenShots(float time)
    {
        _timeBetweenShots = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (_fireContiniously || _fireSingle)
        {
            float timeSinceLastFire = Time.time - _lastfireTime;

            if (timeSinceLastFire >= _timeBetweenShots)
            {
                FireBullet();
                _lastfireTime = Time.time;
                _fireSingle = false;
            }
        }
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
