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
        Invoke("findPlayer", 0.11f);
        StartCoroutine(startPathfind());

    }

    IEnumerator startPathfind(){
        yield return new WaitForSeconds(5);
        List<List<float>> x = new List<List<float>>();
        x = OptimalPathFinder(player.transform.position.x, player.transform.position.y, usbKey.transform.position.x, usbKey.transform.position.y);
        
        if(x != null){
            Debug.Log("X list is populated");
        }
        else{
            Debug.Log("Uh oh");
        }

        foreach(List<float> i in x){
            Vector3 coord = new Vector3(i[0], i[1], 0f);
            Debug.Log(i[0]+" is x coordinate");
            Debug.Log(i[1]+" is y coordinate");
            Vector3.MoveTowards(transform.position, coord, 2f*Time.deltaTime);
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
        return false;
    }

//getAdjacentMoves()
//OUTPUT: List of lists storing floats: an x coordinate of an adjacent space, and a y coordinate of an adjacent space.
//Accounts for north/south/east/west movement. Need to consider removing coordinates of walls and enemies. 
    public List<List<float>> getAdjacentMoves(float pointX, float pointY){
        var adjList = new List<List<float>>();
        var dirs = new List<List<float>>();

        dirs.Add(new List<float>{ 1.6f, 0f });
        dirs.Add(new List<float>{ -1.6f, 0f });
        dirs.Add(new List<float> { 0f, 1.6f });
        dirs.Add(new List<float> { 0f, -1.6f });

        float adjX;
        float adjY;

        foreach (var dir in dirs){
            adjX = pointX + dir[0];
            adjY = pointY + dir[1];
            if(isIllegalMove(adjX, adjY)){
                continue;
            }
            else{ 
                adjList.Add(new List<float>{adjX, adjY}); 
            }
           
        }
        return adjList;
    }


    public List<List<float>> OptimalPathFinder(float srcX, float srcY, float destX, float destY){
        Debug.Log("entered program");

        //HeapQ consists of:
        //Key -> Cost so far
        //Value -> [x,y] coordinates
        //The initial position's cost and coordinates are put in at the start.
        SortedList<float, List<float>> heapQ = new SortedList<float, List<float>>();
        heapQ.Add(0f, new List<float>{ srcX, srcY });

        //distances consists of:
        //Key -> [x,y] coordinates
        //Value -> recorded distance
        Dictionary<List<float>, float> distances = new Dictionary<List<float>, float>();
        distances[new List<float>{srcX, srcY}] = 0f;

        //came_from consists of:
        // Key -> coordinates of child
        // Value -> coordinates of parent (null if no parent)
        Dictionary<List<float>, List<float>> came_from = new Dictionary<List<float>, List<float>>();
        came_from[new List<float> {srcX, srcY}] = null;

        //Initialize variables for later usage
        float curr_distance = 0f;
        float adjCost = 0f;
        float pathCost = 0f;
        bool newAdj = true;
        bool betterCost = false;

        List<float> curr_coords = new List<float>();

        //While the heapQ is not empty,
        //Assign curr_distance to the sortedArray's first element's key,
        //And the respective value stored. 
        while(heapQ.Count != 0){
            Debug.Log(heapQ.Count);
            foreach( KeyValuePair<float, List<float>> kvp in heapQ){
                curr_distance = kvp.Key;
                curr_coords = kvp.Value;
                break;
            }

            //Removes first key+ element in the list to simulate a "pop". 
            heapQ.RemoveAt(0);

            //If the current coordinates are equal to the destination coords,
            //Add our current coords to the path list. Then, iteratively append parents until null.
            //Reverse the list, then the coordinates should start from the beginning and proceed till the end.
            if(curr_coords[0] == destX && curr_coords[1] == destY){
                List<List<float>> path = new List<List<float>>();
                path.Add(curr_coords);
                List<float> current_parent = came_from[curr_coords];
                while(current_parent != null){
                    path.Add(current_parent);
                    current_parent = came_from[current_parent];
                }
                path.Reverse();
                return path;
            }
            //for each adjacent move from the current_coords,
            //calculate distance to destination, and add to existing distance to get pathCost.
            //Check to see if coords already exist, or if the current pathcost is less.
            //If this is the case, trigger the appropriate flags.
            //If either flag is true, update distance and parent for the adjacent move. 
            //Finally, push the pathCost and adjMove onto the heapQ.
            
            foreach(List<float> adjMove in getAdjacentMoves(curr_coords[0], curr_coords[1])){
                newAdj = true;
                betterCost = false;
                //double check later
                adjCost = getDistance(adjMove[0], adjMove[1], destX, destY);
                pathCost = curr_distance + adjCost;


                foreach(KeyValuePair<List<float>, float> kvp in distances){
                    if(adjMove[0] == kvp.Key[0] && adjMove[1] == kvp.Key[1]){
                        newAdj = false;
                        if(pathCost < kvp.Value){
                            betterCost = true;
                        }
                        break;
                    }
                }
                //When this line is added, things get thrown for an infinite loop + crashes Unity.
                //Without this line, we get a key error in that there are already existing keys.
                foreach(KeyValuePair<float, List<float>> kvp in heapQ){
                    if(pathCost == kvp.Key){
                        pathCost = pathCost + 0.01f;
                    
                }}
                if(newAdj || betterCost){
                    distances[adjMove] = pathCost;
                    Debug.Log(distances[adjMove]);
                    came_from[adjMove] = curr_coords;
                    heapQ.Add(pathCost, adjMove);
                }
            }
        }
        Debug.Log("Goodbye");
        return null;
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