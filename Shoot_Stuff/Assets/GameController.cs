using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public BulletManager bulletManager;
    public bool GameRunning { get; set; }
    public PlayerController player;
    public ShieldController shield;

    // Use this for initialization
    void Start () {
        GameRunning = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame()
    {
        if (!GameRunning)
        {
            GameRunning = true;
            bulletManager.PopulateRandomEnemies(17f);
            player.Setup();
            shield.Setup();
        }
    }

    public void GameOver()
    {
        bulletManager.DeleteAll();
        GameRunning = false; 
       // throw new UnityException("Not implemented");
    }
}
