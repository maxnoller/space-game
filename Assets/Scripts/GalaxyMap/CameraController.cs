using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceGame.Input;

namespace SpaceGame.GalaxyMap{
public class CameraController : MonoBehaviour
{
    PlayerControls controls;
    // Start is called before the first frame update
    void Awake(){
        controls = new PlayerControls();
        controls.GalaxyMap.Camera.performed += ctx => MoveCamera(ctx.ReadValue<Vector2>());
    }

    void OnEnable(){
        controls.GalaxyMap.Enable();
    }

    void OnDisable(){
        controls.GalaxyMap.Disable();
    }

    void MoveCamera(Vector2 direction){
        transform.Translate(direction.x, 0, direction.y);
    }
}
}