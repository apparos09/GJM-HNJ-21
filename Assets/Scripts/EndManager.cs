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
