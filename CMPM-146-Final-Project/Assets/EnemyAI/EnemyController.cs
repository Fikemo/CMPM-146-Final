
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FluentBehaviourTree;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Threading;

public class EnemyController : MonoBehaviour {


    //////////////////////////// ENEMY SPRITE PARAMETERS ////////////////////////////
    public Animator animator;

    //////////////////////////// BEHAVIOR TREE PARAMETERS ////////////////////////////
    public IBehaviourTreeNode tree;
    
    //////////////////////////// ENEMY MOVEMENT PARAMETERS ////////////////////////////
	
    private float EnemySpeed = 2f;
    public Transform movePoint;
    public Collider2D enemyColl;
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

    public GameObject LevelLoader;

    //////////////////////////// ENEMY PATROL PATH ////////////////////////////
    int pathIndex = 0;

    //////////////////////////// FUNCTIONS ////////////////////////////
    void Start() {

        LevelLoader = GameObject.Find("LevelLoader");
        int size = LevelLoader.GetComponent<maze>().size;
        char[,] mazeArray = LevelLoader.GetComponent<maze>().mazeArray;
        char[,] reducedMazeArray = LevelLoader.GetComponent<maze>().reducedMazeArray;

        string[] patrolPath = generatePath(mazeArray);

        movePoint.parent = null;

        var builder = new BehaviourTreeBuilder();
        this.tree = builder
		.Selector("my-sequence")
            .Do("PlayerFound", t => 
            {
                if (playerFOUND == true) {
                    checkPosition();
                    moveTowards();
                    return BehaviourTreeStatus.Success;
                }
                return BehaviourTreeStatus.Failure;
            })
			.Do("action1",  t =>
			{
                checkPosition();
                //makeMove(patrolPath);
                RandMove();
                return BehaviourTreeStatus.Success;
			})
		.End()
		.Build();
    }

    void Update() {
       this.tree.Tick(new TimeData(Time.deltaTime));
    }
    
