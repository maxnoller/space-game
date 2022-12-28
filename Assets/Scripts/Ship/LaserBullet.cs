using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Infrastructure;

namespace SpaceGame.Ship{
public class LaserBullet : MonoBehaviour
{
    [SerializeField]
    public Vector3 velocity = new Vector3(0,0,0.1f);

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += velocity * Time.deltaTime;
    }

    public IEnumerator DestroyAfterTime(float time){
        yield return new WaitForSeconds(time);
        ObjectPoolManager.Instance.ReturnObjectToPool("RedLaserBullet", gameObject);
    }

    void OnCollisionEnter(Collision collision){
        Debug.Log("entering");
        if(collision.transform.GetComponent<IDamageable>() != null){
            collision.transform.GetComponent<IDamageable>().damage();
            Debug.Log("Hit: "+collision.transform.name);
        }
    }

}
}
