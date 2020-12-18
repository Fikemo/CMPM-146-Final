using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame () {
        SceneManager.LoadScene("GameScene");
    }

    public void AIGame() {
        SceneManager.LoadScene("TestScene");
    }
    
    public void QuitGame() {
        SceneManager.LoadScene("menuScene");
    }
}
