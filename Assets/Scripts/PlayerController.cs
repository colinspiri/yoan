using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

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
    public float crouchHeight;
    [Header("Movement")] 
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;
    [Header("Sounds")] 
    public float walkLoudness;
    public float runLoudness;

    // private state
    // camera
    private float normalHeight;
    private float cameraPitch;
    private float cycle;
    private Sequence walkingBobSequence;
    private Sequence runningBobSequence;
    // movement
    public enum MoveState { Still, Walking, Running, Crouching };
    private MoveState moveState = MoveState.Still;
    public MoveState GetMoveState => moveState;
    private Vector3 velocity;
    private Vector2 currentDir;
    private Vector2 currentDirVelocity;
    private Vector2 currentMouseDelta;
    private Vector2 currentMouseDeltaVelocity;
    // cover
    private bool inCover;
    public bool InCover => inCover;

    private void Awake()
    {
        Instance = this;
        controller = GetComponent<CharacterController>();
    }

    private void Start() {
        normalHeight = playerCamera.transform.position.y;
        ChangeMoveState(MoveState.Still);

        walkingBobSequence = DOTween.Sequence();
        walkingBobSequence.Append(playerCamera.DOMoveY(normalHeight, bobFrequency));
        walkingBobSequence.Append(playerCamera.DOMoveY(normalHeight + bobMagnitude, bobFrequency));
        walkingBobSequence.Append(playerCamera.DOMoveY(normalHeight, bobFrequency));
        walkingBobSequence.Append(playerCamera.DOMoveY(normalHeight - bobMagnitude, bobFrequency));
        walkingBobSequence.SetEase(Ease.InOutSine);
        walkingBobSequence.SetLoops(-1);
        walkingBobSequence.Pause();

        float runningBobFrequency = bobFrequency / (runSpeed / walkSpeed);
        runningBobSequence = DOTween.Sequence();
        runningBobSequence.Append(playerCamera.DOMoveY(normalHeight, runningBobFrequency));
        runningBobSequence.Append(playerCamera.DOMoveY(normalHeight + bobMagnitude, runningBobFrequency));
        runningBobSequence.Append(playerCamera.DOMoveY(normalHeight, runningBobFrequency));
        runningBobSequence.Append(playerCamera.DOMoveY(normalHeight - bobMagnitude, runningBobFrequency));
        runningBobSequence.SetEase(Ease.InOutSine);
        runningBobSequence.SetLoops(-1);
        runningBobSequence.Pause();
    }

    public void EnterCover() {
        inCover = true;
    }
    public void LeaveCover() {
        inCover = false;
    }
    
    void Update() {
        if (Time.deltaTime == 0) return;
        
        UpdateMouseLook();
        
        // get move input 
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        // change moveState based on move input
        bool moving = targetDir.magnitude > 0.01f;
        if (Input.GetKey(KeyCode.LeftControl) || inCover) {
            if(moveState != MoveState.Crouching) ChangeMoveState(MoveState.Crouching);
        }
        else if (moving && Input.GetKey(KeyCode.LeftShift)) {
            if(moveState != MoveState.Running) ChangeMoveState(MoveState.Running);
        }
        else if(moving) {
            if(moveState != MoveState.Walking) ChangeMoveState(MoveState.Walking);
        }
        else {
            if(moveState != MoveState.Still) ChangeMoveState(MoveState.Still);
        }
        
        // report sound
        if(moveState == MoveState.Running) TorbalanSenses.Instance.ReportSound(transform.position, runLoudness); // TODO: only report every half second instead of every frame
        else if(moveState == MoveState.Walking) TorbalanSenses.Instance.ReportSound(transform.position, walkLoudness);

        // move player
        UpdateMovement();
    }

    private void UpdateMouseLook() {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -mousePitchClamp, mousePitchClamp);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    private void UpdateMovement() {
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

    private void ChangeMoveState(MoveState newMoveState) {
        // Debug.Log("calling ChangeMoveState(" + newMoveState + ") with old moveState = " + moveState);
        // clean up old move state
        if (moveState == MoveState.Walking) {
            walkingBobSequence.Pause();
        }
        else if (moveState == MoveState.Running) {
            runningBobSequence.Pause();
        }

        // initialize new move state
        if (newMoveState == MoveState.Still) {
            playerCamera.DOMoveY(normalHeight, 0.5f);
        }
        else if (newMoveState == MoveState.Crouching) {
            playerCamera.DOMoveY(crouchHeight, 0.5f);
        }
        else if (newMoveState == MoveState.Walking) {
            walkingBobSequence.Restart();
            walkingBobSequence.Play();
        }
        else if (newMoveState == MoveState.Running) {
            walkingBobSequence.Restart();
            runningBobSequence.Play();
        }
        
        // set new move state
        moveState = newMoveState;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkLoudness);
        Gizmos.DrawWireSphere(transform.position, runLoudness);
    }
}