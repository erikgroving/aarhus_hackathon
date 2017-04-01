using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public BulletManager bulletManager;

	// Use this for initialization
	void Start () {
        if (bulletManager == null)
            Debug.Log("A bullet manager was not specified");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        // A bullet hit the player
        if (collision.collider.tag == "Projectile")
        {
            bulletManager.DestroyBullet(collision.collider.gameObject);
            
            // TODO: Subtract score points?

            // TODO: Hit/Damage audio
            
        }
    }

}
