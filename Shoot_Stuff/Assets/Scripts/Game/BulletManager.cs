using System;
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
        
    }
	
	// Update is called once per frame
	void Update () {

        float randX = UnityEngine.Random.Range(1f, 10f);
        float randZ = UnityEngine.Random.Range(1f, 10f);

        if (spawnBulletsRandomInterval)
        {
            if (bullets.Count < maxBullets)
            {
                int rand_idx = UnityEngine.Random.Range(0, enemies.Count);

                if (enemies.Count > 0)
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

        if (enemies.Count > 0)
        {
            List<Enemy> enemiesToRemove = new List<Enemy>();

            foreach (var enemy in enemies)
            {
                if (!enemy.isActive)
                {
                    enemiesToRemove.Add(enemy);
                    Destroy(enemy.gameObject);
                }
            }

            foreach (var enemyToRemove in enemiesToRemove)
            {
                enemies.Remove(enemyToRemove);
            }
        }

    }

    internal void DeleteAll()
    {
        foreach (var item in enemies)
        {
            Destroy(item.gameObject);


        }
        enemies.Clear();

    }

    void SpawnBulletAtPosition(Vector3 position)
    {
        GameObject goBullet = Instantiate(bulletPrefab, position, new Quaternion());
        Bullet bullet = goBullet.GetComponent<Bullet>();
        bullet.ShootBulletAtPlayer(20f, player);
        bullets.Add(bullet);
    }

    public void PopulateRandomEnemies(float distanceToPlayer)
    {
        bool[] initialEnemie = new bool[18];

        
        
        for (int i = 0; i < initialEnemies; i++)
        {
            float angle;
            do
            {
                angle = UnityEngine.Random.Range(0f, 360f);
            } while (!EnemieIsgod(initialEnemie,angle));
            Debug.Log((int)angle/20);



            float randX = distanceToPlayer * Mathf.Cos(angle);
            float randZ = distanceToPlayer * Mathf.Sin(angle);
            
           
            
            GameObject goEnemy = Instantiate(enemyPrefab, new Vector3(randX, 1.5f, randZ), new Quaternion());
            Enemy enemy = goEnemy.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
    }

    private bool EnemieIsgod(bool[] initialEnemie, float angle )
    {
        
        int binID = (int)angle / 20;
        int binlow = binID == 0 ? 17 : binID - 1;
        int binhigh = binID == 17 ? 0 : binID + 1;
        if (initialEnemie[binID] || initialEnemie[binlow] || initialEnemie[binhigh])
        {
            return false;
        }
        initialEnemie[binID] = true;
        return true;
    }

    public void DestroyBullet(GameObject bulletToDestroy)
    {
        Bullet bullet = bulletToDestroy.GetComponent<Bullet>();
        Destroy(bulletToDestroy);
        bullets.Remove(bullet);
    }

    public void DestroyBullet(GameObject bulletToDestroy, float timeToWait)
    {
        Bullet bullet = bulletToDestroy.GetComponent<Bullet>();
        Destroy(bulletToDestroy, timeToWait);
        bullets.Remove(bullet);
    }

}
