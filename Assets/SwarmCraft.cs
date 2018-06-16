using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmCraft : MonoBehaviour {

    public Vector3 destination = new Vector3();
    public int exitRange;

    private Player player;
    private Vector3 playerPos = new Vector3();
    private float step;
    private Enemy enemy;
    private GameManager gameManager;
    private Vector3 startingPos;
    private Vector3 retreatPos;
    private bool initialVectorReached;
    private SwarmCarrier swarmCarrier;

    // Use this for initialization
    void Start () {
        player = GameObject.FindObjectOfType<Player>();
        enemy = GetComponent<Enemy>();
        step = enemy.speed * Time.deltaTime;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        startingPos = transform.position;
        initialVectorReached = false;
        swarmCarrier = GetComponentInParent<SwarmCarrier>();
        retreatPos = swarmCarrier.startingPos;
    }
	
	// Update is called once per frame
	void Update () {
        if (!initialVectorReached)
        {
            destination = RandomStartPos() + transform.position;
            //Rotate enemy to face player
            float rotZ = Mathf.Atan2(destination.y - transform.position.y, destination.x - transform.position.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
            
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            if (Vector3.Distance(destination, transform.position) <= 1)
            {
                initialVectorReached = true;
            }
        }
        if (!player && gameManager.playerDead == false)
        {
            player = GameObject.FindObjectOfType<Player>();
        }
        if (gameManager.playerDead == false && initialVectorReached)
        {
            playerPos = player.transform.position;
            //Rotate enemy to face player
            float rotZ = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);

            destination = playerPos;
            transform.position = Vector3.MoveTowards(transform.position, destination, step);

        }
        if (gameManager.playerDead == true)
        {
            //Player is dead, returns enemies to spawning location until player respawns
            Scatter();
        }
    }

    Vector3 RandomStartPos()
    {
        Vector3 randomStartPos = new Vector3(Random.Range(-exitRange, exitRange + 1), Random.Range(-exitRange, exitRange + 1), 0f);
        return randomStartPos;
    }

    void Scatter()
    {
        transform.position = Vector3.MoveTowards(transform.position, retreatPos, step);
        float rotZ = Mathf.Atan2(startingPos.y - transform.position.y, startingPos.x - transform.position.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }

    private void OnDestroy()
    {
        swarmCarrier.spawnCount -= 1;
    }
}
