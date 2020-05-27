using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Color color1; //00FFDA
    public Color color2;
    public Color color3;
    public Color color4;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        spriteRenderer.color = randomColor();
        
    }

    Color randomColor() {
        float random = Random.Range(0f,20);
        if(random < 2) {
            return color1;
        } 
        if(random < 12) {
            return color2;
        }
        if(random < 18) {
            return color3;
        } else {
            return color4;
        }
    }
}
