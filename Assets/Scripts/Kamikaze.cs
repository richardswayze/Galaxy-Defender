using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : MonoBehaviour {

    [SerializeField] float blastRadius;
    [SerializeField] int blastDamage;
    [SerializeField] int kamikazeDistance;

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
    private GameManager gameManager;

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
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, player.transform.position) < kamikazeDistance && !triggered)
        {
            triggered = true;
            playerLoc = player.transform.position;
        }
        if (!player && gameManager.playerDead == false)
        {
            player = GameObject.FindObjectOfType<Player>();
        }
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
            if (detonationRange <= rangeToDetonate)
            {
                Instantiate(enemy.prefabFX, transform.position, Quaternion.identity);
                if (Vector3.Distance(transform.position, player.transform.position) <= blastRadius)
                {
                    if (player.shield <= 0)
                    {
                        player.health -= blastDamage;
                    } else
                    {
                        player.shield -= blastDamage;
                    }  
                }
                Destroy(gameObject);
            }
        }
        if (gameManager.playerDead == true) 
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
            playerLoc = player.transform.position;
        }
    }

    void Scatter()
    {
        transform.position = Vector3.MoveTowards(transform.position, startingPos, step);
        float rotZ = Mathf.Atan2(startingPos.y - transform.position.y, startingPos.x - transform.position.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
