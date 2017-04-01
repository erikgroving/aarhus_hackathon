using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    public GameObject bulletPrefab;
    List<Bullet> bullets = new List<Bullet>();
    public int maxBullets = 1;
    public float deleteAtDistance = 10;

    public GameObject enemyPrefab;
    List<Enemy> enemies = new List<Enemy>();
    int initialEnemies = 4;

    public GameObject player;

    bool spawnBulletsRandomInterval = true;

	// Use this for initialization
	void Start () {
        PopulateRandomEnemies(17f);
    }
	
	// Update is called once per frame
	void Update () {

        float randX = Random.Range(1f, 10f);
        float randZ = Random.Range(1f, 10f);

        if (spawnBulletsRandomInterval)
        {
            if (bullets.Count < maxBullets)
            {
                int rand_idx = Random.Range(0, enemies.Count);

                SpawnBulletAtPosition(enemies[rand_idx].transform.position);
            }
        }

        if (bullets.Count > 0)
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();

            foreach (var bullet in bullets)
            {
                float distanceToPlayer = Vector3.Distance(bullet.transform.position, player.transform.position);

                if (distanceToPlayer > deleteAtDistance)
                {
                    bulletsToRemove.Add(bullet);
                    DestroyImmediate(bullet.gameObject);
                }
            }

            foreach (var bulletToRemove in bulletsToRemove)
            {
                bullets.Remove(bulletToRemove);
            }
        }

    }

    void SpawnBulletAtPosition(Vector3 position)
    {
        GameObject goBullet = Instantiate(bulletPrefab, position, new Quaternion());
        Bullet bullet = goBullet.GetComponent<Bullet>();
        bullet.ShootBulletAtPlayer(20f, player);
        bullets.Add(bullet);
    }

    void PopulateRandomEnemies(float distanceToPlayer)
    {
        for (int i = 0; i < initialEnemies; i++)
        {
            float angle = Random.Range(0f, 360f);
            float randX = distanceToPlayer * Mathf.Cos(angle);
            float randZ = distanceToPlayer * Mathf.Sin(angle);

            GameObject goEnemy = Instantiate(enemyPrefab, new Vector3(randX, 1.5f, randZ), new Quaternion());
            Enemy enemy = goEnemy.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
    }

    public void DestroyBullet(GameObject bulletToDestroy)
    {
        Bullet bullet = bulletToDestroy.GetComponent<Bullet>();
        Destroy(bulletToDestroy);
        bullets.Remove(bullet);
    }

}
