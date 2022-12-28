using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using SpaceGame.Input;

namespace SpaceGame.Ship{
public class ShipSelector : NetworkBehaviour
{
    [SerializeField]
    [Range(0, 10000)]
    private float selectRange = 200f;
    [SerializeField]
    [Range(0, 10000)]
    private float selectResetRate = 1000f;
    [SerializeField]
    private LayerMask selectMask;
    [SerializeField]
    private GameObject selected;
    public delegate void OnSelectDelegate(GameObject selected);
    public event OnSelectDelegate onSelect;

    PlayerControls playerControls;

    void Awake(){
        playerControls = new PlayerControls();
        playerControls.Ship.Select.performed += Select;
    }

    public void Select(InputAction.CallbackContext ctx){
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
        selectServerRpc(ray);
    }

    [ServerRpc]
    public void selectServerRpc(Ray aimDirection){
        RaycastHit hit;
        if(Physics.Raycast(aimDirection, out hit, selectRange, selectMask)){
            if(hit.collider.gameObject != selected){
                selected = hit.collider.transform.parent.gameObject;
                onSelect?.Invoke(selected);
            }     
        }
    }

    void Update(){
        if(!IsServer) return;
        if(selected != null){
            if(Vector3.Distance(transform.position, selected.transform.position) > selectRange){
                selected = null;
                onSelect?.Invoke(null);
            }
        }
    }

    public override void OnNetworkSpawn(){
        enabled = true;
    }

    void OnEnable(){
        if(!IsLocalPlayer) return;
        playerControls.Ship.Enable();
    }

    void OnDisable(){
        if(!IsLocalPlayer) return;
        playerControls.Ship.Disable();
    }
}
}