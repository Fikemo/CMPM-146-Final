using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FluentBehaviourTree;
public class EnemyController : MonoBehaviour {

    //public bool enemySpotted;
    //public bool enemyHeard;

    public IBehaviourTreeNode tree;

    void Start() {
        var builder = new BehaviourTreeBuilder();
        this.tree = builder
		.Selector("my-sequence")
			.Do("action1",  t => 
			{
				// Action 1.
                Debug.Log("Hey");
				return BehaviourTreeStatus.Success;
			})
			.Do("action2", t => 
			{
				// Action 2.
                Debug.Log("Bye");
				return BehaviourTreeStatus.Failure;
			})
            .Do("action3", t => 
			{
				// Action 2.
                Debug.Log("Luc");
				return BehaviourTreeStatus.Success;
			})
		.End()
		.Build();
    }

    void Update() {
        this.tree.Tick(new TimeData(Time.deltaTime));
    }

    void FixedUpdate()
    {
        float laserLength = 50f;
        Vector2 startPosition = (Vector2)transform.position + new Vector2(-1f, 0f);
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

