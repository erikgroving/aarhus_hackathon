using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public BulletManager bulletManager;
    public GameController gameController;
    public Text LivesText;
    public int _lives;

    // Use this for initialization
    void Start () {
        if (bulletManager == null)
            Debug.Log("A bullet manager was not specified");

        _lives = 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup()
    {
        resetup();
    }

    public void resetup()
    {
        _lives = 5;
        LivesText.text = "Lives: *****";
    }

    private void OnCollisionEnter(Collision collision)
    {
        // A bullet hit the player
        if (collision.collider.tag == "Projectile")
        {
            bulletManager.DestroyBullet(collision.collider.gameObject);

            _lives--;
            if (_lives < 0)
            {
                // TODO: Game over screen and stop game!
                gameController.GameOver();
            }

            else
            {
                LivesText.text = "Lives: ";
                for (int i = 0; i < _lives; i++)
                {
                    LivesText.text += "*";
                }
            }

            // TODO: Hit/Damage audio

        }
    }

    
}
