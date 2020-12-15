using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

//This can be placed on the current Player sprite to see its functionality.

public class Pathfind : MonoBehaviour
{
    public GameObject usbKey;
    public GameObject player;
    private GameObject[] walls;
    public List<List<float>> wallMaster;

//Invoke is needed because objects don't exist before compile time (map isn't generated yet!)

    void Start()
    {
        Invoke("FindUsb", 0.001f);
        Invoke("getWalls", 0.01f);
        Invoke("getWallCoords", 0.1f);

    }

    //Retrives usbKey object
    private void FindUsb(){
        usbKey = GameObject.Find("Key(Clone)");
    }

    //Populates walls list with wall GameObjects
    private void getWalls(){
        walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    //Populates List of Lists storing coordinates of each wall location 
    private void getWallCoords(){
        wallMaster = new List<List<float>>();
        foreach (GameObject w in walls){
            wallMaster.Add(new List<float>{w.transform.position.x, w.transform.position.y});
        }
    }

//getDistance()
//Calculates distance from one point to another; does not account for obstacles, but instead calculates distance from src to dest
    private float getDistance(GameObject source, GameObject dest){
        Vector2 v1 = new Vector2(source.transform.position.x, source.transform.position.y);
        Vector2 v2 = new Vector2(dest.transform.position.x, dest.transform.position.y);
        return Vector2.Distance(v1,v2);
    }

//isIllegalMove()
//Compares adjacent coordinates to wallMaster. If any coordinates are a wall, the move is illegal. 
    private bool isIllegalMove(float x, float y){
        foreach(var w in wallMaster){
            if(x == w[0] && y == w[1]){
                return true;
            }
        }
        return false;
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
            if(isIllegalMove(adjX, adjY)){
                continue;
            }
            else{ 
                adjList.Add(new List<float>{adjX, adjY}); 
            }
           
        }
        return adjList;
    }

    void Update(){
        /*
        var x = getAdjacentMoves(player);
        foreach (List<float> i in x){
            Debug.Log("Adjacent x coord: " + i[0]);
            Debug.Log("Adjacent y coord: " + i[1]);
        }
        */
    }
}

/*                var t = Task.Run(async delegate
              {
                 await Task.Delay(1000);
                 if(walls == null){ Debug.Log("NULL!"); }
                 else{
                     foreach(var w in wallMaster){
                         Debug.Log(w[0]);
                         Debug.Log(w[1]);
                     }
                 }
              });
*/