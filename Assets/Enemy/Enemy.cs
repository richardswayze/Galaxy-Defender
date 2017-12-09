using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Player player;
    private EnemyStats enemy;
    private EnemyParent enemyParent;
    private Rigidbody rigidBody;
    float speed;
    float step;
    int minimumDistance;
    Vector3 playerPos = new Vector3();
    public Vector3 destination = new Vector3();

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
    }


    void Update()
    {
        playerPos = player.transform.position;
        float rotZ = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

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
    }

    Vector3 RandomPos()
    {
        Vector3 randomPos = new Vector3(Random.Range(-minimumDistance, minimumDistance + 1), Random.Range(-minimumDistance, minimumDistance + 1), 0f);
        return randomPos;
    }

    

}
