using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    
    [SerializeField] int maxShield = 7;
    [SerializeField] bool vulnerable = true;
    [SerializeField] int moveSpeed;
    [SerializeField] float shieldRechargeWait;
    [SerializeField] float shieldRechargeRate;

    public int health = 5;
    public int currentLaser;
    public float shield;
    public LaserStats laserStats;
    public GameObject[] laserPrefabs;
    public GameObject deathFX;
    public GameObject laserDamagePrefab;

    private float p_rightHoriz;
    private float p_rightVert;
    private Vector3 p_move = new Vector3();
    private Transform playerShip;
    private Transform cannon;
    private float timeFired;
    private bool canFire;
    private float collisionTime;
    private float timeSinceCollision;
    private Text shieldText;
    private Text healthText;
    private GameManager gameManager;


    void Start()
    {
        playerShip = GameObject.Find("playership").GetComponent<Transform>();
        currentLaser = 0;
        cannon = GameObject.Find("Cannon").GetComponent<Transform>();
        canFire = true;
        collisionTime = 0;
        timeSinceCollision = collisionTime - Time.timeSinceLevelLoad;
        shield = maxShield;
        shieldText = GameObject.Find("Shield").GetComponent<Text>();
        shieldText.enabled = false;
        healthText = GameObject.Find("Health").GetComponent<Text>();
        healthText.enabled = false;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        laserStats = laserPrefabs[currentLaser].GetComponent<LaserStats>();
    }

    void Update()
    {
        if(gameManager.gameStarted == true)
        {
            //Player movement
            p_move.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            p_move.y = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(Mathf.Clamp(transform.position.x + p_move.x, -80f, 80f), Mathf.Clamp(transform.position.y + p_move.y, -50f, 50f), 0f);

            //Right stick input
            p_rightHoriz = Input.GetAxis("R Horiz");
            p_rightVert = Input.GetAxis("R Vert");
            

            shieldText.enabled = true;
            healthText.enabled = true;
        }

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
                RechargeShield();
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
            CancelInvoke("RechargeShield"); //TODO is there an "invoke" call for this?
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
        // Checkes if player in vulnerable state, then sorts into laser or collision damage
        if (vulnerable)
        {
            CancelInvoke("RechargeShield");
            collisionTime = Time.time;

            // Collision damage
            if (collider.GetComponent<Enemy>())
            {
                int collisionDamage = collider.GetComponent<Enemy>().collisionDamage;
                if (shield > 0)
                {
                    shield -= collisionDamage;
                    if (shield < 0)
                    {
                        shield = 0;
                    }
                }
                else
                {
                    health -= collisionDamage;
                }
            }

            // Laser damage
            if (collider.GetComponent<EnemyLaser>())
            {
                EnemyLaser laser = collider.GetComponent<EnemyLaser>();
                int damageApplied = laser.damage;
                if (shield > 0)
                {
                    shield -= damageApplied;
                    Instantiate(laserDamagePrefab, transform.position, Quaternion.identity);
                    if (shield < 0)
                    {
                        shield = 0;
                    }
                }
                else
                {
                    health -= damageApplied;
                }
                Destroy(collider.gameObject);
            }
        }
    }

    void PlayerDeath()
    {
        gameManager.playerDead = true;
        Instantiate(deathFX, transform.position, Quaternion.identity);
        gameManager.lives -= 1;
        if (gameManager.lives < 0)
        {
            gameManager.GameOver();
        } else
        {
            gameManager.respawnLoc = transform.position;
            gameManager.Invoke("Respawn", gameManager.respawnTime);
        }
        Destroy(gameObject);
    }
}
