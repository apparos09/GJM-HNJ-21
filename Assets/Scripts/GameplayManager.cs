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

    // game objects representing the size of the stage.
    public GameObject seaLeftWall; // sea wall on the left
    public GameObject seaRightWall; // sea wall on the right 
    public GameObject seaBottom; // sea bottom

    // the top of the sky, which stops the player when pulling up fish.
    public GameObject skyTop;


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

    // ROTATIONS //

    // euler rotation (2D)
    public static Vector2 RotateEuler(Vector2 v, float angle, bool inDegrees)
    {
        // re-uses rotation calculation for 3D with z = 0.
        Vector3 result = RotateEuler(new Vector3(v.x, v.y, 0.0F), Vector3.forward, angle, inDegrees);
        return new Vector2(result.x, result.y);
    }

    // euler rotation (3D)
    private static Vector3 RotateEuler(Vector3 v, Vector3 axis, float angle, bool inDegrees)
    {
        // angle conversion
        float radAngle = (inDegrees) ? Mathf.Deg2Rad * angle : angle;

        // sin and cos angle
        float sinAngle = Mathf.Sin(radAngle);
        float cosAngle = Mathf.Cos(radAngle);

        // the three rows (set to identity by default)
        Vector3 r0 = Vector3.one;
        Vector3 r1 = Vector3.one;
        Vector3 r2 = Vector3.one;

        // set rotation values
        if (axis == Vector3.right) // x-axis
        {
            r0 = new Vector3(1.0F, 0.0F, 0.0F);
            r1 = new Vector3(0.0f, cosAngle, -sinAngle);
            r2 = new Vector3(0.0F, sinAngle, cosAngle);
        }
        else if (axis == Vector3.up) // y-axis
        {
            r0 = new Vector3(cosAngle, 0.0F, sinAngle);
            r1 = new Vector3(0.0f, 1.0F, 0.0F);
            r2 = new Vector3(-sinAngle, 0.0F, cosAngle);
        }
        else if (axis == Vector3.forward) // z-axis
        {
            r0 = new Vector3(cosAngle, -sinAngle, 0.0F);
            r1 = new Vector3(sinAngle, cosAngle, 0.0F);
            r2 = new Vector3(0.0F, 0.0F, 1.0F);
        }

        // calculation (modelled after matrix multiplication)
        Vector3 result = Vector3.zero;
        result.x = Vector3.Dot(r0, v);
        result.y = Vector3.Dot(r1, v);
        result.z = Vector3.Dot(r2, v);

        return result;
    }

    // rotates along the x-axis
    public static Vector3 RotateEulerX(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.right, angle, inDegrees);
    }

    // rotates along the y-axis
    public static Vector3 RotateEulerY(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.up, angle, inDegrees);
    }

    // rotates along the z-axis
    public static Vector3 RotateEulerZ(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.forward, angle, inDegrees);
    }

    // //

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
