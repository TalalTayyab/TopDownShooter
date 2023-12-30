using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class MissleScript : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _missleSprite;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _scaleLimit;
    [SerializeField] private GameObject _cirlce;
    [SerializeField] private GameObject _firePrefab;
    public AnimationCurve _animationCurve;

    //private GameObject _player;
    //private Camera _camera;
    private Vector3 start;
    private Vector3 _target;

    // Start is called before the first frame update
    void Start()
    {
       // _player = GameObject.Find("Player");
      //  _camera = Camera.main;
        start = transform.position;
        //transform.position = _player.transform.position;

        // var pos = _camera.ScreenToWorldPoint(new Vector3(transform.position.x, _camera.pixelHeight + 32, transform.position.z));
        //_missleSprite.transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);
        _missleSprite.transform.position = start;
    }

    public void Setup(Vector3 target)
    {
        _target = target;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (_shadow.transform.localScale.x <= _scaleLimit || _shadow.transform.localScale.y <= _scaleLimit)
        //{
        //    StartCoroutine(BombDrop());

        //}
        //else
        //{
        //    _shadow.transform.localScale = new Vector3(_shadow.transform.localScale.x - Time.deltaTime, _shadow.transform.localScale.y - Time.deltaTime, _shadow.transform.localScale.z);
        //}
       // DrawCircle(_explosionRadius);
        StartCoroutine(Curve());


    }

    public void DrawCircle(float radius)
    {
        var segments = 360;
        _cirlce.transform.position = _target;
        var line = _cirlce.GetComponent<LineRenderer>();
        line.positionCount = segments + 1;
        line.useWorldSpace = false;

        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }


IEnumerator Curve()
    {
        _shadow.transform.position = _target;

        float duration = 1f;
        float time = 0f;
        // _player.transform.position- (_player.transform.forward * 0.55f)
        Vector3 end =  _target; // lead the target a bit to account for travel time, your math will vary

        while (time < duration)
        {
            time += Time.deltaTime;

            float linearT = time / duration;
            float heightT = _animationCurve.Evaluate(linearT);

            float height = Mathf.Lerp(0f, 4.0f, heightT); // change 3 to however tall you want the arc to be

            _missleSprite.transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);

            yield return null;
        }
        Destroy(gameObject);
        // var explosion = Instantiate(_explosionPrefab, _target, transform.rotation);
        var fire = Instantiate(_firePrefab, _target, transform.rotation);
        Explode(_target);
        Destroy(fire, 3);
       // Destroy(explosion, 1);

    }

    //IEnumerator BombDrop()
    //{
    //    while (_missleSprite.transform.position.y >= transform.position.y)
    //    {
    //        _missleSprite.transform.position = new Vector3(_missleSprite.transform.position.x, _missleSprite.transform.position.y - 1, _missleSprite.transform.position.z);
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //    Destroy(gameObject);
    //    var explosion = Instantiate(_explosionPrefab, transform.position, transform.rotation);
    //    Explode();
    //    Destroy(explosion, 1);

    //}

    private void Explode(Vector3 location)
    {
        var colliders = Physics2D.OverlapCircleAll(location, _explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Player>())
            {
                var healthController = collider.GetComponent<HealthController>();
                healthController.TakeDamage(5, false);
            }
        }
    }
}
