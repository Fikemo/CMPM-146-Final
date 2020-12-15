using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This can be placed on the current Player sprite to see its functionality.

public class Pathfind : MonoBehaviour
{
    public GameObject usbKey;
    public GameObject player;

//Retrieves the Key(Clone) object almost instantly after map is generated.

    void Start()
    {
        Invoke("FindUsb", 0.001f);
    }

    private void FindUsb(){
        usbKey = GameObject.Find("Key(Clone)");
        if(usbKey != null){
            Debug.Log("Key was found");
        }
        else{
            Debug.Log("Key NOT FOUND");
        }
    }

//getDistance()
//Calculates distance from one point to another; does not account for obstacles, but instead calculates distance from src to dest
    private float getDistance(GameObject source, GameObject dest){
        Vector2 v1 = new Vector2(source.transform.position.x, source.transform.position.y);
        Vector2 v2 = new Vector2(dest.transform.position.x, dest.transform.position.y);
        return Vector2.Distance(v1,v2);
    }

//getAdjacentMoves()
//OUTPUT: List of lists storing floats: an x coordinate of an adjacent space, and a y coordinate of an adjacent space.
//Accounts for north/south/east/west movement. Need to consider removing coordinates of walls and enemies. 
    public List<List<float>> getAdjacentMoves(GameObject source){
        var adjList = new List<List<float>>();
        var dirs = new List<List<float>>();

        dirs.Add(new List<float>{ 1.6f, 0f });
        dirs.Add(new List<float>{ -1.6f, 0f });
        dirs.Add(new List<float> { 0f, 1.6f });
        dirs.Add(new List<float> { 0f, -1.6f });

        float adjX;
        float adjY;

        foreach (var dir in dirs){
            adjX = source.transform.position.x + dir[0];
            adjY = source.transform.position.y + dir[1];
            adjList.Add(new List<float>{adjX, adjY});
        }
        return adjList;
    }


    


    void Update(){
        /*
        var x = getAdjacentMoves(player);
        foreach (List<float> i in x){
            Debug.Log("Adjacent x coord: " + i[0]);
            Debug.Log("Adjacent y coord: " + i[1]);
            */
        }
    }

