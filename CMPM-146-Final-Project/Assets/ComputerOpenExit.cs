using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerOpenExit : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.name == "Player" && USBPickUp.haveUsb){
			Debug.Log("USB put into computer, exit open");
			Destroy(GameObject.Find("Exit(Clone)"));
		}
	}	
}
