using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour {
    // components
    public static InteractableManager Instance;
    public InteractableUI interactableUI;

    // state
    private List<Interactable> allInteractables = new List<Interactable>();
    private List<Interactable> candidatesForInteraction = new List<Interactable>();
    private Interactable selectedObject;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        SelectObjectFromCandidates();
    }

    private void SelectObjectFromCandidates() {
        // no candidates
        if (candidatesForInteraction.Count == 0) {
            if(selectedObject != null) selectedObject.Deselect();
            selectedObject = null;
            interactableUI.ClearInteractableUI();
            return;
        }
        // 1 candidate
        if (candidatesForInteraction.Count == 1) {
            if(selectedObject != null) selectedObject.Deselect();
            selectedObject = candidatesForInteraction[0];
            selectedObject.Select();
            interactableUI.ShowSelectedObject(selectedObject);
            return;
        }
        // choose closest candidate
        float minDistance = float.MaxValue;
        Interactable closest = null;
        foreach (Interactable candidate in candidatesForInteraction) {
            if (candidate.GetDistanceToPlayer() < minDistance){
                minDistance = candidate.GetDistanceToPlayer();
                closest = candidate;
            }
        }
        if(selectedObject != null) selectedObject.Deselect();
        selectedObject = closest;
        selectedObject.Select();
        interactableUI.ShowSelectedObject(selectedObject);
    }

    public void AddCandidate(Interactable interactable) {
        if(!candidatesForInteraction.Contains(interactable)) candidatesForInteraction.Add(interactable);
    }
    public void RemoveCandidate(Interactable interactable) {
        if (candidatesForInteraction.Contains(interactable)) candidatesForInteraction.Remove(interactable);
    }

    public void AddInteractable(Interactable interactable) {
        allInteractables.Add(interactable);
    }
    public void RemoveInteractable(Interactable interactable) {
        allInteractables.Remove(interactable);
        if (candidatesForInteraction.Contains(interactable)) candidatesForInteraction.Remove(interactable);
    }
}
