using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public float rotationSpeed;

	// Use this for initialization
	void Start () {
        rotationSpeed = rotationSpeed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0f, rotationSpeed, 0f);
	}
}
