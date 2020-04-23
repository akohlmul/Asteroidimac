using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{

	public float maxThrust;
	public float maxTorque;
	public Rigidbody2D rb;
	public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    public int asteroidSize; //3 = Large, 2 = Medium, 1 = Small
    public GameObject asteroidMedium;
    public GameObject asteroidSmall;
    public int points;
    public GameObject player;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
    	//Add a random amount of torque and thrust to the asteroid
    	Vector2 thrust = new Vector2(Random.Range(-maxThrust, maxThrust),Random.Range(-maxThrust, maxThrust));
    	float torque = Random.Range(-maxTorque, maxTorque);

    	rb.AddForce(thrust);
    	rb.AddTorque(torque);

        // find the player
        player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
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

    void OnTriggerEnter2D(Collider2D other) {
        // check to see if its a bullet
        if(other.CompareTag("bullet")) {
            //destroy the bullet
            Destroy(other.gameObject);
            //check the size of the asteroid and spawn in the next smaller size
            if(asteroidSize == 3) {
                //Spawn two medium asteroids
                Instantiate(asteroidMedium, transform.position, transform.rotation);
                Instantiate(asteroidMedium, transform.position, transform.rotation);
            }
            else if(asteroidSize == 2) {
                //Spawn two small asteroids
                Instantiate(asteroidSmall, transform.position, transform.rotation);
                Instantiate(asteroidSmall, transform.position, transform.rotation);
            }
            else if(asteroidSize == 1) {
                //Remove the asteroid
            }

            // tell the player to score come points
            player.SendMessage("ScorePoints", points);

            // make an explosion
            GameObject newExplosion = Instantiate(explosion,transform.position,transform.rotation);
            Destroy(newExplosion, 3f);

            // remove the current asteroid
            Destroy(gameObject);
        }
    }
}
