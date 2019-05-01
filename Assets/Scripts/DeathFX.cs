using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFX : MonoBehaviour {

    private float lifecycle = 3f;
    private float creationTime;

	// Use this for initialization
	void Start () {
        creationTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time - creationTime >= lifecycle)
        {
            Destroy(gameObject);
        }
	}
}
