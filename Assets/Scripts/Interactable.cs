using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    // components
    private Outline outline;

    // state
    private bool selected;
    private bool interactable = true;

    protected virtual void Awake() {
        outline = GetComponent<Outline>();
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        InteractableManager.Instance.AddInteractable(this);

        if (outline != null) {
            outline.enabled = false;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 4.0f;
        }
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
        if(outline != null) outline.enabled = true;
    }
    public void Deselect() {
        selected = false;
        if(outline != null) outline.enabled = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, InteractableManager.Instance.interactionRadius);
    }

    protected virtual void OnDestroy() {
        InteractableManager.Instance.RemoveInteractable(this);
    }
}