    /////////////////////////////////////////////////// GUARD PATROL PATH SECTION /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    string[] generatePath(char[,] maze) 
    {
        //find the location of the guard
        int size = maze.GetLength(0);
        tag = gameObject.tag;
        for (int i = 0; i < size; i++) //rows (y)
        {
            for (int j = 0; j < size; j++) //columns (x)
            {
                if (maze[i, j] == tag[0])
                {
                    //found the guard
                    Debug.Log("Found Guard: " + tag + " at (" + i + "," + j + ")");

                    //find the best direction for the guards patrol path to be
                    int n = 0;
                    int e = 0;
                    int s = 0;
                    int w = 0;
                    int best = 0;
                    int dir = 4;
                    for (int k = 0; k < 4; k++)
                    {
                        int count = 0;
                        if (k == 0) //west
                        {
                            while (maze[i,j-count] != '2')
                                count++;
                            w = count - 1;
                            best = w;
                            dir = 0;
                        }
                        else if (k == 1) //south
                        {
                            while (maze[i + count, j] != '2')
                                count++;
                            s = count - 1;
                            if (s > best)
                            {
                                best = s;
                                dir = 1;
                            }
                        }
                        else if (k == 2) //east
                        {
                            while (maze[i, j + count] != '2')
                                count++;
                            e = count - 1;
                            if (e > best)
                            {
                                best = e;
                                dir = 2;
                            }
                        }
                        else //north
                        {
                            while (maze[i - count, j] != '2')
                                count++;
                            n = count - 1;
                            if (n > best)
                            {
                                best = n;
                                dir = 3;
                            }
                        }
                    }
                    string[] path = new string[best * 2 + 2];
                    //find walls
                    int openSide = 0;
                    //dir = the best direction
                    //0 = w | 1 = s | 2 = n | 3 = e
                    if (dir == 0) //west
                    {
                        Debug.Log("Best Direction: w");
                        if (maze[i - 1, j] == '2') //wall above
                        {
                            openSide = 1;
                        }
                        else //wall below or no wall
                        {
                            openSide = -1;
                        }

                        if (openSide == -1)
                        {
                            for (int q = 0; q < best; q++)
                            {
                                if (maze[i - 1, j - q] == '2')
                                {
                                    openSide = 1;
                                    break;
                                }
                            }
                        }
                        Debug.Log("Open Side: " + openSide + " Best: " + best);

                        //chart path knowing where there is wall now
                        int patrolIndex = 0;
                        bool ret = false;
                        for (int p = 0; p < 2; p++)
                        {
                            int l;
                            for (l = 1; l < best + 1; l++)
                            {
                                if (ret == false)
                                {
                                    path[patrolIndex] += i; path[patrolIndex] += ' '; path[patrolIndex] += j - l; path[patrolIndex] += ' '; path[patrolIndex] += 'w';
                                    patrolIndex++;
                                }
                                else
                                {
                                    path[patrolIndex] += i + openSide; path[patrolIndex] += ' '; path[patrolIndex] += j - best + l; path[patrolIndex] += ' '; path[patrolIndex] += 'e';
                                    patrolIndex++;
                                }
                            }
                            if (openSide == 1 && ret == false)
                            {
                                path[patrolIndex] += i + openSide; path[patrolIndex] += ' '; path[patrolIndex] += j - l; path[patrolIndex] += ' '; path[patrolIndex] += 's';
                                patrolIndex++;
                            }
                            else if (openSide == 1 && ret == true)
                            {
                                path[patrolIndex] += i; path[patrolIndex] += ' '; path[patrolIndex] += j - best + l; path[patrolIndex] += ' '; path[patrolIndex] += 'n';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == false)
                            {
                                path[patrolIndex] += i + openSide; path[patrolIndex] += ' '; path[patrolIndex] += j - l + 1; path[patrolIndex] += ' '; path[patrolIndex] += 'n';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == true)
                            {
                                path[patrolIndex] += i; path[patrolIndex] += ' '; path[patrolIndex] += j - best + l - 1; path[patrolIndex] += ' '; path[patrolIndex] += 's';
                                patrolIndex++;
                            }
                            ret = true;
                        }
                    }
                    else if (dir == 1) //south
                    {
                        Debug.Log("Best Direction: s");
                        if (maze[i, j - 1] == '2') //wall left
                        {
                            openSide = 1;
                        }
                        else //wall right or no wall
                        {
                            openSide = -1;
                        }

                        if (openSide == -1)
                        {
                            for (int q = 0; q < best; q++)
                            {
                                if (maze[i + q, j - 1] == '2')
                                {
                                    openSide = 1;
                                    break;
                                }
                            }
                        }
                        Debug.Log("Open Side: " + openSide + " Best: " + best);

                        //chart path knowing where there is wall now
                        int patrolIndex = 0;
                        bool ret = false;
                        for (int p = 0; p < 2; p++)
                        {
                            int l;
                            for (l = 1; l < best + 1; l++)
                            {
                                if (ret == false)
                                {
                                    path[patrolIndex] += i + l; path[patrolIndex] += ' '; path[patrolIndex] += j; path[patrolIndex] += ' '; path[patrolIndex] += 's';
                                    patrolIndex++;
                                }
                                else
                                {
                                    path[patrolIndex] += i + best - l; path[patrolIndex] += ' '; path[patrolIndex] += j + openSide; path[patrolIndex] += ' '; path[patrolIndex] += 'n';
                                    patrolIndex++;
                                }
                            }
                            if (openSide == 1 && ret == false)
                            {
                                path[patrolIndex] += i + l; path[patrolIndex] += ' '; path[patrolIndex] += j + openSide; path[patrolIndex] += ' '; path[patrolIndex] += 'w';
                                patrolIndex++;
                            }
                            else if (openSide == 1 && ret == true)
                            {
                                path[patrolIndex] += i + best - l; path[patrolIndex] += ' '; path[patrolIndex] += j; path[patrolIndex] += ' '; path[patrolIndex] += 'e';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == false)
                            {
                                path[patrolIndex] += i + l - 1; path[patrolIndex] += ' '; path[patrolIndex] += j + openSide; path[patrolIndex] += ' '; path[patrolIndex] += 'e';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == true)
                            {
                                path[patrolIndex] += i + best - l + 1; path[patrolIndex] += ' '; path[patrolIndex] += j; path[patrolIndex] += ' '; path[patrolIndex] += 'w';
                                patrolIndex++;
                            }
                            ret = true;
                        }
                    }
                    else if (dir == 2) //east
                    {
                        Debug.Log("Best Direction: e");
                        if (maze[i - 1, j] == '2') //wall above
                        {
                            openSide = 1;
                        }
                        else //wall below or no wall
                        {
                            openSide = -1;
                        }

                        if (openSide == -1)
                        {
                            for (int q = 0; q < best; q++)
                            {
                                if (maze[i - 1, j + q] == '2')
                                {
                                    openSide = 1;
                                    break;
                                }
                            }
                        }
                        Debug.Log("Open Side: " + openSide + " Best: " + best);

                        //chart path knowing where there is wall now
                        int patrolIndex = 0;
                        bool ret = false;
                        for (int p = 0; p < 2; p++)
                        {
                            int l;
                            for (l = 1; l < best + 1; l++)
                            {
                                if (ret == false)
                                {
                                    path[patrolIndex] += i; path[patrolIndex] += ' '; path[patrolIndex] += j + l; path[patrolIndex] += ' '; path[patrolIndex] += 'e';
                                    patrolIndex++;
                                }
                                else
                                {
                                    path[patrolIndex] += i + openSide; path[patrolIndex] += ' '; path[patrolIndex] += j + best - l; path[patrolIndex] += ' '; path[patrolIndex] += 'w';
                                    patrolIndex++;
                                }
                            }
                            if (openSide == 1 && ret == false)
                            {
                                path[patrolIndex] += i + openSide; path[patrolIndex] += ' '; path[patrolIndex] += j + l + 1; path[patrolIndex] += ' '; path[patrolIndex] += 's';
                                patrolIndex++;
                            }
                            else if (openSide == 1 && ret == true)
                            {
                                path[patrolIndex] += i; path[patrolIndex] += ' '; path[patrolIndex] += j + best - l - 1; path[patrolIndex] += ' '; path[patrolIndex] += 'n';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == false)
                            {
                                path[patrolIndex] += i + openSide; path[patrolIndex] += ' '; path[patrolIndex] += j + l - 1; path[patrolIndex] += ' '; path[patrolIndex] += 'n';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == true)
                            {
                                path[patrolIndex] += i; path[patrolIndex] += ' '; path[patrolIndex] += j + best - l + 1; path[patrolIndex] += ' '; path[patrolIndex] += 's';
                                patrolIndex++;
                            }
                            ret = true;
                        }
                    }
                    else //north
                    {
                        Debug.Log("Best Direction: n");
                        if (maze[i, j - 1] == '2') //wall left
                        {
                            openSide = 1;
                        }
                        else //wall right or no wall
                        {
                            openSide = -1;
                        }

                        if (openSide == -1)
                        {
                            for (int q = 0; q < best; q++)
                            {
                                if (maze[i - q, j - 1] == '2')
                                {
                                    openSide = 1;
                                    break;
                                }
                            }
                        }
                        Debug.Log("Open Side: " + openSide + " Best: " + best);

                        //chart path knowing where there is wall now
                        int patrolIndex = 0;
                        bool ret = false;
                        for (int p = 0; p < 2; p++)
                        {
                            int l;
                            for (l = 1; l < best + 1; l++)
                            {
                                if (ret == false)
                                {
                                    path[patrolIndex] += i - l; path[patrolIndex] += ' '; path[patrolIndex] += j; path[patrolIndex] += ' '; path[patrolIndex] += 'n';
                                    patrolIndex++;
                                }
                                else
                                {
                                    path[patrolIndex] += i - best + l; path[patrolIndex] += ' '; path[patrolIndex] += j + openSide; path[patrolIndex] += ' '; path[patrolIndex] += 's';
                                    patrolIndex++;
                                }
                            }
                            if (openSide == 1 && ret == false)
                            {
                                path[patrolIndex] += i - l; path[patrolIndex] += ' '; path[patrolIndex] += j + openSide; path[patrolIndex] += ' '; path[patrolIndex] += 'w';
                                patrolIndex++;
                            }
                            else if (openSide == 1 && ret == true)
                            {
                                path[patrolIndex] += i - best + l; path[patrolIndex] += ' '; path[patrolIndex] += j; path[patrolIndex] += ' '; path[patrolIndex] += 'e';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == false)
                            {
                                path[patrolIndex] += i - l + 1; path[patrolIndex] += ' '; path[patrolIndex] += j + openSide; path[patrolIndex] += ' '; path[patrolIndex] += 'e';
                                patrolIndex++;
                            }
                            else if (openSide == -1 && ret == true)
                            {
                                path[patrolIndex] += i - best + l - 1; path[patrolIndex] += ' '; path[patrolIndex] += j; path[patrolIndex] += ' '; path[patrolIndex] += 'w';
                                patrolIndex++;
                            }
                            ret = true;
                        }
                    }
                    Debug.Log(path);
                    return path;
                }
            }
        }
        return null;
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

            float lowestDistance = Mathf.Min(upDistance, rightDistance, downDistance, leftDistance);

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
        return Vector2.Distance(v1,v2);
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
        
        
    }

