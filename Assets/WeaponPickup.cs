using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {

    private Player player;

    private float decayTime;
    private float creationTime;

	// Use this for initialization
	void Start () {
        decayTime = 7;
        creationTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - creationTime >= decayTime)
        {
            Destroy(gameObject);
        }
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
