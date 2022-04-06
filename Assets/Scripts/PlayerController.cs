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
    [Header("Movement")] 
    public float mouseSensitivity = 3.5f;
    public float mousePitchClamp = 90f;
    public float walkSpeed;
    public float bobAmount;
    public float bobMagnitude;
    // sounds
    [Header("Sounds")] 
    public float walkLoudness;

    // private state
    private float originalCameraY;
    private float cameraPitch;
    private Vector3 velocity;
    private float velocityY;
    private float cycle;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

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

    void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -mousePitchClamp, mousePitchClamp);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        
        // Bob head
        cycle += velocity.magnitude * bobAmount * Time.deltaTime;
        cycle %= 2 * Mathf.PI;
        if (currentDirVelocity.magnitude == 0) cycle = 0;
        Debug.Log("cycle: " + cycle);
        
        playerCamera.position = new Vector3(playerCamera.position.x, originalCameraY + Mathf.Sin(cycle) * bobMagnitude, playerCamera.position.z);
    }

    void UpdateMovement() {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if(controller.isGrounded) velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;
		
        velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkLoudness);
    }
}