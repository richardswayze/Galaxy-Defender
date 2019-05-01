using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour {

    public float cameraToPlayerDistance;

    private Player player;
    private Animator animator;
    private bool readyToPlay;
    private Animator playerAnimator;
    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        readyToPlay = false;
        playerAnimator = player.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (player && readyToPlay)
        {
            Vector3 cameraPos = new Vector3(player.transform.position.x, player.transform.position.y, cameraToPlayerDistance);
            transform.position = (cameraPos);
            transform.LookAt(player.transform);
        } else
        {
            player = FindObjectOfType<Player>();
        }   
	}

    public void CameraToStart()
    {
        animator.SetTrigger("MoveToStart");
    }

    public void CameraIsSet()
    {
        readyToPlay = true;
        playerAnimator.SetTrigger("GameState");
        gameManager.gameStarted = true;
        animator.enabled = false;
    }
}
