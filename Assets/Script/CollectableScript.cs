using UnityEngine;
using UnityEngine.Events;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] private float _autoDestoryTime;
    [SerializeField] private Animator _animator;
    HealthController _playerHealthController;
    GameObject _player;
    bool hasCollided;
    public UnityEvent OnCoinCollected;
    


    private void Awake()
    {
        _player = FindObjectOfType<Player>().gameObject;
        _playerHealthController = FindObjectOfType<Player>().GetComponent<HealthController>();
       // Destroy(gameObject, _autoDestoryTime);
    }

    private void FixedUpdate()
    {
        if (hasCollided)
        {
            transform.position = Vector3.Slerp( transform.position, _player.transform.position, 0.2f);

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
            //_animator.SetBool("IsCollected", true);
            //Destroy(gameObject,1);
        }
    }

}
