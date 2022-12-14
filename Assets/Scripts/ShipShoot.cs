using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class ShipShoot : NetworkBehaviour
{
    PlayerControls playerControls;

    void Awake(){
        playerControls = new PlayerControls();
        playerControls.Ship.PrimaryFire.performed += ctx => requestFireServerRpc();
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
        ObjectPoolManager.Instance.GetObjectPool("RedLaserBullet")
    }

    [ServerRpc]
    public void requestFireServerRpc(){
        performFire();
    }
}
