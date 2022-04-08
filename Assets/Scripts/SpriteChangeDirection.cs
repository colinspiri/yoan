using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangeDirection : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;

    // Update is called once per frame
    void Update()
    {
        // TODO: FIX. NEED TO FACTOR IN TRANSFORM.FORWARD INstead of just being the vector from player to target
        // if player forward and object forward are facing each other, show front
        // if theyre in opposite directions, show back
        // maybe use dot product
        Vector3 vectorToTarget = Camera.main.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
        float angleAbs = Mathf.Abs(angle);

        if (angleAbs <= 75) {
            spriteRenderer.sprite = frontSprite;
        }
        else if (angleAbs <= 105) {
            if (angle < 0) spriteRenderer.sprite = rightSprite;
            else spriteRenderer.sprite = leftSprite;
        }
        else {
            spriteRenderer.sprite = backSprite;
        }
    }
}
