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

    // the depth text (TODO: initialize in Start() if not set)
    public TMPro.TextMeshProUGUI depthText;

    // the speed at which an object will stop.
    private static Vector2 stopSpeed = new Vector3(0.0001F, 0.0001F);
    
    // the drag applied when going through water.
    private static Vector2 waterDrag = new Vector2(0.985F, 1.0F);

    // the timer
    [Header("Timer")]
    // gets the game timer
    public float timer = 100.0F;

    // the timer is active.
    public bool pausedTimer = false;

    // the timer text (TODO: initialize in Start() if not set)
    public TMPro.TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        // looks for depth text
        // if(depthText == null)
        // {
        // 
        // }

        // gets player.
        if (player == null)
            player = FindObjectOfType<Player>();
    }

    // returns the stop speed.
    public static Vector2 StopSpeed
    {
        get { return stopSpeed; }
    }
    
    // gets the drag factor
    public static Vector2 WaterDrag 
    {
        get { return waterDrag; }
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
            // str = (pDepth / 100.0F).ToString("F2"); // format
            str = pDepth.ToString("F3");
            // str += " m"; // add unit
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
