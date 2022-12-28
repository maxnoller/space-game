using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using Cinemachine;

namespace SpaceGame.Ship{
public class CameraController : NetworkBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera virtual_camera;
    
    void Start() {
        if(IsLocalPlayer){
            virtual_camera.enabled = true;
        }
    }

}
}