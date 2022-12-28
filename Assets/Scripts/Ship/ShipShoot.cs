using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using SpaceGame.Input;
using SpaceGame.Infrastructure;

namespace SpaceGame.Ship{
public class ShipShoot : NetworkBehaviour
{
    [SerializeField]
    private float bulletSpeedModifier = 150f;
    [SerializeField]
    private Transform weapons;
    [SerializeField]
    private float weapons_range;

    private Rigidbody rb;
    PlayerControls playerControls;
    [SerializeField]
    LayerMask ignore;
    [SerializeField] Camera cam;

    GameObject target;

    void Awake(){
        playerControls = new PlayerControls();
        playerControls.Ship.PrimaryFire.performed += Shoot;
        playerControls.Ship.PrimaryFire.canceled += Shoot;
        rb = GetComponent<Rigidbody>();
    }

    void Start(){
        if(!IsServer) return;

    }

    public void Shoot(InputAction.CallbackContext ctx){
        if(cam == null){
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        requestFireServerRpc(cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2)), Vector3.Distance(cam.transform.position, transform.position));
    }

    
    public override void OnNetworkSpawn(){
        enabled = true;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //TODO: MOVE THIS SOMEWHERE ELSE THIS IS DISGUSTING
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        gameObject.tag = "Player";
    }

    void OnEnable(){
        if(IsLocalPlayer)
            playerControls.Ship.Enable();
    }

    void OnDisable(){
        if(IsLocalPlayer)
            playerControls.Ship.Disable();
    }

    void performFire(Ray aimDirection, float distance){
        if(!IsServer) return;
        if(target != null){
            performFireClientRpc(target.transform.position);
            return;
        }
        performFireClientRpc(FireDistanceRay(aimDirection, distance));


    }

    int last_weapon_index = -1;
    GameObject getWeaponBulletSpawnPoint(){
        if(weapons.childCount == 0) return weapons.gameObject;
        if(last_weapon_index == -1){
            last_weapon_index = 0;
        } else {
            last_weapon_index = (last_weapon_index + 1) % weapons.childCount;
        }
        return weapons.GetChild(last_weapon_index).gameObject;
    }
    
    void SpawnBullet(Vector3 fireDirection){
        //Setup Object
        GameObject bullet = ObjectPoolManager.Instance.GetObjectFromPool("RedLaserBullet");
        LaserBullet laserBullet = bullet.GetComponent<LaserBullet>();

        GameObject gun = getWeaponBulletSpawnPoint();
        bullet.transform.position = gun.transform.position;
        weapons.GetChild(gun.transform.GetSiblingIndex()).GetComponent<AudioSource>().Play();
        bullet.transform.LookAt(fireDirection);
        Vector3 velocity = bullet.transform.forward * (bulletSpeedModifier + transform.InverseTransformDirection(rb.velocity).z);
        float bulletLifeTime = weapons_range / velocity.magnitude;
        bullet.GetComponent<LaserBullet>().velocity = velocity;
        bullet.GetComponent<LaserBullet>().StartCoroutine(bullet.GetComponent<LaserBullet>().DestroyAfterTime(bulletLifeTime));
    }

    Vector3 FireDistanceRay(Ray aimDirection, float distance){
        RaycastHit hit;
        if(Physics.Raycast(aimDirection, out hit, weapons_range+distance, ~ignore)){
            return hit.point;
        }
        return aimDirection.origin + aimDirection.direction * (weapons_range+distance);
    }

    [ClientRpc]
    public void performFireClientRpc(Vector3 fireDirection){
        SpawnBullet(fireDirection);
    }

    [ServerRpc]
    public void requestFireServerRpc(Ray aimDirection, float distance){
        performFire(aimDirection, distance);
    }
}
}