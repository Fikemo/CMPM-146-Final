
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FluentBehaviourTree;

public class EnemyController : MonoBehaviour {


    //////////////////////////// ENEMY SPRITE PARAMETERS ////////////////////////////
    public Animator animator;

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
    public bool isMoving = false;
    private Vector3 lastPosition;
    int moveBack = 0;

    //////////////////////////// ENEMY DETECTIOn PARAMETERS ////////////////////////////
    Vector2 anglePosition;

    //////////////////////////// FUNCTIONS ////////////////////////////
    void Start() {
        movePoint.parent = null;

        var builder = new BehaviourTreeBuilder();
        this.tree = builder
		.Selector("my-sequence")
            .Do("AllowEnemyMovement", t => 
            {
                checkPosition();
                return BehaviourTreeStatus.Failure;
            })
			.Do("action1",  t => 
			{
				// Action 1.
                RandMove();
                return BehaviourTreeStatus.Success;
			})
            /* NON-RANDOM MOVEMENT, REQUIRES OnTriggerEnter to be edited to include saveHoriz, saveVert, and all bool block variables
            .Do("action1",  t => 
			{
				// Action 1.
                Debug.Log("Moving Up");
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
                    upBlock = false;
                    rightBlock = false;
                    downBlock = false;
                    leftBlock = false;
                    return BehaviourTreeStatus.Failure;
                }
                else {
                    MoveLeft();
                    return BehaviourTreeStatus.Success;
                }
                RandMove();
                return BehaviourTreeStatus.Success;
			})
            */
		.End()
		.Build();
    }

    void Update() {
        this.tree.Tick(new TimeData(Time.deltaTime));
    }

    void RandMove() {
        int move = UnityEngine.Random.Range(0, 4); // RANDOM NUMBER BETWEEN 0 - 3, since 4 is (exclusive) in the random range
        if (move == 0) { //Moving up
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("NorthWalk");
                moveBack = 0;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(0f, 1.6f, 0f);
                saveHoriz = 0f;
                saveVert = -1.6f;
            }
        }
        if (move == 1) { //Moving Right
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("EastWalk");
                moveBack = 1;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(1.6f, 0f, 0f);
                saveHoriz = -1.6f;
                saveVert = 0f;
            }
        }
        if (move == 2) { //Moving Down
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("SouthWalk");
                moveBack = 2;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(0f, -1.6f, 0f);
                saveHoriz = 0f;
                saveVert = 1.6f;
            }
        }
        if (move == 3) { //Moving Left
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("WestWalk");
                moveBack = 3;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(-1.6f, 0f, 0f);
                saveHoriz = 1.6f;
                saveVert = 0f;
            }
        }
    }

    void MoveUp() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
        if (isMoving != true) { // If not moving, allow movement
            movePoint.position += new Vector3(0f, 1.6f, 0f);
            saveHoriz = 0f;
            saveVert = -1.6f;
        }
    }

	void MoveLeft(){
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
        if (isMoving != true) { // If not moving, allow movement
            movePoint.position += new Vector3(-1.6f, 0f, 0f);
            saveHoriz = 1.6f;
            saveVert = 0f;
        }
	}

    void MoveRight() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
        if (isMoving != true) { // If not moving, allow movement
            movePoint.position += new Vector3(1.6f, 0f, 0f);
            saveHoriz = -1.6f;
            saveVert = 0f;
        }
    }

	void MoveDown() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
        if (isMoving != true) { // If not moving, allow movement
            movePoint.position += new Vector3(0f, -1.6f, 0f);
            saveHoriz = 0f;
            saveVert = 1.6f;
        }
	}

    void checkPosition() {
        if (transform.position == movePoint.position) {
            //Debug.Log("Point and Enemy are at same Position");
            isMoving = false;
        }
        else if (transform.position != movePoint.position) {
            isMoving = true;
        }
    }

    void OnTriggerEnter2D(Collider2D enemyColl) {
        if (enemyColl.gameObject.name == "Wall(Clone)" || enemyColl.gameObject.name == "Enemy(Clone)") {
            ResetTrigger();
            if (moveBack == 0) {
                animator.SetTrigger("SouthWalk");
                anglePosition = new Vector2(0f, -1f);
            }
            else if (moveBack == 1) {
                animator.SetTrigger("WestWalk");
                anglePosition = new Vector2(-1f, 0f);
            }
            else if (moveBack == 2) {
                animator.SetTrigger("NorthWalk");
                anglePosition = new Vector2(0f, 1f);
            }
            else if (moveBack == 3) {
                animator.SetTrigger("EastWalk");
                anglePosition = new Vector2(1f, 0f);
            }
            movePoint.position = lastPosition;
        }
    }

    void ResetTrigger() {
        animator.ResetTrigger("NorthWalk");
        animator.ResetTrigger("EastWalk");
        animator.ResetTrigger("SouthWalk");
        animator.ResetTrigger("WestWalk");
    }

    void FixedUpdate() {
        float laserLength = 3f;
        Vector2 startPosition = (Vector2)transform.position + new Vector2(0f, 0.5f);
        int layerMask = LayerMask.GetMask("Default");

        if (moveBack == 0) {
            anglePosition = new Vector2(0f, 1f);
        }
        else if (moveBack == 1) {
            anglePosition = new Vector2(1f, 0f);
        }
        else if (moveBack == 2) {
            anglePosition = new Vector2(0f, -1f);
        }
        else if (moveBack == 3) {
            anglePosition = new Vector2(-1f, 0f);
        }

        RaycastHit2D hit = Physics2D.Raycast(startPosition, anglePosition, laserLength, layerMask, 0);

        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                Debug.Log("Hitting the Player");
                Application.LoadLevel(Application.loadedLevel);
            }
        }
        Debug.DrawRay(startPosition, anglePosition * 3f, Color.red);
    }
}
