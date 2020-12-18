using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenExit : MonoBehaviour
{

    public Sprite exit_open;
    public Sprite exit_door;
    // Update is called once per frame
    void Update()
    {
        if(ComputerOpenExit.haveData){
            gameObject.GetComponent<SpriteRenderer> ().sprite = exit_open;
        }
        else{
            gameObject.GetComponent<SpriteRenderer> ().sprite = exit_door;
        }
    }
}
