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

    // the speed at which an object will stop.
    private static Vector2 stopSpeed = new Vector3(0.0001F, 0.0001F);
    
    // the drag applied when going through water.
    private static Vector2 waterDrag = new Vector2(0.985F, 1.0F);

    // the itmer
    [Header("Timer")]
    // gets the game timer
    public float timer = 100.0F;

    // the timer is active.
    public bool pausedTimer = false;

    // Start is called before the first frame update
    void Start()
    {
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
        }

        if (PlayerAtOrAboveSurface())
            Debug.Log("Above Water");
            
    }
}
