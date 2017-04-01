using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewGameButton : MonoBehaviour {

    public GameController gameController;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!gameController.GameRunning)
        {
            WaitAndStartGame(2f);
            gameController.StartGame();
            Debug.Log("Game started");
        }
    }

    IEnumerator WaitAndStartGame(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}
