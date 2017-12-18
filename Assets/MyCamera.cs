using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour {

    public float cameraToPlayerDistance;

    private Player player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (player)
        {
            Vector3 cameraPos = new Vector3(player.transform.position.x, player.transform.position.y, cameraToPlayerDistance);
            transform.position = (cameraPos);
            transform.LookAt(player.transform);
        } else
        {
            player = FindObjectOfType<Player>();
        }   
	}
}
