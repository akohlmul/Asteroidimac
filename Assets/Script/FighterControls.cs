using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterControls : MonoBehaviour
{

	public Rigidbody2D rb;
	public float thrust;
	public float turnThrust;
	private float thrustInput;
	private float turnInput;
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    public float bulletForce;
    public float deathForce;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;

    public AudioSource audio;

    public GameObject explosion;
    public GameObject bullet;

    public Color inColor;
    public Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;

        scoreText.text = "Score " + score;
        livesText.text = "Lives " + lives;
    }

    // Update is called once per frame
    void Update()
    {
    	// Get input from the keyboard and apply thrust
    	//Check for input from the keyboard
    	thrustInput = Input.GetAxis("Vertical");
    	turnInput = Input.GetAxis("Horizontal");

        //Check for input from the fire key and make bullets
        if(Input.GetButtonDown("Fire1")) {
            GameObject newBullet = Instantiate(bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * bulletForce);
            Destroy(newBullet, 5.0f);
        }

        //rotate the ship
        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnThrust);

        //Screen wraping
        Vector2 newPos = transform.position;
        if(transform.position.y > screenTop) {
            newPos.y = screenBottom;
        }
        if(transform.position.y < screenBottom) {
            newPos.y = screenTop;
        }
        if(transform.position.x > screenRight) {
            newPos.x = screenLeft;
        }
        if(transform.position.x < screenLeft) {
            newPos.x = screenRight;
        }

        transform.position = newPos;

    }

    void FixedUpdate() 
    {
    	rb.AddRelativeForce(Vector2.up * thrustInput * thrust);
        //rb.AddTorque(-turnInput);
    }

    void ScorePoints(int pointsToAdd) {
        score += pointsToAdd;
        scoreText.text = "Score " + score;
    }

    void Respawn() {
        rb.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        sr.color = inColor;
        Invoke("Invunerable", 3f);
    }

    void Invunerable() {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().color = normalColor;
    }

    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log(col.relativeVelocity.magnitude);

        if(col.relativeVelocity.magnitude > deathForce) {
            lives--;
            //make explosion
            GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            livesText.text = "Lives " + lives;

            //Respawn New Life
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
            Invoke("Respawn", 3f);
            
            if(lives <= 0) {
                GameOver();
            }
        } else {
            audio.Play ();
        }
    }

    void GameOver() {
        CancelInvoke();
    }
}
