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
    public int lives;
    public GameObject playerPrefab;
    public Vector3 respawnLoc;
    public int playerScore;
    public int startDelay;
    public int respawnTime;
    public int dropChance;
    public bool gameStarted;

    private EnemyParent enemyParent;
    private Text scoreDisplay;
    private Text debugDisplay;
    private Spawner[] spawners;
    private Text startButton;
    private Text gameOver;
    private Vector3 playerOrigin;

	// Use this for initialization
	void Start () {
        playerOrigin = GameObject.FindObjectOfType<Player>().GetComponent<Transform>().position;
        enemyParent = GameObject.Find("Enemy Parent").GetComponent<EnemyParent>();
        scoreDisplay = GameObject.Find("Score").GetComponent<Text>();
        scoreDisplay.enabled = false;
        debugDisplay = GameObject.Find("Debug").GetComponent<Text>();
        startButton = GameObject.Find("Start Game").GetComponent<Text>();
        gameStarted = false; 
        gameOver = GameObject.Find("Game Over").GetComponent<Text>();
        gameOver.enabled = false;
        spawners = GameObject.Find("Spawners").GetComponentsInChildren<Spawner>();
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
        gameStarted = false;
        gameOver.enabled = true;
        CancelInvoke("StartWave");
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

    public void StartGame()
    {
        startButton.enabled = false;
        lives = 3;
        playerScore = 0;
        Invoke("StartWave", startDelay);
        scoreDisplay.enabled = true;
    }

    public void Reset()
    {
        Instantiate(playerPrefab, playerOrigin, Quaternion.identity);
        startButton.enabled = true;
        scoreDisplay.enabled = false;
        GameObject[] enemies = enemyParent.GetComponentsInChildren<GameObject>();
        foreach (GameObject thisEnemy in enemies)
        {
            Destroy(thisEnemy.gameObject);
        }
    }
}
