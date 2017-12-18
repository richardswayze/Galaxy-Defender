using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject laser;

    private Player player;
    private EnemyStats enemy;
    private EnemyParent enemyParent;
    private Rigidbody rigidBody;
    float speed;
    float step;
    float firingDistance;
    int minimumDistance;
    Vector3 playerPos = new Vector3();
    public Vector3 destination = new Vector3();
    float distanceToPlayer;
    float fireRate;
    bool inDistance;
    float timeShot;
    int accuracy;
    Vector3 startingPos;

    // Use this for initialization
    void Start () {
        enemyParent = GameObject.FindObjectOfType<EnemyParent>();
        enemy = GetComponent<EnemyStats>();
        transform.parent = enemyParent.transform;
        enemyParent.enemyCount++;
        rigidBody = GetComponent<Rigidbody>();
        speed = enemy.speed;
        player = GameObject.FindObjectOfType<Player>();
        step = speed * Time.deltaTime;
        minimumDistance = enemy.minimumDistanceToPlayer;
        destination = RandomPos() + playerPos;
        firingDistance = enemy.firingDistance;
        fireRate = enemy.fireRate;
        startingPos = transform.position;
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
         
    }

    Vector3 RandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(-minimumDistance, minimumDistance + 1), Random.Range(-minimumDistance, minimumDistance + 1), 0f);
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

}
