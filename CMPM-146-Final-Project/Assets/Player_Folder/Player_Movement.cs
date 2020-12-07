using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public Animator animator;

    void Start() {
        movePoint.parent = null;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= .05f) {
            if (Input.GetAxisRaw("Horizontal") == 1f) {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") + .6f, 0f, 0f);
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
            if (Input.GetAxisRaw("Horizontal") == -1f) {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") + -.6f, 0f, 0f);
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
            if (Input.GetAxisRaw("Vertical") == 1f) {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") + .6f, 0f);
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
            if (Input.GetAxisRaw("Vertical") == -1f) {
                movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") + -.6f, 0f);
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);
            }
        }

    }
}
