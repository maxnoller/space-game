using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;
using SpaceGame.UI;

namespace SpaceGame.Ship{
public class ShipTargetManager : NetworkBehaviour
{
    [SerializeField]
    float autoaimDistance = 20f;
    ShipSelector selector;

    NetworkVariable<NetworkObjectReference> target = new NetworkVariable<NetworkObjectReference>(default);
    Camera cam;

    public delegate void OnAutoaimChange(bool autoaim);
    public OnAutoaimChange onAutoaimChange;
    bool autoaim;
    GameObject targetObject;

    void Start()
    {
        if(!IsServer) return;
        selector = GetComponent<ShipSelector>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        selector.onSelect += (GameObject target) => {
            if(target == null){
                this.target.Value = default;
                targetObject = null;
                return;
            }
            this.target.Value = target.GetComponent<NetworkObject>();
            targetObject = target;
        };
    }

    // Activate autoaim when the players aims within the autoaimDistance of the target
    void Update(){
        if(!IsLocalPlayer) return;
        if(target.Value.Equals(default)){
            if(autoaim){
                autoaim = false;
                onAutoaimChange?.Invoke(autoaim);
            }
            return;
        }
        NetworkObject enemy;
        if(target.Value.TryGet(out enemy)){
            if(Vector3.Distance(cam.transform.position, enemy.transform.position) < autoaimDistance){
                if(!autoaim){
                    autoaim = true;
                    onAutoaimChange?.Invoke(autoaim);
                }
            } else {
                if(autoaim){
                    autoaim = false;
                    onAutoaimChange?.Invoke(autoaim);
                }
            }
        }
    }

    public override void OnNetworkSpawn(){
        if(!IsLocalPlayer) return;
        enabled = true;
        target.OnValueChanged += (NetworkObjectReference old, NetworkObjectReference current) => {
            NetworkObject enemy;
            if(current.Equals(default)){
                EnemyMarkerUI.Instance.resetMarker();
            }
            if(current.TryGet(out enemy)){
                EnemyMarkerUI.Instance.setMarker(enemy.gameObject);
            }
        };
    }
}
}