using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BotMoveAi : MonoBehaviour
{
    public Transform target;
    public float speed = 3f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker; 
    Rigidbody2D rb;
    public GameObject usbKey;
    void Start()
    {
        Invoke("findUsb", 0.01f);
        StartCoroutine(startPathfind());
    }

     IEnumerator startPathfind(){
        yield return new WaitForSeconds(5);
         seeker = GetComponent<Seeker>();
         rb = GetComponent<Rigidbody2D>();
         seeker.StartPath(rb.position, target.position, OnPathComplete);
     }
   private void FindUsb(){
        usbKey = GameObject.Find("Key(Clone)");
    }
    void OnPathComplete(Path p){
        if (!p.error){
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
