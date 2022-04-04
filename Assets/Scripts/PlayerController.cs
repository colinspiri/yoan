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
    
    // public constants
    public float mouseSensitivity = 3.5f;
    public float mousePitchClamp = 90f;
    public float walkSpeed = 6.0f;
    public float gravity = -13.0f;
    [Range(0.0f, 0.5f)] public float moveSmoothTime = 0.3f;
    [Range(0.0f, 0.5f)] public float mouseSmoothTime = 0.03f;

    // private state
    private float cameraPitch;
    private float velocityY;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
    }

    void Update() {
        if (Time.deltaTime != 0) {
            UpdateMouseLook();
            UpdateMovement();
        }
    }

    void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -mousePitchClamp, mousePitchClamp);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement() {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        if(controller.isGrounded) velocityY = 0.0f;

        velocityY += gravity * Time.deltaTime;
		
        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
    }
}