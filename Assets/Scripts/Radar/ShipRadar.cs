using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class ShipRadar : NetworkBehaviour
{
    [SerializeField]
    private float detectionRange = 1000f;
    [SerializeField]
    private LayerMask enemyMask;
    Collider cd;
    public NetworkList<NetworkObjectReference> syncedDetectedEnemies;

    
    void Awake()
    {
        cd = GetComponent<Collider>();
        syncedDetectedEnemies = new NetworkList<NetworkObjectReference>(default, NetworkVariableReadPermission.Owner);
    }


    void FixedUpdate(){
        if(!IsServer) return;
        DetectEnemies();
    }

    void DetectEnemies(){
        Collider[] newDetectedEnemies = Physics.OverlapSphere(transform.position, detectionRange, enemyMask);
        if(newDetectedEnemies.Length < syncedDetectedEnemies.Count){
            syncedDetectedEnemies.Clear();
        }

        foreach(Collider c in newDetectedEnemies){
            NetworkObjectReference cNetworkObjectReference = new NetworkObjectReference(c.GetComponent<NetworkObject>());
            if(!syncedDetectedEnemies.Contains(cNetworkObjectReference)){
                syncedDetectedEnemies.Add(cNetworkObjectReference);
            }
        }
    }

}
