using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class USBPickUp : MonoBehaviour
{
	public static bool haveUsb = false;
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.name == "Player"){
			haveUsb = true;
			Destroy(gameObject);
			Debug.Log("USB Picked up");
		}
	}	
}
