using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class TriLaser : MonoBehaviour {

    private Transform cannon;
    private LaserStats laserStats;
    private int laserChildren;

    // Use this for initialization
    void Start () {

        //Retrieve stats and right stick direction
        cannon = GameObject.Find("Cannon").GetComponent<Transform>();
        transform.rotation = cannon.GetComponent<Transform>().rotation;
        laserStats = GetComponent<LaserStats>();
        int speed = laserStats.speed;
        float horiz = CrossPlatformInputManager.GetAxis("R Horiz");
        float vert = CrossPlatformInputManager.GetAxis("R Vert");
        Vector3 trajectory = new Vector3(horiz, vert, 0f).normalized;

        //Alter trajectories for diagonal shots
        Rigidbody[] lasersRigidBody = GetComponentsInChildren<Rigidbody>();
        Vector3 vectorLeft = new Vector3();
        vectorLeft = Quaternion.AngleAxis(20, Vector3.forward) * trajectory;
        Vector3 vectorRight = new Vector3();
        vectorRight = Quaternion.AngleAxis(-20, Vector3.forward) * trajectory;

        //Fire lasers with their trajectories
        lasersRigidBody[0].velocity = trajectory * speed;
        lasersRigidBody[1].velocity = vectorLeft * speed;
        lasersRigidBody[2].velocity = vectorRight * speed;
    }
	
	// Update is called once per frame
	void Update () {
        laserChildren = transform.childCount;
        if (laserChildren == 0)
        {
            Destroy(gameObject);
        }
	}

    
}
