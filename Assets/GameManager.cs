using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int lives = 3;
    public GameObject playerPrefab;
    public Vector3 respawnLoc;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Respawn()
    {
        Debug.Log("Respawn invoked");
        Instantiate(playerPrefab, respawnLoc, Quaternion.identity);
    }
}
