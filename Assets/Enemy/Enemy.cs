using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject prefabFX;

    public float speed;
    public int health;
    public int collisionDamage;
    public float firingDistance;
    public float fireRate;
    public int accuracy;
    public int scoreToAdd;
    public int minimumDistanceToPlayer;
    public Player player;

    private EnemyParent enemyParent;
    private GameManager gameManager;
    private bool inDistance;
    private float timeShot;

    // Use this for initialization
    void Start()
    {
        enemyParent = GameObject.FindObjectOfType<EnemyParent>();
        enemyParent.enemyCount++;
        if (!GetComponent<SwarmCraft>())
        {
            transform.parent = enemyParent.transform;
        }
        enemyParent.enemyCount++;
        player = GameObject.FindObjectOfType<Player>();
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }
    void Update()
    {
        if (health <= 0)
        {
            Instantiate(prefabFX, transform.position, Quaternion.identity);
            enemyParent.enemyCount--;
            gameManager.playerScore += scoreToAdd;
            if (!gameObject.GetComponent<SwarmCraft>())
            {
                gameManager.GenerateLoot(transform.position);
            }
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            health = -10;
        }
    }
}