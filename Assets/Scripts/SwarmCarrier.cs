﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmCarrier : MonoBehaviour
{

    public Vector3 destination = new Vector3();
    public GameObject laser;
    public GameObject swarmPrefab;
    public float distanceToSpawn;
    public float spawnRate;
    public int spawnCount;
    public int maxSpawn;
    public Vector3 startingPos;

    private Player player;
    private Vector3 playerPos = new Vector3();
    private float step;
    private float minimumDistanceToPlayer;
    private Enemy enemy;
    private float fireRate;
    private float firingDistance;
    private float timeShot;
    private GameManager gameManager;
    private float lastSpawnTime;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        startingPos = transform.position;
        enemy = GetComponent<Enemy>();
        step = enemy.speed * Time.deltaTime;
        fireRate = enemy.fireRate;
        firingDistance = enemy.firingDistance;
        minimumDistanceToPlayer = enemy.minimumDistanceToPlayer;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player && gameManager.playerDead == false)
        {
            player = GameObject.FindObjectOfType<Player>();
        }
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

            if (Vector3.Distance(transform.position, playerPos) < distanceToSpawn && Time.time - lastSpawnTime > spawnRate && spawnCount < maxSpawn)
            {
                SpawnShip();
            }
            //Random firing at player
            if (Vector3.Distance(transform.position, playerPos) < firingDistance && Time.time - timeShot > fireRate)
            {
                Fire();
            }
        }
        if (gameManager.playerDead == true)
        {
            //Player is dead, returns enemies to spawning location until player respawns
            Scatter();
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

    void SpawnShip()
    {
        GameObject newSwarmShip = Instantiate(swarmPrefab, transform.position, Quaternion.identity);
        newSwarmShip.transform.parent = transform;
        lastSpawnTime = Time.time;
        spawnCount += 1;
    }
}
