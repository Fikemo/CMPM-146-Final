using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject mainCanvas;

    public GameObject optionsCanvas;

    // Start is called before the first frame update
    void Start() {
        optionsCanvas.SetActive(false);
    }

    public void PlayGame () {
        SceneManager.LoadScene("GameScene");
    }

    public void OptionsMenu() {
        mainCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void finishedOptions() {
        mainCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
