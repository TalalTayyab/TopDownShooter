using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] private float _autoDestoryTime;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _healthSprite;
    [SerializeField] Sprite _multiweaponSprite;
    [SerializeField] Sprite _fastWeaponSprite;
    HealthController _playerHealthController;
    Shoot _playerShoot;
    CollectableTypeEnum _collectableType = CollectableTypeEnum.Health;

    public enum CollectableTypeEnum { Health = 0, FastWeapon = 1 , MultiShot =2 };
    private void Awake()
    {
        _playerHealthController = FindObjectOfType<Player>().GetComponent<HealthController>();
        _playerShoot = FindObjectOfType<Player>().GetComponent<Shoot>();
        Destroy(gameObject, _autoDestoryTime);
    }

    public void SetCollectableType(CollectableTypeEnum collectableType)
    {
        switch (collectableType)
        {
            case CollectableTypeEnum.Health:
                _spriteRenderer.sprite = _healthSprite;
                break;

            case CollectableTypeEnum.FastWeapon:
                _spriteRenderer.sprite = _fastWeaponSprite;
                break;

            case CollectableTypeEnum.MultiShot:
                _spriteRenderer.sprite = _multiweaponSprite;
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
                _playerShoot.SetTimeBetweenShots(0.5f);
                _playerShoot.SetMultiShot(false);

            }

            if (_collectableType == CollectableTypeEnum.MultiShot)
            {
                _playerShoot.SetTimeBetweenShots(1f);
                _playerShoot.SetMultiShot(true);
            }


            Destroy(gameObject);
        }
    }

}
