using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour {

    Rigidbody ownedRigidbody;
    public GameObject player;
    public float deleteAtDistance = 20f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void ShootBulletAtPlayer(float speed)
    {
        var x = transform.position.x - player.transform.position.x;
        var z = transform.position.z - player.transform.position.z;

        var directionToMove = new Vector3(-x * speed, 0f, -z * speed);
        ownedRigidbody.AddForce(directionToMove);
    }

    public void ShootBulletAtPlayer(float speed, GameObject playerToUse)
    {
        ownedRigidbody = gameObject.GetComponent<Rigidbody>();

        var x = transform.position.x - playerToUse.transform.position.x;
        var z = transform.position.z - playerToUse.transform.position.z;

        var directionToMove = new Vector3((-x * speed), 0f, (-z * speed));

        if (ownedRigidbody == null)
        {
            Debug.Log("Rigidbody is null");
            return;
        }

        ownedRigidbody.AddForce(directionToMove);
    }
}
