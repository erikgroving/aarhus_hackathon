using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int Health { get; set; }
    public Vector2 Position { get; set; }
    public bool isActive = true;

    // Use this for initialization
    void Start () {
        Health = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void DecreaseHealth(int amountToDecrease)
    {
        Health -= amountToDecrease;

        if (Health <= 0)
        {
            isActive = false;
        }
    }
}
