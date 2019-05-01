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
    public List<EnemyTable> enemytable = new List<EnemyTable>();

    private EnemyParent enemyParent;

    [SerializeField] Transform[] spawnLocations;
    [SerializeField] float spawnRateReducer;
    [SerializeField] float minSpawnRate;
    private void Awake()
    {
        enemyParent = GameObject.FindObjectOfType<EnemyParent>();
    }

    Transform ChooseSpawnLocation()
    {
        int index = Random.Range(0, spawnLocations.Length);
        return spawnLocations[index];
    }

    GameObject ChooseEnemy()
    {
        int enemyLots = 0;
        for (int i = 0; i < enemytable.Count; i++)
        {
            enemyLots += enemytable[i].rarity;
        }
        int chosenLot = Random.Range(0, enemyLots);
        for (int i = 0; i < enemytable.Count; i++)
        {
            if (enemytable[i].rarity > chosenLot)
            {
                return enemytable[i].enemyPrefab;
            }
            else
            {
                chosenLot -= enemytable[i].rarity;
            }
        }
        Debug.LogError("Enemy Table failed to choose enemy. chosenLot = " + chosenLot + " enemyLots = " + enemyLots);
        return enemytable[enemytable.Count - 1].enemyPrefab;
    }

    public IEnumerator SpawnEnemy()
    {
        Vector3 location = ChooseSpawnLocation().position;
        Instantiate(ChooseEnemy(), location, Quaternion.identity, enemyParent.transform);
        if (spawnRate < minSpawnRate)
        {
            spawnRate = minSpawnRate;
        }
        else
        {
            spawnRate -= spawnRateReducer;
        }
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine("SpawnEnemy");
    }
}
