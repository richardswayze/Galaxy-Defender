using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

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
    public float startDelay;
    public int respawnTime;
    public int dropChance;
    public bool gameStarted;
    public bool playerDead;

    private EnemyParent enemyParent;
    private Spawner spawner;
    private Vector3 playerOrigin;
    
    [SerializeField] CinemachineVirtualCamera gameCamera;
    [SerializeField] Text scoreDisplay;
    [SerializeField] Text gameOver;
    [SerializeField] Text livesText;
    [SerializeField] Text restart;
    [SerializeField] Text mainMenu;
    [SerializeField] Text finalScore;

    // Use this for initialization
    void Start () {
        gameStarted = false;
        playerDead = false;
        playerOrigin = GameObject.FindObjectOfType<Player>().GetComponent<Transform>().position;
        enemyParent = GameObject.Find("Enemy Parent").GetComponent<EnemyParent>();
        spawner = FindObjectOfType<Spawner>();
        gameOver.enabled = false;
        restart.enabled = false;
        mainMenu.enabled = false;
        finalScore.enabled = false;
        StartGame();
    }
	
	// Update is called once per frame
	void Update () {
        scoreDisplay.text = playerScore.ToString();
                            
        livesText.text = "Lives: " + lives.ToString();
	}

    public void StartGame()
    {
        gameStarted = true;
        lives = 3;
        playerScore = 0;
        scoreDisplay.enabled = true;
        livesText.enabled = true;
        spawner.StartCoroutine("SpawnEnemy");
    }

    public void Respawn()
    {
        Instantiate(playerPrefab, respawnLoc, Quaternion.identity);
        gameCamera.m_Follow = GameObject.FindObjectOfType<Player>().transform;
        playerDead = false;
        Enemy[] enemyChildren = enemyParent.GetComponentsInChildren<Enemy>();
        foreach (Enemy thisEnemy in enemyChildren)
        {
            thisEnemy.player = GameObject.FindObjectOfType<Player>();
        }
    }

    public void GameOver()
    {
        scoreDisplay.enabled = false;
        gameStarted = false;
        gameOver.enabled = true;
        finalScore.enabled = true;
        finalScore.text = "Final Score: " + playerScore;
        livesText.enabled = false;
        restart.enabled = true;
        mainMenu.enabled = true;
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

    public void Reset()
    {
        Instantiate(playerPrefab, playerOrigin, Quaternion.identity);
        scoreDisplay.enabled = false;
        restart.enabled = false;
        gameOver.enabled = false;
        GameObject[] enemies = enemyParent.GetComponentsInChildren<GameObject>();
        foreach (GameObject thisEnemy in enemies)
        {
            Destroy(thisEnemy.gameObject);
        }
    }
}
