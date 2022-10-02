using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    // state
    private bool selected;
    private bool interactable = true;

    // components
    private GameObject player;

    private void Awake() {
        player = GameObject.FindWithTag("Player");
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        InteractableManager.Instance.AddInteractable(this);
    }

    // Update is called once per frame
    void Update() {
        // add as candidate for selection
        if (interactable && GetDistanceToPlayer() <= InteractableManager.Instance.interactionRadius && GetAngleWithPlayer() <= InteractableManager.Instance.interactionAngle){
            InteractableManager.Instance.AddCandidate(this);
        }
        else InteractableManager.Instance.RemoveCandidate(this);
    }

    public virtual void Interact() {
        // Debug.Log("Interacting with " + gameObject.name);
    }

    public abstract string GetUIText();

    public float GetDistanceToPlayer() {
        return Vector3.Distance(player.transform.position, transform.position);
    }
    private float GetAngleWithPlayer() { 
        return Vector3.Angle(player.transform.forward, transform.position - player.transform.position);
    }

    protected void SetInteractable(bool value) {
        interactable = value;
        if (!interactable) {
            InteractableManager.Instance.RemoveCandidate(this);
        }
    }

    public bool IsInteractable() {
        return interactable; 
    }

    public void Select() {
        selected = true;
    }
    public void Deselect() {
        selected = false;
    }

    protected virtual void OnDestroy() {
        InteractableManager.Instance.RemoveInteractable(this);
    }
}
