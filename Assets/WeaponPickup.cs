using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {

    private Player player;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
            player.currentLaser = 1;
            player.laserStats = player.laserPrefabs[1].GetComponent<LaserStats>();
            Destroy(gameObject);
        }
    }
}
