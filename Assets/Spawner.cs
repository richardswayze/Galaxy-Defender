using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float spawnRate;
    public GameObject[] enemyPrefabs;
    
    private Transform[] spawners;
    private GameManager gameManager;
   
    // Use this for initialization
    void Start () {
        spawners = GameObject.Find("Spawners").GetComponentsInChildren<Transform>();
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();


        //InvokeRepeating("SpawnEndlessEnemies", .001f, spawnRate);
        
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
        int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomEnemy], transform.position, Quaternion.identity);
        float rateDecreaase = .25f;
        spawnRate = spawnRate - rateDecreaase;
        if (spawnRate <= .25f)
        {
            spawnRate = .25f;
        }
        Invoke("SpawnWave", spawnRate);
    }
        
}
