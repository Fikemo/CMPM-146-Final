using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    private float moveSpeed = 5f;
    public Transform movePoint;
    public Animator animator;
    public Collider2D coll;
    private float saveHoriz = 0f;
    private float saveVert = 0f;
    void Start() {
        movePoint.parent = null;
    }

    void Update()
    {
         
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift)) {
              moveSpeed = 2f;
              Debug.Log("Hello!");
              Debug.Log(moveSpeed);
          }
          else {
              moveSpeed = 5f;
              Debug.Log(moveSpeed);
          }
          
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f) {
            if (Input.GetAxisRaw("Horizontal") == 1f) {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") + .6f, 0f, 0f);
                saveHoriz = -1.6f;
                saveVert = 0f;
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
            else if (Input.GetAxisRaw("Horizontal") == -1f) {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") + -.6f, 0f, 0f);
                saveHoriz = 1.6f;
                saveVert = 0f;
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
            else if (Input.GetAxisRaw("Vertical") == 1f) {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") + .6f, 0f);
                saveHoriz = 0;
                saveVert = -1.6f;
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
            else if (Input.GetAxisRaw("Vertical") == -1f) {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") + -.6f, 0f);
                saveHoriz = 0;
                saveVert = 1.6f;
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.name == "Wall(Clone)") {
            movePoint.position += new Vector3(saveHoriz, saveVert, 0f);
        }
    }
}
