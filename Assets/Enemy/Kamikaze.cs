using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour {

    private Enemy enemy;
    private bool triggered;
    private Player player;
    private Vector3 playerLoc;
    private float step;
    private float detonationRange;
    private Vector3 playerPos;
    private Vector3 destination;
    private float fireRate;
    private float firingDistance;
    private float minimumDistanceToPlayer;
    private Vector3 startingPos;
    private float timeShot;

    public float rangeToDetonate;
    public int ramSpeed;
    public GameObject laser;

    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
        triggered = false;
        fireRate = enemy.fireRate;
        firingDistance = enemy.firingDistance;
        minimumDistanceToPlayer = enemy.minimumDistanceToPlayer;
        step = enemy.speed * Time.deltaTime;
        startingPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (player && !triggered)
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
        }

        else if (triggered)
        {
            step = ramSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerLoc, step);
            detonationRange = Mathf.Abs(Vector3.Distance(transform.position, playerLoc));
            Debug.Log(detonationRange);
            if (detonationRange <= rangeToDetonate)
            {
                Instantiate(enemy.prefabFX, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
        else 
        {
            //Player is dead, returns enemies to spawning location until player respawns
            Scatter();
        }      
	}

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.GetComponent<LaserStats>())
        {
            triggered = true;
            player = GameObject.FindObjectOfType<Player>();
            playerLoc = player.transform.position;
        }
    }

    void Scatter()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingPos, step);
        float rotZ = Mathf.Atan2(startingPos.y - transform.position.y, startingPos.x - transform.position.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        InvokeRepeating("FindPlayer", 1, 2);
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

    void FindPlayer()
    {
        player = GameObject.FindObjectOfType<Player>();
        if (player)
        {
            CancelInvoke();
        }
    }

}
