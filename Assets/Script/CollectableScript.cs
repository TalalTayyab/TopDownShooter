using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] private float _autoDestoryTime;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Sprite _healthSprite;
    [SerializeField] Sprite _multiweaponSprite;
    [SerializeField] Sprite _fastWeaponSprite;
    [SerializeField] Sprite _bombWeaponSprite;
    [SerializeField] Sprite _laserWeaponSprite;
    HealthController _playerHealthController;
    Shoot _playerShoot;
    CollectableTypeEnum _collectableType = CollectableTypeEnum.Health;

    public enum CollectableTypeEnum { Health = 0, FastWeapon = 1 , MultiShot =2, Bomb = 3, Laser = 4 };
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

            case CollectableTypeEnum.Bomb:
                _spriteRenderer.sprite = _bombWeaponSprite;
                break;

            case CollectableTypeEnum.Laser:
                _spriteRenderer.sprite = _laserWeaponSprite;
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
                _playerShoot.SetFireGun(true);
                _playerShoot.SetMultiShot(false);
                _playerShoot.SetBomb(false);
                _playerShoot.SetLaser(false);


            }

            if (_collectableType == CollectableTypeEnum.MultiShot)
            {
                _playerShoot.SetTimeBetweenShots(1f);
                _playerShoot.SetMultiShot(true);
                _playerShoot.SetFireGun(true);
                _playerShoot.SetBomb(false);
                _playerShoot.SetLaser(false);
                
            }

            if (_collectableType == CollectableTypeEnum.Bomb)
            {
                _playerShoot.SetTimeBetweenShots(1f);
                _playerShoot.SetBomb(true);
                _playerShoot.SetMultiShot(false);
                _playerShoot.SetLaser(false);
                _playerShoot.SetFireGun(false);
            }

            if (_collectableType == CollectableTypeEnum.Laser)
            {
                _playerShoot.SetTimeBetweenShots(1f);
                _playerShoot.SetLaser(true);
                _playerShoot.SetMultiShot(false);
                _playerShoot.SetBomb(false);
                _playerShoot.SetFireGun(false);
            }


            Destroy(gameObject);
        }
    }

}
