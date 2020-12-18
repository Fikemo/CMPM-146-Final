using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setRepeatFlag : MonoBehaviour
{
    public static bool repeatLevel = false;

    public static void whenReplayPressed(){
        repeatLevel = true;
        Debug.Log(repeatLevel);
    }
    public static void whenNormPressed(){
        repeatLevel = false;
        Debug.Log(repeatLevel);
    }
}
