using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement_AI : MonoBehaviour
{
    private float moveSpeed = 5f;
    public Transform movePoint;
    public Animator animator;
    public Collider2D coll;
    public bool AIControlled = false;
    public GameObject levelLoader;
    public List<string> commands;
    private float saveHoriz = 0f;
    private float saveVert = 0f;
    void Start() {
        movePoint.parent = null;
        levelLoader = GameObject.Find("LevelLoader");
        commands = levelLoader.GetComponent<maze>().commands;
    }

    void Update()
    {
         
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = 2f;
        }
        else {
            moveSpeed = 5f;
        }
        AIControlled = true;
        if ((Vector3.Distance(transform.position, movePoint.position)) <= .05f && !AIControlled) {
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

        else if ((Vector3.Distance(transform.position, movePoint.position) <= .05f) && AIControlled && commands.Count > 0)
        {
            string nextStep = commands[0];
            Debug.Log(nextStep);

            if (nextStep == "right")
            {
                movePoint.position += new Vector3(1f + .6f, 0f, 0f);
                saveHoriz = -1.6f;
                saveVert = 0f;
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", 1f);
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);

                commands.RemoveAt(0);
            }
            else if (nextStep == "left")
            {
                movePoint.position += new Vector3(-1f + -.6f, 0f, 0f);
                saveHoriz = 1.6f;
                saveVert = 0f;
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", -1f);
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);

                commands.RemoveAt(0);
            }
            else if (nextStep == "up")
            {
                movePoint.position += new Vector3(0f, 1f + .6f, 0f);
                saveHoriz = 0;
                saveVert = -1.6f;
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", 1f);
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);

                commands.RemoveAt(0);
            }
            else if (nextStep == "down")
            {
                movePoint.position += new Vector3(0f, -1f + -.6f, 0f);
                saveHoriz = 0;
                saveVert = 1.6f;
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", -1f);
                animator.SetFloat("Speed", movePoint.position.sqrMagnitude);

                commands.RemoveAt(0);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.name == "Wall(Clone)") {
            movePoint.position += new Vector3(saveHoriz, saveVert, 0f);
        }
        if (coll.gameObject.name == "Enemy(Clone)") {
            SceneManager.LoadScene("DeathScene");
        }
        if (coll.gameObject.name == "Exit(Clone)" && ComputerOpenExit.haveData) {
            SceneManager.LoadScene("EndScene");
        }
    }
}
