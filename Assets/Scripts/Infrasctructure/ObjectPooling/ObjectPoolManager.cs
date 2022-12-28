using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace SpaceGame.Infrastructure{
public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject poolPrefab;
    Dictionary<string, ObjectPool> poolDictionary = new Dictionary<string, ObjectPool>();

    public static ObjectPoolManager Instance;
    private void Awake() {
        Instance = this;
    }

    public void CreatePool(GameObject prefab, int poolSize){
        string poolKey = prefab.name;
        if(!poolDictionary.ContainsKey(poolKey)){
            poolDictionary.Add(poolKey, Instantiate(poolPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ObjectPool>().CreatePool(prefab, poolSize));
        }
    }

    public ObjectPool GetObjectPool(string name){
        if(poolDictionary.ContainsKey(name)){
            return poolDictionary[name];
        }
        return null;
    }

    public void ReturnObjectToPool(string name, GameObject obj){
        if(poolDictionary.ContainsKey(name)){
            poolDictionary[name].ReturnObject(obj);
        }
    }

    public GameObject GetObjectFromPool(string name){
        if(poolDictionary.ContainsKey(name)){
            return poolDictionary[name].GetObject();
        }
        return null;
    }


     
}
}
