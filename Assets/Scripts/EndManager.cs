using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// the ending manager
public class EndManager : MonoBehaviour
{
    // the final score.
    public float finalScore = 0.0F;

    // the text to show the final score
    public TMPro.TextMeshProUGUI scoreText = null;

    // The exit button.
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    {
        // round information.
        GameInfo gameInfo = FindObjectOfType<GameInfo>();

        if (gameInfo != null)
            finalScore = gameInfo.finalScore;

        // text display
        if (scoreText != null)
            scoreText.text = finalScore.ToString("F3");

        // destroys the game object.
        if(gameInfo != null)
            Destroy(gameInfo.gameObject);


        // The exit button is set.
        if (exitButton != null)
        {
            // If in WebGL, disable the button.
            if(Application.platform == RuntimePlatform.WebGLPlayer)
            {
                exitButton.interactable = false;
            }
            // If not in WebGL, enable the button.
            else
            {
                exitButton.interactable = true;
            }
        }

    }

    // returns to the title screen.
    public void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // exits the game
    public void ExitApplication()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        // text display
        if (scoreText != null)
            scoreText.text = finalScore.ToString("F3");
    }
}
