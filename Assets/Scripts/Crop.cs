using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Crop : Interactable {
    // components
    public SpriteRenderer spriteRenderer;
    
    // constants
    public Sprite waterSprite;
    public Sprite harvestSprite;
    public Sprite emptySprite;
    public enum CropState { Water, Ripening, Harvest, Empty }
    public CropState cropState;
    private float ripenTime = 8;
    
    // state
    private float ripenTimer;

    protected override void Start() {
        base.Start();
        spriteRenderer.sprite = cropState switch {
            CropState.Water => waterSprite,
            CropState.Harvest => harvestSprite,
            CropState.Empty => emptySprite,
            _ => emptySprite
        };
        if(cropState == CropState.Empty) SetInteractable(false);
    }

    protected override void Interact() {
        base.Interact();
        
        if (cropState == CropState.Water && !WaterUI.Instance.IsWaterEmpty()) Water();
        else if (cropState == CropState.Harvest) Harvest();
    }

    private void Water() {
        cropState = CropState.Ripening;
        StartCoroutine(Ripen());
        
        WaterUI.Instance.UseWater();

        AudioManager.Instance.PlayWaterSound();
    }

    private void Harvest() {
        CropCounter.Instance.CountCrop();
        MakeEmpty();
        
        AudioManager.Instance.PlayHarvestSound();
    }

    public void MakeEmpty() {
        // change state
        cropState = CropState.Empty;
        spriteRenderer.sprite = emptySprite;

        // make uninteractable
        SetInteractable(false);
    }

    private IEnumerator Ripen() {
        // count down timer
        ripenTimer = ripenTime;
        while (ripenTimer > 0) {
            ripenTimer -= Time.deltaTime;
            yield return null;
        }
        cropState = CropState.Harvest;
        spriteRenderer.sprite = harvestSprite;
    }

    public override string GetUIText() {
        if (cropState == CropState.Water) {
            return WaterUI.Instance.IsWaterEmpty() ? "out of water" : "E to water tomato";
        }
        if (cropState == CropState.Ripening) return "ready to harvest in " + ripenTimer.ToString("0") + "s";
        if (cropState == CropState.Harvest) return "E to harvest tomato";
        return "ERROR";
    }
}
