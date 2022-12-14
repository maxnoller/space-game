using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    Queue<GameObject> objectPool = new Queue<GameObject>();

    public ObjectPool CreatePool(GameObject prefab, int pool_size){
        for(int i = 0; i < pool_size; i++){
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
        return this;
    }

    public GameObject GetObject(){
        GameObject obj = objectPool.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj){
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }


}
