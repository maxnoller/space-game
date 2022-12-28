using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceGame.UI{
public class EnemyMarkerUI : MonoBehaviour
{
    [SerializeField]
    GameObject marker;
    public static EnemyMarkerUI Instance;
    GameObject trackingTarget;

    void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(this);
            return;
        }
        Instance = this;
        marker.SetActive(false);
    }

    void Update(){
        if(trackingTarget != null){
            Vector3 screenPos = Camera.main.WorldToScreenPoint(trackingTarget.transform.position);
            bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
            if(onScreen && screenPos.z > 0)
                marker.transform.position = screenPos;
        }
    }

    public void setMarker(GameObject target){
        marker.SetActive(true);
        trackingTarget = target;
    }

    public void resetMarker(){
        marker.SetActive(false);
        trackingTarget = null;
    }
}
}