using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseController : MonoBehaviour
{
    ShipMovement ship_movement;
    bool mouse_locked = true;

    void Start()
    {
        ship_movement = GetComponent<ShipMovement>();
        Screen.fullScreen = true;
        changeCursorMode(true);
    }

    void changeCursorMode(bool mode){
        if(mode){
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void changeMouseLocked(){
        Debug.Log("changing mouse");
        mouse_locked = !mouse_locked;
        changeCursorMode(mouse_locked);
    }

    #region Input methods
    public void onMouseControl(InputAction.CallbackContext context){
        if(context.performed){
             changeMouseLocked();
        }
    }

    #endregion
}
