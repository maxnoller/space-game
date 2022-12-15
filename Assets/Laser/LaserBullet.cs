using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBullet : MonoBehaviour
{
    [SerializeField]
    public Vector3 velocity = new Vector3(0,0,0.1f);
    [SerializeField]
    public float lifeTime = 15f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public IEnumerator DestroyAfterTime(){
        yield return new WaitForSeconds(lifeTime);
        ObjectPoolManager.Instance.ReturnObjectToPool("RedLaserBullet", gameObject);
    }

}
