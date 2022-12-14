using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGameApplication : MonoBehaviour
{
    [SerializeField]
    private GameObject redLaserBulletPrefab;
    ObjectPoolManager pool_manager;

    void Start(){
        pool_manager = ObjectPoolManager.Instance;
        pool_manager.CreatePool(redLaserBulletPrefab, 1000);
    }
}
