using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour {

    public int enemyCount;

    private Enemy[] enemies;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        enemies = gameObject.GetComponentsInChildren<Enemy>();
        enemyCount = enemies.Length;
	}
}
