using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sight : MonoBehaviour{
	
    //public bool enemySpotted;
	//public bool enemyHeard;
 	
	void FixedUpdate()
{
    float laserLength = 50f;
    Vector2 startPosition = (Vector2)transform.position + new Vector2(0.5f, 0.2f);
    int layerMask = LayerMask.GetMask("Default");

    RaycastHit2D hit = Physics2D.Raycast(startPosition, Vector2.right, laserLength, layerMask, 0);
 
    if (hit.collider != null){
		if(hit.collider.tag == "Player"){
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

