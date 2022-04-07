using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    
    // sfx
    public AudioSource walkingSound;
    public AudioSource runningSound;
    public AudioSource waterSound;
    public AudioSource harvestSound;

    private void Awake() {
        Instance = this;
    }

    public void StopGameSound() {
        AudioListener.pause = true;
    }
    public void ResumeGameSound() {
        AudioListener.pause = false;
    }

    private void Update() {
        var state = PlayerController.Instance.GetMoveState;
        if (state == PlayerController.MoveState.Crouching || state == PlayerController.MoveState.Still) {
            walkingSound.Stop();
            runningSound.Stop();
        }
        else if (state == PlayerController.MoveState.Walking) {
            if (!walkingSound.isPlaying) walkingSound.Play();
            runningSound.Stop();
        }
        else if (state == PlayerController.MoveState.Running) {
            walkingSound.Stop();
            if(!runningSound.isPlaying) runningSound.Play();
        }
    }

    public void PlayWaterSound() {
        waterSound.Play();
    }
    public void PlayHarvestSound() {
        harvestSound.Play();
    }
}
