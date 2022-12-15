using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class ShipShoot : NetworkBehaviour
{
    [SerializeField]
    private float bulletSpeedModifier = 150f;
    [SerializeField]
    private float bulletLifeTime = 15f;
    [SerializeField]
    private Transform weapons;
    [SerializeField]
    private float weapons_range;

    private Rigidbody rb;
    PlayerControls playerControls;


    void Awake(){
        playerControls = new PlayerControls();
        playerControls.Ship.PrimaryFire.performed += ctx => requestFireServerRpc();
        rb = GetComponent<Rigidbody>();
    }

    
    public override void OnNetworkSpawn(){
        enabled = true;
    }

    void OnEnable(){
        if(IsLocalPlayer)
            playerControls.Ship.Enable();
    }

    void OnDisable(){
        if(IsLocalPlayer)
            playerControls.Ship.Disable();
    }

    void performFire(){
        if(!IsServer) return;
        performFireClientRpc();

    }

    int last_weapon_index = -1;
    Vector3 getWeaponBulletSpawnPoint(){
        if(weapons.childCount == 0) return weapons.position;
        if(last_weapon_index == -1){
            last_weapon_index = 0;
        } else {
            last_weapon_index = (last_weapon_index + 1) % weapons.childCount;
        }
        return weapons.GetChild(last_weapon_index).position;
    }
    
    void SpawnBullet(){
        GameObject bullet = ObjectPoolManager.Instance.GetObjectFromPool("RedLaserBullet");
        Quaternion newRot = transform.rotation;
        bullet.transform.position = getWeaponBulletSpawnPoint();
        bullet.transform.LookAt(transform.forward * weapons_range + transform.position);
        bullet.GetComponent<LaserBullet>().velocity = bullet.transform.forward * (bulletSpeedModifier + rb.velocity.z);
        bullet.GetComponent<LaserBullet>().lifeTime = bulletLifeTime;
        bullet.GetComponent<LaserBullet>().StartCoroutine(bullet.GetComponent<LaserBullet>().DestroyAfterTime());
    }

    [ClientRpc]
    public void performFireClientRpc(){
        SpawnBullet();
    }

    [ServerRpc]
    public void requestFireServerRpc(){
        performFire();
    }
}
