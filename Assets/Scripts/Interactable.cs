using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    // state
    private bool selected;
    private bool interactable = true;

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

        if (interactable && selected && Input.GetKeyDown(KeyCode.E)){
            Interact();
        }
    }

    protected virtual void Interact() {
        // Debug.Log("Interacting with " + gameObject.name);
    }

    public abstract string GetUIText();

    public float GetDistanceToPlayer() {
        return Vector3.Distance(PlayerController.Instance.transform.position, transform.position);
    }
    private float GetAngleWithPlayer() { 
        return Vector3.Angle(PlayerController.Instance.transform.forward, transform.position - PlayerController.Instance.transform.position);
    }

    protected void SetInteractable(bool value) {
        interactable = value;
        if (!interactable) {
            InteractableManager.Instance.RemoveCandidate(this);
        }
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
