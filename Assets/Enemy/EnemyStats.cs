﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    public float speed;
    public int minimumDistanceToPlayer;
    public int health;
    public int collisionDamage;
    public float firingDistance;
    public float fireRate;
    public int accuracy;


	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
	}


}
