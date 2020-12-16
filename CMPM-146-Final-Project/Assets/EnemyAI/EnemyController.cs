
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FluentBehaviourTree;

public class EnemyController : MonoBehaviour {


    //////////////////////////// ENEMY SPRITE PARAMETERS ////////////////////////////
    private Animation enemyMovement;

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

    //////////////////////////// FUNCTIONS ////////////////////////////
    void Start() {
        enemyMovement = GetComponent<Animation>();
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
                if (!enemyMovement.IsPlaying("Enemy_Up"))
                {
                    Debug.Log("Up");
                    enemyMovement.Play("Enemy_Up");
                }
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
                if (!enemyMovement.IsPlaying("Enemy_Right"))
                {
                    Debug.Log("Right");
                    enemyMovement.Play("Enemy_Right");
                }
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
                if (!enemyMovement.IsPlaying("Enemy_Down"))
                {
                    Debug.Log("Down");
                    enemyMovement.Play("Enemy_Down");
                }
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
                if (!enemyMovement.IsPlaying("Enemy_Left"))
                {
                    Debug.Log("Left");
                    enemyMovement.Play("Enemy_Left");
                }
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
            movePoint.position = lastPosition;
        }
    }

    void FixedUpdate() {
        float laserLength = 50f;
        Vector2 startPosition = (Vector2)transform.position + new Vector2(0f, 0.5f);
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
        Debug.DrawRay(startPosition, Vector2.right * 3f, Color.red);
    }

    //KNOWN BUGS:
    // Hitbox doesn't account for walls at the moment. 
    // Will probably need two separate hitboxes; one for sound, one for direct line of vision.
    // Smart movement still needs to be implemented for the guards
}

