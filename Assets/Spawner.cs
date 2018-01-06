using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] int spawnRate;

    public GameObject[] enemyPrefabs;

    private EnemyParent enemyParent;
    private Transform[] spawners;
    private int enemyCount;
    

    // Use this for initialization
    void Start () {
        enemyParent = GameObject.Find("Enemy Parent").GetComponent<EnemyParent>();
        spawners = GameObject.Find("Spawners").GetComponentsInChildren<Transform>();
        //InvokeRepeating("SpawnEnemies", .001f, spawnRate);
        
	}
	
	// Update is called once per frame
	void Update () {
        enemyCount = enemyParent.enemyCount;
    }

    public void SpawnEnemies()
    {
        int randomSpawner = Random.Range(1, spawners.Length);
        int randomEnemy = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randomEnemy], spawners[randomSpawner].transform.position, Quaternion.identity);
    }
        
}
