using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [System.Serializable]
    public class EnemyTable
    {
        public string name;
        public GameObject enemyPrefab;
        public int rarity;
    }

    public float spawnRate;
    public GameObject[] enemyPrefabs;
    public List<EnemyTable> enemytable = new List<EnemyTable>();
    
    private EnemyParent enemyParent;
   
    // Use this for initialization
    void Start () {
        enemyParent = GameObject.FindObjectOfType<EnemyParent>().GetComponent<EnemyParent>();

        //InvokeRepeating("SpawnEndlessEnemies", .001f, spawnRate);
        
	}

    void SpawnEnemy()
    {
        int spawnWeight = 0;
        for (int i = 0; i < enemytable.Count; i++)
        {
            spawnWeight += enemytable[i].rarity;
        }
        int spawnLot = Random.Range(0, spawnWeight + 1);
        for (int i = 0; i < enemytable.Count; i++)
        {
            if (spawnLot <= enemytable[i].rarity)
            {
                Instantiate(enemytable[i].enemyPrefab, transform.position, Quaternion.identity);
                return;
            }
        }
    }
 	
	// Update is called once per frame
	void Update () {

    }

    // Enemies spawn infinitely
    public void SpawnEndlessEnemies()
    {
        int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomEnemy], transform.position, Quaternion.identity);
    }

    public void SpawnWave()
    {
        SpawnEnemy();

        //int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        //Instantiate(enemyPrefabs[randomEnemy], transform.position, Quaternion.identity);

        //Spawn rate determined by score
        //if (spawnRate >= 1)
        //{
        //    float rateDecrease = .0001f * gameManager.playerScore;
        //    spawnRate = spawnRate - rateDecrease;
        //} else
        //{
        //    spawnRate = .5f;
        //}


        //Spawn rate decreses by .25 seconds every wave
        if (spawnRate >= 4.25f)
        {
            float rateDecreaase = .25f;
            spawnRate = spawnRate - rateDecreaase;
        }

        if (enemyParent.enemyCount > 70)
        {
            Invoke("SpawnWave", spawnRate + 20);
        } else
        {
            Invoke("SpawnWave", spawnRate);
        }
    }       
}
