using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangeDirection : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    private Camera cam;

    private void Awake() {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 toTarget = cam.transform.position - transform.position;
        toTarget.Normalize();
        float dot = Vector3.Dot(toTarget, transform.forward);

        if (dot < -0.75) {
            spriteRenderer.sprite = backSprite;
        }
        else if (dot < 0.75) {
            float angle = Vector3.SignedAngle(toTarget, transform.forward, Vector3.up);
            spriteRenderer.sprite = angle < 0 ? leftSprite : rightSprite;
        }
        else {
            spriteRenderer.sprite = frontSprite;
        }
    }
}
