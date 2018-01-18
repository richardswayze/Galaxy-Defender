using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public class LootTable
    {
        public string name;
        public GameObject item;
        public int dropRarity;
    }

    public List<LootTable> lootTable = new List<LootTable>();
    public int lives = 3;
    public GameObject playerPrefab;
    public Vector3 respawnLoc;
    public int playerScore = 0;
    public int startDelay;
    public int respawnTime;
    public int dropChance;

    private EnemyParent enemyParent;
    private Text scoreDisplay;
    private Text debugDisplay;
    private Spawner[] spawners;

	// Use this for initialization
	void Start () {
        enemyParent = GameObject.Find("Enemy Parent").GetComponent<EnemyParent>();
        scoreDisplay = GameObject.Find("Score").GetComponent<Text>();
        debugDisplay = GameObject.Find("Debug").GetComponent<Text>();
        spawners = GameObject.Find("Spawners").GetComponentsInChildren<Spawner>();
        Invoke("StartWave", startDelay);
    }
	
	// Update is called once per frame
	void Update () {
        scoreDisplay.text = playerScore.ToString();
        debugDisplay.text = "ECount" + enemyParent.enemyCount.ToString() + " " +
                            "SRate" + spawners[1].spawnRate;
	}

    public void Respawn()
    {
        Instantiate(playerPrefab, respawnLoc, Quaternion.identity);
        Enemy[] enemyChildren = enemyParent.GetComponentsInChildren<Enemy>();
        foreach (Enemy thisEnemy in enemyChildren)
        {
            thisEnemy.player = GameObject.FindObjectOfType<Player>();
        }
    }

    public void StartWave()
    {
        foreach(Spawner thisSpawner in spawners)
        {
            thisSpawner.SpawnWave();
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    public void GenerateLoot(Vector3 location)
    {
        int dropCheck = Random.Range(0, 101);
        int dropWeight = 0;
        if (dropCheck < dropChance)
        {
            for (int i = 0; i < lootTable.Count; i++)
            {
                dropWeight += lootTable[i].dropRarity;
            }
        } else
        {
            return;
        }
        int calculateItem = Random.Range(0, dropWeight + 1);
        for (int i = 0; i < lootTable.Count; i++)
        {
            if (calculateItem <= lootTable[i].dropRarity)
            {
                Instantiate(lootTable[i].item, location, Quaternion.identity);
                return;
            }
        }
    }
}
