
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

    //////////////////////////// ENEMY DETECTION PARAMETERS ////////////////////////////
    Vector2 anglePosition;
    Vector2 castingPosition;

    bool playerFOUND = false; 
    
    public GameObject playerPosition;


    //////////////////////////// FUNCTIONS ////////////////////////////
    void Start() {
        movePoint.parent = null;

        var builder = new BehaviourTreeBuilder();
        this.tree = builder
		.Selector("my-sequence")
            .Do("PlayerFound", t => 
            {
                if (playerFOUND == true) {
                    checkPosition();
                    moveTowards();
                    return BehaviourTreeStatus.Success; // IN HERE IS WHERE WE SHOULD IMPLEMENT THE TRACK PLAYER FUNCTIOn
                }
                return BehaviourTreeStatus.Failure;
            })
			.Do("action1",  t => 
			{
				// Action 1.
                checkPosition();
                RandMove();
                return BehaviourTreeStatus.Success;
			})
		.End()
		.Build();
    }

    void Update() {
        this.tree.Tick(new TimeData(Time.deltaTime));
    }

/////////////////////////////////////////////////// ENEMY DETECTION SECTION /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void moveTowards() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);

        if (isMoving != true) {
            float startingDistance = getDistance(gameObject.transform.position.x, gameObject.transform.position.y, playerPosition.transform.position.x, playerPosition.transform.position.y);
            float upDistance;
            float rightDistance;
            float downDistance;
            float leftDistance;

            if (free("Up") == true) {
                upDistance = getDistance(gameObject.transform.position.x, gameObject.transform.position.y + 1.6f, playerPosition.transform.position.x, playerPosition.transform.position.y);
            }
            else upDistance = 100000000; 

            if (free("Right") == true) {
                rightDistance = getDistance(gameObject.transform.position.x + 1.6f, gameObject.transform.position.y, playerPosition.transform.position.x, playerPosition.transform.position.y);
            }
            else rightDistance = 100000000;

            if (free("Down") == true) {
                downDistance = getDistance(gameObject.transform.position.x, gameObject.transform.position.y - 1.6f, playerPosition.transform.position.x, playerPosition.transform.position.y);
            }
            else downDistance = 100000000;

            if (free("Left") == true) {
                leftDistance = getDistance(gameObject.transform.position.x - 1.6f, gameObject.transform.position.y, playerPosition.transform.position.x, playerPosition.transform.position.y);
            }
            else leftDistance = 100000000;

            Debug.Log("up" + upDistance);
            Debug.Log("down" + downDistance);
            Debug.Log("right" + rightDistance);
            Debug.Log("left" + leftDistance);
            float lowestDistance = Mathf.Min(upDistance, rightDistance, downDistance, leftDistance);
            Debug.Log("UP: " + upDistance);
            Debug.Log("RIGHT: " + rightDistance);
            Debug.Log("DOWN: " + downDistance);
            Debug.Log("LEFT: " + leftDistance);

            if (lowestDistance == upDistance) {
                Debug.Log("Moving UP");
                MoveUp();
            }
            else if (lowestDistance == rightDistance) {
                Debug.Log("Moving Right");
                MoveRight();
            }
            else if (lowestDistance == downDistance) {
                Debug.Log("Moving Down");
                MoveDown();
            }
            else if (lowestDistance == leftDistance) {
                Debug.Log("Moving Left");
                MoveLeft();
            }
        }
    }

    float getDistance(float x1, float y1, float x2, float y2) {
        Vector2 v1 = new Vector2(x1, y1);
        Vector2 v2 = new Vector2(x2, y2);
        return Vector2.Distance(v2,v1);
    }

    bool free(string direction) {
        float rayLength = 2f; // LENGTH OF THE LINE ITSELF
        Vector2 rayPosition = (Vector2)transform.position + new Vector2(0f, 0.5f);
        if (direction == "Up") {
            castingPosition = new Vector2(0f, 1f);
        }
        if (direction == "Right") {
            castingPosition = new Vector2(1f, 0f);
        }
        if (direction == "Down") {
            castingPosition = new Vector2(0f, -1f);
        }
        if (direction == "Left") {
            castingPosition = new Vector2(-1f, 0f);
        }

        int layerMask = LayerMask.GetMask("Default");

        RaycastHit2D castingHit = Physics2D.Raycast(rayPosition, castingPosition, rayLength, layerMask, 0);

        if (castingHit.collider != null)
        {
            if (castingHit.collider.tag == "Wall")
            {
                Debug.Log("Hitting Wall: " + direction);
                return false;
            }
        }
        Debug.DrawRay(rayPosition, castingPosition * 2f, Color.blue, 0.01f);
        return true;
    }

    void MoveUp() {
        ResetTrigger();
        animator.SetTrigger("NorthWalk");
        movePoint.position += new Vector3(0f, 1.6f, 0f);
        saveHoriz = 0f;
        saveVert = -1.6f;

    }

    void MoveLeft() {
        ResetTrigger();
        animator.SetTrigger("WestWalk");
        movePoint.position += new Vector3(-1.6f, 0f, 0f);
        saveHoriz = 1.6f;
        saveVert = 0f;
    }

    void MoveRight() {
        ResetTrigger();
        animator.SetTrigger("EastWalk");
        movePoint.position += new Vector3(1.6f, 0f, 0f);
        saveHoriz = -1.6f;
        saveVert = 0f;
    }

    void MoveDown() {
        ResetTrigger();
        animator.SetTrigger("SouthWalk");
        movePoint.position += new Vector3(0f, -1.6f, 0f);
        saveHoriz = 0f;
        saveVert = 1.6f;
    }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
                moveBack = 2;
                anglePosition = new Vector2(0f, -1f);
            }
            else if (moveBack == 1) {
                animator.SetTrigger("WestWalk");
                moveBack = 3;
                anglePosition = new Vector2(-1f, 0f);
            }
            else if (moveBack == 2) {
                animator.SetTrigger("NorthWalk");
                moveBack = 0;
                anglePosition = new Vector2(0f, 1f);
            }
            else if (moveBack == 3) {
                animator.SetTrigger("EastWalk");
                moveBack = 1;
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
        float laserLength = 4f; // LENGTH OF THE LINE ITSELF
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
                //Debug.Log("Hitting the Player");
                playerFOUND = true; // SET TO TRUE NOW THAT PLAYER HAS BEEN SEEN
                // WE NEED A POINT TO RESET BACK TO FALSE SO THAT WAY THEY STOP TRACKING THE PLAYER, OR MAYBE WE JSUT ALWAYS HAVE THEM TRACK THE PLAYER
            }
        }
        Debug.DrawRay(startPosition, anglePosition * 3f, Color.red);
    }
}
