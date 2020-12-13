
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FluentBehaviourTree;
public class EnemyController : MonoBehaviour {

    //////////////////////////// BEHAVIOR TREE PARAMETERS ////////////////////////////
    public IBehaviourTreeNode tree;
    
    //////////////////////////// ENEMY MOVEMENT PARAMETERS ////////////////////////////
	
    private float EnemySpeed = 1f;
    public Transform movePoint;
    public Collider2D enemyColl;
    private float saveHoriz = 0f;
    private float saveVert = 0f;
	public bool upBlock = false;
	public bool leftBlock = false;
	public bool rightBlock = false;
	public bool downBlock = false; 

    //////////////////////////// FUNCTIONS ////////////////////////////
    void Start() {
        var builder = new BehaviourTreeBuilder();
        this.tree = builder
		.Selector("my-sequence")
			.Do("action1",  t => 
			{
				// Action 1.
                if (upBlock == true) {
                    return BehaviourTreeStatus.Failure;
                }
                else {
                    MoveUp();
                    return BehaviourTreeStatus.Success;
                }
			})
			.Do("action2", t => 
			{
                // Action 2.
                if (rightBlock == true) {
                    return BehaviourTreeStatus.Failure;
                }
                else {
                    MoveRight();
                    return BehaviourTreeStatus.Success;
                }
			})
            .Do("action3", t => 
			{
				// Action 2.
                if (downBlock == true) {
                    return BehaviourTreeStatus.Failure;
                }
                else {
                    MoveDown();
                    return BehaviourTreeStatus.Success;
                }
			})
            .Do("action4", t => 
			{
				// Action 2.
                if (leftBlock == true) {
                    return BehaviourTreeStatus.Failure;
                }
                else {
                    MoveLeft();
                    return BehaviourTreeStatus.Success;
                }
			})
		.End()
		.Build();
    }

    void Update() {
        this.tree.Tick(new TimeData(Time.deltaTime));
    }

    void MoveUp() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);

        movePoint.position += new Vector3(0f, 1.6f, 0f);
        saveHoriz = 0f;
        saveVert = -1.6f;
    }

	void MoveLeft(){
		transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);

        movePoint.position += new Vector3(-1.6f, 0f, 0f);
        saveHoriz = 1.6f;
        saveVert = 0f;
	}

    void MoveRight() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);

        movePoint.position += new Vector3(1.6f, 0f, 0f);
        saveHoriz = -1.6f;
        saveVert = 0f;
    }

	void MoveDown(){
		transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);

        movePoint.position += new Vector3(0f, -1.6f, 0f);
        saveHoriz = 0f;
        saveVert = 1.6f;
	}

    void OnTriggerEnter2D(Collider2D enemyColl) {
        if (enemyColl.gameObject.name == "Wall(Clone)") {
            if (saveHoriz == 0f && saveVert == -1.6f) {
                upBlock = true;
            }
            else if (saveHoriz == 0f && saveVert == 1.6f){
                downBlock = true; 
            }
            else if (saveHoriz == 1.6f && saveVert == 0f){
                leftBlock = true;
            }
            else if (saveHoriz == -1.6f && saveVert == 0f){
                rightBlock = true;
            }
            movePoint.position += new Vector3(saveHoriz, saveVert, 0f);
        }
    }
    void onTriggerExit2D(Collider2D enemyColl){
        if (enemyColl.gameObject.name == "Wall(Clone)") {
            if (saveHoriz == 0f && saveVert == -1.6f) {
                upBlock = false;
            }
            else if (saveHoriz == 0f && saveVert == 1.6f){
                downBlock = false; 
            }
            else if (saveHoriz == 1.6f && saveVert == 0f){
                leftBlock = false;
            }
            else if (saveHoriz == -1.6f && saveVert == 0f){
                rightBlock = false;
            }
        }
    }

    void FixedUpdate() {
        float laserLength = 50f;
        Vector2 startPosition = (Vector2)transform.position + new Vector2(0f, -1.6f);
        int layerMask = LayerMask.GetMask("Default");

        RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.right, laserLength, layerMask, 0);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                Debug.Log("Hitting the Player");
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        //Debug.DrawRay(startPosition, Vector2.right * laserLength, Color.red);
    }

    //KNOWN BUGS:
    // Hitbox doesn't account for walls at the moment. 
    // Will probably need two separate hitboxes; one for sound, one for direct line of vision.
    // Smart movement still needs to be implemented for the guards
}

