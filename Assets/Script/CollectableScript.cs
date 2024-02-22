using UnityEngine;
using UnityEngine.Events;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] private float _autoDestoryTime;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed;
    HealthController _playerHealthController;
    GameObject _player;
    bool hasCollided;
    public UnityEvent OnCoinCollected;
    private CircleCollider2D _collider;


    private void Awake()
    {
        _player = FindObjectOfType<Player>().gameObject;
        _playerHealthController = FindObjectOfType<Player>().GetComponent<HealthController>();
        // Destroy(gameObject, _autoDestoryTime);
        _collider = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        _collider.radius = PowerUpManagerFactory.PowerUpManager.CoinPickupRange;
        if (hasCollided)
        {
            transform.position = Vector3.Slerp( transform.position, _player.transform.position, _speed);

            var diff = transform.position - _player.transform.position;
            if (Mathf.Abs(diff.magnitude) < 0.4f)
            {
                OnCoinCollected.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            hasCollided = true;
            _animator.SetBool("IsCollected", true);
            //Destroy(gameObject,1);
        }
    }

}
