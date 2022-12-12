using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ShipController : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private Rigidbody rigid_body;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rollSpeed = 5f;

    [SerializeField]
    bool camera_locked = false;
    [SerializeField]
    private GameObject main_camera;

    private void Start(){
        if(!IsLocalPlayer){
            main_camera.SetActive(false);
        }
        rigid_body = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate(){
        if (Input.GetKey(KeyCode.W)){
            speed = Mathf.Lerp(speed, movementSpeed, Time.deltaTime);
        } else if(Input.GetKeyDown(KeyCode.S)){
            speed = Mathf.Lerp(speed, movementSpeed/10, Time.deltaTime);
        }
        Debug.Log(speed);
        Vector3 move_direction = new Vector3(0,0,speed);
        move_direction = transform.TransformDirection(move_direction);
        rigid_body.AddForce(move_direction);

        Vector3 torque = new Vector3();
        if(camera_locked){
            torque.x = -Input.GetAxis("Mouse Y");
            torque.y = Input.GetAxis("Mouse X");
        }
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        rigid_body.AddRelativeTorque(torque);
    }

    float pressedTime;
    private void Update(){
        if(pressedTime+0.5f < Time.time && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Space)){
            pressedTime = Time.time;
            camera_locked = !camera_locked;
            Cursor.visible = !camera_locked;

        }
        
    }

}
