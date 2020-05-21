using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FighterControls : MonoBehaviour
{

	public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Collider2D collider;
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
    private bool hyperspace; // true = currently hyperspacing
    public AlienScript alien;

    public int score;
    public int lives;

    public Text scoreText;
    public Text livesText;
    public GameObject gameOverPanel;
    public GameObject newHighScorePanel;
    public InputField highScoreInput;
    public Text highScoreListText;
    public GameManager gm;

    public AudioSource audio;

    public GameObject explosion;
    public GameObject bullet;

    public Color inColor;
    public Color normalColor;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        hyperspace = false;
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

        //Check for hyperspace
        if(Input.GetButtonDown("Hyperspace") && !hyperspace) {
            hyperspace = true;
            //turn off colliders and spriteRender
            spriteRenderer.enabled = false;
            collider.enabled = false;
            Invoke("Hyperspace",1f);
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
        spriteRenderer.enabled = true;
        spriteRenderer.color = inColor;
        Invoke("Invunerable", 3f);
    }

    void Invunerable() {
        collider.enabled = true;
        spriteRenderer.color = normalColor;
    }

    void Hyperspace() {
        //Move to a new random position
        Vector2 newPosition = new Vector2(Random.Range(-3.5f, 3.5f),Random.Range(-2.5f, 2.5f));
        transform.position = newPosition;
        //turn on colliders and spriteRender
        spriteRenderer.enabled = true;
        collider.enabled = true;

        hyperspace = false;
    }

    void LoseLife() {
        lives--;
        //Make explosion
        GameObject newExplosion = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(newExplosion, 3f);
        livesText.text = "Lives " + lives;

        //Respawn New Life
        spriteRenderer.enabled = false;
        collider.enabled = false;
         Invoke("Respawn", 3f);
            
        if(lives <= 0) {
            GameOver();
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log(col.relativeVelocity.magnitude);

        if(col.relativeVelocity.magnitude > deathForce) {
            LoseLife();
        } else {
            audio.Play ();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Beam")) {
            LoseLife();
            alien.Disable();
        }
    }

    void GameOver() {
        CancelInvoke();
        //Tell the game manager to check for high score
        if(gm.CheckForHighScore(score)) {
            newHighScorePanel.SetActive(true);
        } else {
            gameOverPanel.SetActive(true);
            highScoreListText.text = "HIGH SCORES" + "\n" + PlayerPrefs.GetString("highscoreName") + " " + PlayerPrefs.GetInt("highscore");
        }
    }

    public void HighScoreInput() {
        string newInput = highScoreInput.text;
        Debug.Log(newInput);
        newHighScorePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        PlayerPrefs.SetString("highscoreName", newInput);
        PlayerPrefs.SetInt("highscore", score);
        highScoreListText.text = "HIGH SCORES" + "\n" + PlayerPrefs.GetString("highscoreName") + " " + PlayerPrefs.GetInt("highscore");
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("Start Menu");
    }

    public void PlayAgain() {
        SceneManager.LoadScene("Main");
    }
}
