using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldScript : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _radius = 1f;
    private GameObject _player1;

    public float Radius => _radius;
    public float Angle { get; set; }

    

    // Start is called before the first frame update
    void Start()
    {
        _player1 = FindObjectOfType<Player>().gameObject;
        transform.position = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Angle += _rotationSpeed * Time.deltaTime;
        Vector2 anchor = _player1.transform.position;
        var offset = new Vector2(Mathf.Sin(Angle), Mathf.Cos(Angle)) * _radius;
        transform.position = anchor + offset;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(1, false);
        }
    }
}
