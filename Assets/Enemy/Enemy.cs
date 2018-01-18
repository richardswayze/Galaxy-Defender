using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject laser;
    public GameObject prefabFX;

    public float speed;
    public int minimumDistanceToPlayer;
    public int health;
    public int collisionDamage;
    public float firingDistance;
    public float fireRate;
    public int accuracy;
    public Player player;
    public int scoreToAdd;
    public GameObject[] itemPool;

    private EnemyParent enemyParent;
    private GameManager gameManager;

    float step;
    int minimumDistance;
    Vector3 playerPos = new Vector3();
    public Vector3 destination = new Vector3();
    float distanceToPlayer;
    bool inDistance;
    float timeShot;
    Vector3 startingPos;

    // Use this for initialization
    void Start () {
        enemyParent = GameObject.FindObjectOfType<EnemyParent>();
        enemyParent.enemyCount++;
        transform.parent = enemyParent.transform;
        enemyParent.enemyCount++;
        player = GameObject.FindObjectOfType<Player>();
        step = speed * Time.deltaTime;
        destination = RandomPos() + playerPos;
        startingPos = transform.position;
        gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }


    void Update()
    {
        if (player)
        {
            playerPos = player.transform.position;

            //Rotate enemy to face player
            float rotZ = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            //Receive new random destination when arrive at current destination
            if (transform.position == destination)
            {
                destination = RandomPos() + playerPos;
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
            }
            else
            {
                //MoveToPlayer
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
            }

            //Random firing at player
            if (Vector3.Distance(transform.position, playerPos) < firingDistance && Time.time - timeShot > fireRate)
            {
                Fire();
            }
        } else
        {
            //Player is dead, returns enemies to spawning location until player respawns
            Scatter();
        }

        if (health <= 0)
        {
            Instantiate(prefabFX, transform.position, Quaternion.identity);
            enemyParent.enemyCount--;
            gameManager.playerScore += scoreToAdd;
            gameManager.GenerateLoot(transform.position);
            Destroy(gameObject);
        }

    }

    Vector3 RandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(-minimumDistanceToPlayer, minimumDistanceToPlayer + 1), Random.Range(-minimumDistanceToPlayer, minimumDistanceToPlayer + 1), 0f);
        return randomPos;
    }

    void Fire()
    {
        timeShot = Time.time;
        Instantiate(laser, transform.position, Quaternion.identity);
    }
    
    void Scatter()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingPos, step);
        float rotZ = Mathf.Atan2(startingPos.y - transform.position.y, startingPos.x - transform.position.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Player>())
        {
            health = -1;
        }

    }
}
