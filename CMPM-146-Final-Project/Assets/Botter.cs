using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Botter : MonoBehaviour
{
    GameObject usbKey;
    GameObject computer;
    GameObject exit;
    public float speed = 3f;
    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    bool reachedUsb = false;
    bool reachedComputer = false;

    void Start()
    {
        Invoke("FindUsb", 0.001f);
        Invoke("FindComputer", 0.002f);
        Invoke("FindExit", 0.003f);
        StartCoroutine(startPathfind());
    }
    IEnumerator startPathfind(){
        yield return new WaitForSeconds(1);
         AstarPath.active.Scan();
         seeker = GetComponent<Seeker>();
         rb = GetComponent<Rigidbody2D>();
         InvokeRepeating("UpdatePath", 0f, 0.5f);

        
    }

    void UpdatePath(){
        usbReached();
        computerReached();
        if(!reachedUsb && seeker.IsDone()){
            seeker.StartPath(rb.position, usbKey.transform.position, onPathComplete);
        }
        else if(reachedUsb && !reachedComputer && seeker.IsDone()){
            seeker.StartPath(rb.position, computer.transform.position, onPathComplete);
        }
        else if(reachedUsb && reachedComputer && seeker.IsDone()){
            seeker.StartPath(rb.position, exit.transform.position, onPathComplete);
        }
    }
        

    void usbReached(){
        if(rb.position == (Vector2)usbKey.transform.position){
            reachedUsb = true;
        }
    }
    void computerReached(){
        if(rb.position == (Vector2)computer.transform.position){
            reachedComputer = true;
        }
    }


    void onPathComplete(Path p){
        if(!p.error){
            p = path;
            currentWaypoint = 0;
        }
    }

      void FindUsb(){
        usbKey = GameObject.Find("Key(Clone)");
     }

     void FindComputer(){
         computer = GameObject.Find("Computer(Clone)");
     }
     void FindExit(){
         exit = GameObject.Find("Exit(Clone)");
     }


    // Update is called once per frame
    void FixedUpdate()
    {
        AstarPath.active.Scan();
        if (path == null){
            return;
        }

        if(currentWaypoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }
        else{
            reachedEndOfPath = false;
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance){
            currentWaypoint++;
        }

    }
}
