using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroyedController : MonoBehaviour
{
    [SerializeField] GameObject _collectablePrefab;
    public void DestoryEnemy(float delay)
    {
        Destroy(gameObject, delay);

        var collectableObj = Instantiate(_collectablePrefab, transform.position,Quaternion.Euler(0,0,0));
        var rnd = Random.Range(0, 5);
        if (rnd == 0)
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.Health);
        }
        else if (rnd == 1)
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.FastWeapon);
        }
        else if (rnd == 2)
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.MultiShot);
        }
        else if (rnd == 3)
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.Bomb);
        }
        else if (rnd == 4)
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.Laser);
        }
    }
 
}
