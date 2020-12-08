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

    //////////////////////////// FUNCTIONS ////////////////////////////
    void Start() {
        movePoint.parent = null;

        var builder = new BehaviourTreeBuilder();
        this.tree = builder
		.Sequence("my-sequence")
			.Do("action1",  t => 
			{
				// Action 1.
                Debug.Log("Hello");
				return BehaviourTreeStatus.Success;
			})
			.Do("action2", t => 
			{
                // Action 2.
                Debug.Log("Goodbye");
                return BehaviourTreeStatus.Success;
			})
            .Do("action3", t => 
			{
				// Action 2.
                Debug.Log("World");
				return BehaviourTreeStatus.Success;
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
        saveHoriz = 0;
        saveVert = -1.6f;
    }

    void OnTriggerEnter2D(Collider2D enemyColl) {
        if (enemyColl.gameObject.name == "Wall(Clone)") {
            //Debug.Log("CrashedIntoAFuckingWall");
            movePoint.position += new Vector3(saveHoriz, saveVert, 0f);
        }
    }

    void FixedUpdate() {
        float laserLength = 50f;
        Vector2 startPosition = (Vector2)transform.position + new Vector2(1.6f, 0f); // CHANGING THESE VALUES HERE WILL CHANGE WHER TH
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

