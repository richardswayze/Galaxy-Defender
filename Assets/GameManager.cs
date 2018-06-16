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
    public bool playerDead;
    public Animator camAnim;

    private EnemyParent enemyParent;
    private Text scoreDisplay;
    private Text debugDisplay;
    private Spawner[] spawners;
    private Text startButton;
    private Text gameOver;
    private Vector3 playerOrigin;
    private Text livesText;
    private AudioSource audioSource;
    private Text reset;

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
        playerDead = false;
        livesText = GameObject.Find("Lives").GetComponent<Text>();
        livesText.enabled = false;
        audioSource = GetComponent<AudioSource>();
        reset = GameObject.Find("Main Menu").GetComponent<Text>();
        reset.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        scoreDisplay.text = playerScore.ToString();
        debugDisplay.text = "ECount" + enemyParent.enemyCount.ToString() + " " +
                            "SRate" + spawners[1].spawnRate + " " +
                            "PlayTime" + spawners[1].playTime;
                            
        livesText.text = "Lives: " + lives.ToString();
	}

    public void Respawn()
    {
        Instantiate(playerPrefab, respawnLoc, Quaternion.identity);
        playerDead = false;
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
            thisSpawner.startTime = Time.time;
            thisSpawner.SpawnWave();
        }
    }

    public void GameOver()
    {
        gameStarted = false;
        gameOver.enabled = true;
        //CancelInvoke("StartWave");
        livesText.enabled = false;
        reset.enabled = true;
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
        gameStarted = true;
        startButton.enabled = false;
        lives = 3;
        playerScore = 0;
        Invoke("StartWave", startDelay);
        scoreDisplay.enabled = true;
        livesText.enabled = true;
        audioSource.Play();
    }

    public void Reset()
    {
        Instantiate(playerPrefab, playerOrigin, Quaternion.identity);
        startButton.enabled = true;
        scoreDisplay.enabled = false;
        reset.enabled = false;
        gameOver.enabled = false;
        camAnim.SetTrigger("MoveToIdle");
        GameObject[] enemies = enemyParent.GetComponentsInChildren<GameObject>();
        foreach (GameObject thisEnemy in enemies)
        {
            Destroy(thisEnemy.gameObject);
        }
    }
}
