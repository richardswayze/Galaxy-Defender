using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Laser : MonoBehaviour {


    private Rigidbody rigidBody;
    private Transform cannon;
    private LaserStats laserStats;

    // Use this for initialization
    void Start () {
        cannon = GameObject.Find("Cannon").GetComponent<Transform>();
        transform.rotation = cannon.GetComponent<Transform>().rotation;
        laserStats = GetComponent<LaserStats>();
        int speed = laserStats.speed;
        rigidBody = GetComponent<Rigidbody>();
        float horiz = CrossPlatformInputManager.GetAxis("R Horiz");
        float vert = CrossPlatformInputManager.GetAxis("R Vert");
        Vector3 trajectory = new Vector3(horiz, vert, 0f).normalized;
        rigidBody.velocity = trajectory * speed;
    }


    //Fire the Tri-Laser and determine spread of lasers
    void FireTriLaser(Vector3 trajectory)
    {
        int speed = laserStats.speed;
        GameObject laserParent = transform.parent.gameObject;
        Rigidbody[] lasersRigidBody = laserParent.GetComponentsInChildren<Rigidbody>();
        Vector3 vectorLeft = new Vector3();
        vectorLeft = Quaternion.AngleAxis(18, Vector3.forward) * trajectory;
        Vector3 vectorRight = new Vector3();
        vectorRight = Quaternion.AngleAxis(-18, Vector3.forward) * trajectory;
        Transform[] lasersTransforms = laserParent.GetComponentsInChildren<Transform>();
        lasersTransforms[0].rotation = Quaternion.AngleAxis(18f, Vector3.forward);
        lasersTransforms[1].rotation = Quaternion.AngleAxis(-18f, Vector3.forward);
        lasersRigidBody[0].velocity = trajectory * speed;
        lasersRigidBody[1].velocity = vectorLeft * speed;
        lasersRigidBody[2].velocity = vectorRight * speed;
    }


    // Update is called once per frame
    void Update () {

    }

    
}
