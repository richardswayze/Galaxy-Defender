using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserStats : MonoBehaviour {

    public int speed;
    public int damage;
    public float fireRate;

    private Rigidbody rigidBody;
    
	// Use this for initialization
	void Start () {

	}

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Enemy>())  
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            enemy.health -= damage;
            Destroy(gameObject);
        }
    }
}
