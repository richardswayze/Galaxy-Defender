using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Laser>()) {
            Destroy(collider.gameObject);
        }
        if (collider.GetComponent<TriLaser>())
        {
            Destroy(collider.gameObject);
        }
    }
}
