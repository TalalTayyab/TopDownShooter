using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    HealthController _playerHealthController;
    Shoot _playerShoot;
    CollectableTypeEnum _collectableType = CollectableTypeEnum.Health;
    public enum CollectableTypeEnum { Health = 0, FastWeapon = 1 , MultiShot =2 };
    private void Awake()
    {
        _playerHealthController = FindObjectOfType<Player>().GetComponent<HealthController>();
        _playerShoot = FindObjectOfType<Player>().GetComponent<Shoot>();
    }

    public void SetCollectableType(CollectableTypeEnum collectableType)
    {
        switch (collectableType)
        {
            case CollectableTypeEnum.Health:
                _spriteRenderer.color = Color.green;
                break;

            case CollectableTypeEnum.FastWeapon:
                _spriteRenderer.color = Color.yellow;
                break;

            case CollectableTypeEnum.MultiShot:
                _spriteRenderer.color = Color.red;
                break;
        }

        _collectableType = collectableType;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>())
        {
            if (_collectableType == CollectableTypeEnum.Health)
            {
                _playerHealthController.AddHealth(10);
               
            }

            if (_collectableType == CollectableTypeEnum.FastWeapon)
            {
                _playerShoot.SetTimeBetweenShots(0.2f);

            }

            if (_collectableType == CollectableTypeEnum.MultiShot)
            {
                _playerShoot.SetMultiShot(true);
            }


            Destroy(gameObject);
        }
    }

}
