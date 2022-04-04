using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HarvestCrop : Interactable {
    public SpriteRenderer sprite;
    public Sprite emptySprite;

    protected override void Interact() {
        base.Interact();
        
        // add to count
        CropCounter.Instance.CountCrop();
        
        // change sprite
        sprite.sprite = emptySprite;

        // make uninteractable
        interactable = false;
    }

    public override string GetUIText() {
        return "harvest tomato";
    }
}
