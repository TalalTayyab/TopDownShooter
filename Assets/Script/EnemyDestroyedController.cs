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
        var rnd = Random.Range(0, 3);
        if (rnd == 0)
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.Health);
        }
        else if (rnd == 1)
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.FastWeapon);
        }
        else
        {
            collectableObj.GetComponent<CollectableScript>().SetCollectableType(CollectableScript.CollectableTypeEnum.MultiShot);
        }
    }
 
}
