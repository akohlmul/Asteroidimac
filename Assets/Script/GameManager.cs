using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int numberOfAsteroids; //This is the current number of asteroids in the scene
    public int levelNumber = 1;
    public Text levelText;
    public GameObject asteroid;
    public AlienScript alien;
    
    public void UpdateNumberOfAsteroids(int change) {
        numberOfAsteroids += change;

        //Check to see if we have any asteroids left
        if(numberOfAsteroids <= 0) {
            //Start new level
            Invoke("StartNewLevel",3f);

        }
    }

    void StartNewLevel() {
        levelNumber++;
        levelText.text = "Level " + levelNumber;

        //Spawn new asteroids
        for(int i = 0; i < levelNumber*2; i++) {
            Vector2 spawnPosition = new Vector2(Random.Range(-4.45f,4.45f), 3.35f);
            Instantiate(asteroid, spawnPosition, Quaternion.identity);
            numberOfAsteroids++;
        }

        //Setup the Alien
        alien.NewLevel();
    }

    public bool CheckForHighScore(int score) {
        int highScore = PlayerPrefs.GetInt("highscore");
        if(score > highScore) {
            return true;
        }

        return false;
    }

}
