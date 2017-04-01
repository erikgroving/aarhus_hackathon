using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour {

    public BulletManager bulletManager;
    public float amountToLowerShield = 1f;
    public Text PointsText;
    public int Points;
    ShieldPosition currentShieldPosition;

	// Use this for initialization
	void Start () {
        if (bulletManager == null)
            Debug.Log("A bullet manager was not specified");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("1"))
            MoveShield(ShieldPosition.Down);
        if (Input.GetKeyDown("2"))
            MoveShield(ShieldPosition.Up);
    }

    public void Setup()
    {
        Points = 0;
        PointsText.text = "Points:" + Points.ToString("d4");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // A bullet hit the player
        if (collision.collider.tag == "Projectile")
        {
            bulletManager.DestroyBullet(collision.collider.gameObject, 1.0f);

            collision.collider.gameObject.GetComponent<Rigidbody>().useGravity = true;

            // TODO: Add score points?
            Points += 10;
            PointsText.text ="Points:"+ Points.ToString("d4");   


            // TODO: Impact audio

        }
    }

    public enum ShieldPosition { Up, Down }

    //TODO animate shield
    void MoveShield(ShieldPosition positionToUse)
    {
        switch(positionToUse)
        {
            case ShieldPosition.Down:
                if (currentShieldPosition !=  ShieldPosition.Down)
                {
                    gameObject.transform.Translate(0, -amountToLowerShield, 0);
                    currentShieldPosition = ShieldPosition.Down;
                }
                    
                break;
            case ShieldPosition.Up:
                if (currentShieldPosition != ShieldPosition.Up)
                {
                    gameObject.transform.Translate(0, amountToLowerShield, 0);
                    currentShieldPosition = ShieldPosition.Up;
                }
                break;
        }
    }

}
