using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CropCounter : MonoBehaviour
{
    public static CropCounter Instance;
    
    // components
    public TextMeshProUGUI counterText;
    
    // state
    private int cropsHarvested;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        counterText.text = "harvested 0 tomatoes";
    }

    public void CountCrop() {
        cropsHarvested++;

        counterText.text = "harvested " + cropsHarvested + " tomatoes";
    }

    public int GetCropsHarvested() {
        return cropsHarvested;
    }
}
