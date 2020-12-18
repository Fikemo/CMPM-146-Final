using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class USBPickUp : MonoBehaviour
{
	public static bool haveUsb = false;
	GameObject usb;

	void Start(){
		Invoke("FindUsb", 0.001f);
	}
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.name == "Player"){
			haveUsb = true;
			StartCoroutine(destroyUsb());
			Debug.Log("USB Picked up");
		}
	}

	void FindUsb(){
    	usb = GameObject.Find("Key(Clone)");
     }
		IEnumerator destroyUsb(){
        	yield return new WaitForSeconds(0.5f);
			usb.active = false;
    }	
}
