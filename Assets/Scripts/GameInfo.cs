using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// game info that is transferred between scenes.
// this is instantiated when a round ends, provides values to the next round, and is destroyed once that round begins.
public class GameInfo : MonoBehaviour
{
    // the final score from the game.
    public float finalScore = 0.0F;

    // the game's length
    public float gameLength = 0.0F;

    // Start is called before the first frame update
    void Start()
    {
        // don't destory this object
        DontDestroyOnLoad(this);
    }

    // gets the final score
    public float GetFinalScore()
    {
        return finalScore;
    }

    // sets the final score
    public void SetFinalScore(float score)
    {
        if (score >= 0.0F)
            finalScore = score;
    }

}
