using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShipRadarUI : MonoBehaviour
{
    ShipRadar radar;
    void Start()
    {
        radar = GetComponent<ShipRadar>();
        radar.syncedDetectedEnemies.OnListChanged += OnRadarChanged;
    }

    public void OnRadarChanged(NetworkListEvent<NetworkObjectReference> changeEvent)
    {
        Debug.Log("Radar changed");
        NetworkObject networkObj;
        changeEvent.Value.TryGet(out networkObj, NetworkManager.Singleton);
        Debug.Log(networkObj.name);
    }

    void DrawEnemyUI(){
        foreach (var enemy in radar.syncedDetectedEnemies)
        {
            NetworkObject networkObj;
            enemy.TryGet(out networkObj, NetworkManager.Singleton);
            if (networkObj != null)
            {
                Vector3 enemyPos = networkObj.transform.position;
                if (isOnScreen(enemyPos))
                {
                    networkObj.transform.Find("EnemyMarker").gameObject.SetActive(true);
                    
                }
            }
        }
    }

    void Update()
    {
        DrawEnemyUI();
    }

    bool isOnScreen(Vector3 position)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

}
