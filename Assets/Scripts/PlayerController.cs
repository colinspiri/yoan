using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // components
    public static PlayerController Instance;
    public Transform playerCamera;
    private CharacterController controller;
    
    // private constants
    private float gravity = -13.0f;
    private float moveSmoothTime = 0.3f;
    private float mouseSmoothTime = 0.03f;
    
    // public constants
    [Header("Camera")] 
    public float mouseSensitivity = 3.5f;
    public float mousePitchClamp = 90f;
    public float bobFrequency;
    public float bobMagnitude;
    [Header("Movement")] 
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;
    [Header("Sounds")] 
    public float walkLoudness;

    // private state
    // camera
    private float originalCameraY;
    private float cameraPitch;
    private float cycle;
    // movement
    private Vector3 velocity;
    private Vector2 currentDir;
    private Vector2 currentDirVelocity;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;
    private enum MoveState { Walking, Running, Crouching };
    private MoveState moveState = MoveState.Walking;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
    }

    private void Start() {
        originalCameraY = playerCamera.transform.position.y;
    }

    void Update() {
        if (Time.deltaTime != 0) {
            UpdateMouseLook();
            UpdateMovement();
            
            if(currentDir.magnitude > 0) TorbalanSenses.Instance.ReportSound(transform.position, walkLoudness);
        }
    }

    private void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -mousePitchClamp, mousePitchClamp);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        
        // Bob head
        cycle += velocity.magnitude * bobFrequency * Time.deltaTime;
        cycle %= 2 * Mathf.PI;
        if (currentDirVelocity.magnitude == 0 || moveState == MoveState.Crouching) cycle = 0;
        
        playerCamera.position = new Vector3(playerCamera.position.x, originalCameraY + Mathf.Sin(cycle) * bobMagnitude, playerCamera.position.z);
    }

    private void UpdateMovement() {
        // get input for direction
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);
        // get input for movestate
        if (Input.GetKey(KeyCode.LeftControl)) moveState = MoveState.Crouching;
        else if (Input.GetKey(KeyCode.LeftShift)) moveState = MoveState.Running;
        else moveState = MoveState.Walking;

        // gravity
        if(controller.isGrounded) velocity.y = 0.0f;
        velocity.y += gravity * Time.deltaTime;
        
        // get speed based on movestate
        float speed = moveState switch {
            MoveState.Crouching => crouchSpeed,
            MoveState.Walking => walkSpeed,
            MoveState.Running => runSpeed,
            _ => 0
        };

        // calculate velocity
        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * speed + Vector3.up * velocity.y;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkLoudness);
    }
}