    void MoveLeft() {
        ResetTrigger();
        animator.SetTrigger("WestWalk");
        movePoint.position += new Vector3(-1.6f, 0f, 0f);
        
        
    }

    void MoveRight() {
        ResetTrigger();
        animator.SetTrigger("EastWalk");
        movePoint.position += new Vector3(1.6f, 0f, 0f);
        
        
    }

    void MoveDown() {
        ResetTrigger();
        animator.SetTrigger("SouthWalk");
        movePoint.position += new Vector3(0f, -1.6f, 0f);
        
        
    }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    void makeMove(string[] pPath)
    {
        string temp = pPath[pathIndex];
        string[] temp2 = temp.Split(' ');
        char coord = temp2[2][0];
        
        if (coord == 'n') //moveUp
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("NorthWalk");
                moveBack = 0;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(0f, 1.6f, 0f);
            }
        }
        else if (coord == 'e') //moveRight
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("EastWalk");
                moveBack = 1;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(1.6f, 0f, 0f);
            }
        }
        else if (coord == 's') //moveDown
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("SouthWalk");
                moveBack = 2;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(0f, -1.6f, 0f);
            }
        }
        else //moveLeft
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, EnemySpeed * Time.deltaTime);
            if (isMoving != true)
            { // If not moving, allow movement
                ResetTrigger();
                animator.SetTrigger("WestWalk");
                moveBack = 3;
                lastPosition = movePoint.position;
                movePoint.position += new Vector3(-1.6f, 0f, 0f);
            }
        }

        if (pathIndex == pPath.Length - 1)
        {
            pathIndex = 0;
        }
        else
        {
            pathIndex++;
        }
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
