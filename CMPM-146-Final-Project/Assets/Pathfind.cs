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
    public List<List<float>> visited = new List<List<float>>();

//Invoke is needed because objects don't exist before compile time (map isn't generated yet!)

    void Start()
    {
        Invoke("FindUsb", 0.001f);
        Invoke("getWalls", 0.01f);
        Invoke("getWallCoords", 0.1f);
        Invoke("findPlayer", 0.11f);
        StartCoroutine(startPathfind());

    }

    IEnumerator startPathfind(){
        yield return new WaitForSeconds(5);
        List<float> x = new List<float>();
        x = OptimalPathFinder(player.transform.position.x, player.transform.position.y, usbKey.transform.position.x, usbKey.transform.position.y);
        visited.Add(x);

        if(x != null){
            Debug.Log("X list is populated");
        }
        else{
            Debug.Log("Uh oh");
        }

        bool destFound = false;

        while(!destFound){
            if(x[0] == usbKey.transform.position.x && x[1] == usbKey.transform.position.y){
                destFound = true;
            }
            transform.position = new Vector3(x[0], x[1], 0f);
            x = OptimalPathFinder(x[0],x[1],usbKey.transform.position.x,usbKey.transform.position.y);
            Debug.Log(x[0]+" is x");
            Debug.Log(x[1]+" is y");
            if(x[0] <= 0 && x[1] <= 0){ Debug.Log("Null valu"); }
            if(x == null){
                visited.Clear();
                x = OptimalPathFinder(x[0],x[1],usbKey.transform.position.x,usbKey.transform.position.y);
            }
            visited.Add(x);
        }
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

    private void findPlayer(){
        player = GameObject.Find("Player (1)");
    }

//getDistance()
//Calculates distance from one point to another; does not account for obstacles, but instead calculates distance from src to dest
/*
    private float getDistance(GameObject source, GameObject dest){
        Vector2 v1 = new Vector2(source.transform.position.x, source.transform.position.y);
        Vector2 v2 = new Vector2(dest.transform.position.x, dest.transform.position.y);
        return Vector2.Distance(v1,v2);
    } */

    private float getDistance(float xOne, float yOne, float xTwo, float yTwo){
        //Check that this function is working
        Vector2 v1 = new Vector2(xOne, yOne);
        Vector2 v2 = new Vector2(xTwo, yTwo);
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
        foreach(var z in visited){
            if(x == z[0] && y == z[1]){
                return true;
            }
        }
        return false;
    }

//getAdjacentMoves()
//OUTPUT: List of lists storing floats: an x coordinate of an adjacent space, and a y coordinate of an adjacent space.
//Accounts for north/south/east/west movement. Need to consider removing coordinates of walls and enemies. 
    public List<List<float>> getAdjacentMoves(float pointX, float pointY){
        var adjList = new List<List<float>>();
        var dirs = new List<List<float>>();
        float count = 0;

        dirs.Add(new List<float>{ 1.6f, 0f });
        dirs.Add(new List<float>{ -1.6f, 0f });
        dirs.Add(new List<float> { 0f, 1.6f });
        dirs.Add(new List<float> { 0f, -1.6f });

        float adjX = 0f;
        float adjY = 0f;

        foreach (var dir in dirs){
            adjX = pointX + dir[0];
            adjY = pointY + dir[1];
            if(isIllegalMove(adjX, adjY)){
                count++;
                continue;
            }
            else{ 
                adjList.Add(new List<float>{adjX, adjY}); 
            }
           
        }
        return adjList;
    }


    public List<float> OptimalPathFinder(float srcX, float srcY, float destX, float destY){
        Debug.Log("entered programy");
        float x;
        float min = 9999f;
        List<float> bestAdj = null;
        List<List<float>> adjMoves = getAdjacentMoves(srcX, srcY);
        foreach(List<float> adj in adjMoves){
            x = getDistance(adj[0],adj[1],destX,destY);
            if(x <= min){
                min = x;
                bestAdj = adj; 
            }
        }
        return bestAdj;
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

//pe