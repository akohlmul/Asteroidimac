﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour {

    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public float shootingDelay; //time between shots in seconds
    public float lastTimeShot = 0f;
    public float bulletSpeed;
    public Transform player;
    public GameObject bullet;
    public GameObject explosion;
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
    public bool disabled;   //true chen currently disabled
    public int points;
    public float timeBeforeSpawning;
    public Transform startPosition;
    public int currentLevel = 0;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("Player").transform;
        NewLevel();
    }

    // Update is called once per frame
    void Update() {
        if(disabled) {
            return;
        }

        if(Time.time > lastTimeShot + shootingDelay) {
            //Shoot
            float angle = Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            //Make a bullet
            GameObject newBullet = Instantiate(bullet, transform.position, q);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f,bulletSpeed));
            lastTimeShot = Time.time;
        }
    }

    void FixedUpdate() {
        if(disabled) {
            return;
        }

        //Figure out which way to move to approach the player
        direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void NewLevel() {
        Disable();
        currentLevel++;
        timeBeforeSpawning = Random.Range(5f,20f);
        Invoke("Enable", timeBeforeSpawning);
        speed = currentLevel / 10f;
        bulletSpeed = 45 + currentLevel;
        points = 150 * currentLevel;
    }

    void Enable() {
        //Move to start position
        transform.position = startPosition.position;
        //Turn on collider and sprite renderer
        collider.enabled = true;
        spriteRenderer.enabled = true;
        disabled = false;
    }

    public void Disable() {
        //Turn off colliders and sprite renderer
        collider.enabled = false;
        spriteRenderer.enabled = false;
        disabled = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("bullet")) {
            // tell the player to score come points
            player.SendMessage("ScorePoints", points);

            //Destroy the alien
            //Explosion
            GameObject newExplosion = Instantiate(explosion,transform.position,transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.transform.CompareTag("Player")) {
            GameObject newExplosion = Instantiate(explosion,transform.position,transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
        }
    }
}
