using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class ShipMovement : NetworkBehaviour
{
    [Header("=== Ship Movement Settings ===")]
    [SerializeField]
    private float yawTorque = 500f;
    [SerializeField]
    private float pitchTorque = 1000f;
    [SerializeField]
    private float rollTorque = 1000f;
    [SerializeField]
    private float thrust = 100f;
    [SerializeField]
    private float maxSpeed = 500f;
    [SerializeField]
    private float brakeFactor = 0.75f;
    [SerializeField]
    private float rollDecay = 0.75f;
    [SerializeField]
    private float dragCoefficient =  0.4f;

    Rigidbody rb;
    private float thrustInput;
    private float upDown1D;
    private float roll1D;
    private float brake;
    private Vector2 pitchYaw;
    PlayerControls playerControls;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    void Awake(){
        playerControls = new PlayerControls();
        playerControls.Ship.Thrust.performed += ctx => thrustInput = ctx.ReadValue<float>();
        playerControls.Ship.Thrust.canceled += ctx => thrustInput = 0f;
        playerControls.Ship.Brake.performed += ctx => brake = ctx.ReadValue<float>();
        playerControls.Ship.Brake.canceled += ctx => brake = 0f;
        playerControls.Ship.Roll.performed += ctx => roll1D = ctx.ReadValue<float>();
        playerControls.Ship.Roll.canceled += ctx => roll1D = 0f;
        playerControls.Ship.PitchYaw.performed += ctx => pitchYaw = ctx.ReadValue<Vector2>();
        playerControls.Ship.PitchYaw.canceled += ctx => pitchYaw = Vector2.zero;
    }

	public override void OnNetworkSpawn(){
		enabled = true;
	}

    void FixedUpdate()
    {
        if(IsLocalPlayer){
            HandleMovement(roll1D, brake, thrustInput, pitchYaw);
            HandleMovementServerRpc(roll1D, brake, thrustInput, pitchYaw);
        }
    }

    //RPC to handle movement
    [ServerRpc]
    void HandleMovementServerRpc(float roll, float brake, float thrust, Vector2 pitchYaw){
        HandleMovement(roll, brake, thrust, pitchYaw);
    }

    void HandleMovement(float roll, float brake, float thrust, Vector2 pitchYaw){
        rb.AddRelativeTorque(Vector3.back * roll * rollTorque * Time.deltaTime);
        rb.AddRelativeTorque(Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.deltaTime);
        rb.AddRelativeTorque(Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.deltaTime);
        if(brake != 0)
            rb.AddForce(-brakeFactor*rb.velocity*brake*Time.deltaTime);

        if(thrust != 0){
            float currentThrust = this.thrust;
            rb.AddRelativeForce(Vector3.forward  * currentThrust * Time.deltaTime);
        }
        Vector3 test = transform.InverseTransformDirection(rb.velocity);
        print("Magnitude: "+rb.velocity.magnitude+" Velocity X: "+test.x+" Velocity Y:"+test.y+" Velocity Z:"+test.z);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        if(test.z > 0.1f){
            rb.AddRelativeForce(- dragCoefficient * new Vector3(test.x, test.y, 0), ForceMode.Force);
        } else {
            rb.AddRelativeForce(- dragCoefficient * new Vector3(test.x, test.y, test.z), ForceMode.Force);
        }
    }

    void OnEnable(){
        if(IsLocalPlayer)
            playerControls.Ship.Enable();
    }

    void OnDisable(){
        if(IsLocalPlayer)
            playerControls.Ship.Disable();
    }

}
