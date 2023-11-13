using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissleScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private GameObject _missleSprite;
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _shadow;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _scaleLimit;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");
        _camera = Camera.main;
        transform.position = _player.transform.position;
        
        var pos = _camera.ScreenToWorldPoint(new Vector3(transform.position.x, _camera.pixelHeight + 32, transform.position.z));
        _missleSprite.transform.position = new Vector3(transform.position.x, pos.y, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_shadow.transform.localScale.x <= _scaleLimit || _shadow.transform.localScale.y <= _scaleLimit)
        {
            StartCoroutine(BombDrop());
           
        }
        else
        {
            _shadow.transform.localScale = new Vector3(_shadow.transform.localScale.x - Time.deltaTime, _shadow.transform.localScale.y - Time.deltaTime, _shadow.transform.localScale.z);
        }
    }

    IEnumerator BombDrop()
    {
        while (_missleSprite.transform.position.y >= transform.position.y)
        {
            _missleSprite.transform.position = new Vector3(_missleSprite.transform.position.x, _missleSprite.transform.position.y - 1, _missleSprite.transform.position.z);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
        var explosion = Instantiate(_explosionPrefab, transform.position, transform.rotation);
        Explode();
        Destroy(explosion, 1);

    }

    private void Explode()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<Player>())
            {
                var healthController = collider.GetComponent<HealthController>();
                healthController.TakeDamage(5);
            }
        }
    }
}
