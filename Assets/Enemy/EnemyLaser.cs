using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour {

    public int speed;
    public int damage;
    public int error;

    private Player player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<Player>();
        float tangent = Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x) * Mathf.Rad2Deg + 90 + error;
        transform.rotation = Quaternion.AngleAxis(tangent, Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
	}
}
