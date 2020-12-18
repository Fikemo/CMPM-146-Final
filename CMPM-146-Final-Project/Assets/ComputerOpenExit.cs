using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerOpenExit : MonoBehaviour
{
	public static bool haveData = false;

	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.name == "Player" && USBPickUp.haveUsb){
			Debug.Log("USB put into computer, exit open");
			haveData = true;
		}
	}	
}
