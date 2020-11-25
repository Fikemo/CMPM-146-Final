using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sight : MonoBehaviour{
	
    public bool enemySpotted;
	public bool enemyHeard;
 
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Player"){
			Debug.Log("Touched the hitbox");
            Application.LoadLevel(Application.loadedLevel);
        }
    }
	
	//KNOWN BUGS:
	// Hitbox doesn't account for walls at the moment. 
	// Will probably need two separate hitboxes; one for sound, one for direct line of vision.
	// Smart movement still needs to be implemented for the guards
}

