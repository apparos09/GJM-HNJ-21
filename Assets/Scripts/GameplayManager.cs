using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // the player
    public Player player;

    [Header("Water")]
    // the water's surface
    public float waterSurfacePosY = 0.0F;

    // if 'true', the surface of the water is set using the sea surface object on start (if it is available)
    public bool setSurfacePosUsingObject = true;

    // the depth text (TODO: initialize in Start() if not set)
    public TMPro.TextMeshProUGUI depthText;

    // the timer
    [Header("Timer")]
    // gets the game timer
    public float timer = 100.0F;

    // the start of the timer.
    public float timerStart = 100.0F;

    // the timer is active.
    public bool pausedTimer = false;

    // the timer text (TODO: initialize in Start() if not set)
    public TMPro.TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        // grabs game info.
        GameInfo gi = FindObjectOfType<GameInfo>();

        // the game info object exists.
        if(gi != null)
        {
            timerStart = gi.gameLength;
            Destroy(gi);
        }

        // looks for depth text
        // if(depthText == null)
        // {
        // 
        // }

        // gets player.
        if (player == null)
            player = FindObjectOfType<Player>();

        // sets the timer start.
        timer = timerStart;
    }

    // ends the game
    public void GameOver()
    {
        // creates an info object.
        object infoPrefab = Resources.Load("Prefabs/Game Info");
        GameObject infoObject; // game object.
        GameInfo gi; // info

        // checks if prefab was found.
        if(infoPrefab == null)
        {
            // makes new object.
            infoObject = new GameObject("Game Info");
        }
        else
        {
            infoObject = Instantiate((GameObject)infoPrefab);
        }

        // gets the game info prefab.
        gi = infoObject.GetComponent<GameInfo>();

        // adds the info component.
        if (gi == null)
            gi = infoObject.AddComponent<GameInfo>();

        // set values
        gi.finalScore = player.score;
        gi.gameLength = timerStart;
    }

    // quits the game
    public void GameQuit()
    {

    }

    // gets the player's distance from the surface.
    public float GetPlayerDistanceBelowSurface()
    {
        return waterSurfacePosY - player.transform.position.y;
    }

    // checks to see if the player is at or above the water's surface.
    public bool PlayerAtOrAboveSurface()
    {
        // gets player's height
        float depth = GetPlayerDistanceBelowSurface();

        // check value.
        if (depth <= 0.0F)
            return true;
        else
            return false;
    }

    // Update is called once per frame
    void Update()
    {
        // reduces the timer
        if(!pausedTimer)
        {
            timer -= Time.deltaTime;
            timer = (timer < 0.0F) ? 0.0F : timer;

            // updates timer text.
            if (timerText != null)
                timerText.text = timer.ToString("F3");

            // changes the scene.
            if(timer <= 0)
                SceneHelper.ChangeScene("EndScene");
        }

        // updates depth text
        if(depthText != null)
        {
            string str = "";
            float pDepth = GetPlayerDistanceBelowSurface();
            str = (pDepth / 10.0F).ToString("F3"); // format
            // str = pDepth.ToString("F3");
            str += " m"; // add unit
            depthText.text = str;
        }

        // catches the fish.
        if (PlayerAtOrAboveSurface())
        {
            Debug.Log("Above Water");
            player.CatchFish();
        }
            
            
    }
}
