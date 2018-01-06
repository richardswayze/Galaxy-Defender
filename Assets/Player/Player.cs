using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField] int health = 5;
    [SerializeField] int maxShield = 7;
    [SerializeField] bool vulnerable = true;
    [SerializeField] int moveSpeed;
    [SerializeField] GameObject[] laserPrefabs;
    [SerializeField] float shieldRechargeWait;
    [SerializeField] float shieldRechargeRate;
    [SerializeField] float shieldRechargeSpeed;
    [SerializeField] int respawnTime;

    public int currentLaser;
    public float shield;

    private float p_horiz;
    private float p_vert;
    private float p_rightHoriz;
    private float p_rightVert;
    private Vector3 p_move = new Vector3();
    private Transform playerShip;
    private Transform cannon;
    private float timeFired;
    private bool canFire;
    private LaserStats laserStats;
    private float collisionTime;
    private float timeSinceCollision;
    private Text shieldText;
    private Text healthText;
    private EnemyParent enemyParent;
    private GameManager gameManager;


    void Start()
    {
        playerShip = GameObject.Find("playership").GetComponent<Transform>();
        currentLaser = 0;
        cannon = GameObject.Find("Cannon").GetComponent<Transform>();
        canFire = true;
        laserStats = laserPrefabs[currentLaser].GetComponent<LaserStats>();
        collisionTime = 0;
        timeSinceCollision = collisionTime - Time.timeSinceLevelLoad;
        shield = maxShield;
        shieldText = GameObject.Find("Shield").GetComponent<Text>();
        healthText = GameObject.Find("Health").GetComponent<Text>();
        enemyParent = GameObject.FindObjectOfType<EnemyParent>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    void Update()
    {
        //Player movement
        p_move.x = CrossPlatformInputManager.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        p_move.y = CrossPlatformInputManager.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x + p_move.x, -80f, 80f), Mathf.Clamp(transform.position.y + p_move.y, -50f, 50f), 0f);
        
        //Right stick input
        p_rightHoriz = CrossPlatformInputManager.GetAxis("R Horiz");
        p_rightVert = CrossPlatformInputManager.GetAxis("R Vert");
       
        if (health <= 0)
        {
            PlayerDeath();
        }

        //Player model rotation
        if (p_move.x != 0 || p_move.y != 0)
        {
            float tangent = Mathf.Atan2(p_move.y, p_move.x) * Mathf.Rad2Deg + 90;
            playerShip.transform.rotation = Quaternion.AngleAxis(tangent, Vector3.forward);
        }

        //Rotates direction of fire and fires laser
        if (p_rightHoriz != 0 || p_rightVert != 0)
        {
            float tangent = Mathf.Atan2(p_rightVert, p_rightHoriz) * Mathf.Rad2Deg - 90;
            cannon.transform.rotation = Quaternion.AngleAxis(tangent, Vector3.forward);
            FireLaser();
        }

        //Shield recharging
        if (shield < maxShield)
        {
            timeSinceCollision = Time.time - collisionTime;
            if (timeSinceCollision >= shieldRechargeWait)
            {
                InvokeRepeating("RechargeShield", .01f, shieldRechargeSpeed);
            }
        }

        shieldText.text = Mathf.RoundToInt(shield).ToString();
        healthText.text = health.ToString();


    }

    void RechargeShield()
    {
        shield += shieldRechargeRate;
        if (shield >= maxShield)
        {
            CancelInvoke("RechargeShield");
            shield = maxShield;
        }

    }

    //Fires laser checking if time since last fired laser has been longer than fireRate
    void FireLaser()
    {
        if (Time.time - timeFired >= laserStats.fireRate || canFire)
        {
            Instantiate(laserPrefabs[currentLaser], cannon.transform.position, Quaternion.identity);
            timeFired = Time.time;
            canFire = false;
        } 
    }

    void OnTriggerEnter(Collider collider)
    {
        collisionTime = Time.time;
        if (collider.GetComponent<Enemy>())
        {
            int collisionDamage = collider.GetComponent<EnemyStats>().collisionDamage;
            if (shield > 0)
            {
                shield -= collisionDamage;
                if (shield < 0)
                {
                    shield = 0;
                }
            } else
            {
                health -= collisionDamage;
            }
        }

        if (collider.GetComponent<EnemyLaser>())
            {
                EnemyLaser laser = collider.GetComponent<EnemyLaser>();
                int damageApplied = laser.damage;
                if (shield > 0)
                {
                    shield -= damageApplied;
                    if (shield < 0)
                    {
                        shield = 0;
                    }
                }
                else
                {
                    health -= damageApplied;
                }
            }
        Destroy(collider.gameObject);
    }

    void PlayerDeath()
    {
        gameManager.lives -= 1;
        if (gameManager.lives < 0)
        {
            EndGame();
        } else
        {
            Debug.Log("Let's invoke.");
            gameManager.respawnLoc = transform.position;
            gameManager.Invoke("Respawn", respawnTime);

        }
        Destroy(gameObject);
        }

    void EndGame()
    {
        print("Game has ended.");
    }
}
