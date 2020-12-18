using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class mainMenu : MonoBehaviour
{
    public GameObject mainCanvas;

    public GameObject optionsCanvas;

    public GameObject inputField;

    public static string optionString = "14";
    public static string conversionString; 
    public static int optionInt = 14;

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
        optionString = inputField.GetComponent<TextMeshProUGUI>().text.ToString();
        if (optionString.Length == 2) {
            conversionString = conversionString + optionString[0];
        }
        else if (optionString.Length == 3) {
            conversionString = conversionString + optionString[0];
            conversionString = conversionString + optionString[1];
        }
        optionInt = int.Parse(conversionString);
        mainCanvas.SetActive(true);
        optionsCanvas.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
