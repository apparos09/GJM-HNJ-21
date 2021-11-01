using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the script for the fish.
public class Fish : MonoBehaviour
{
    // the amount of fish in the scene.
    static int fishCount = 0;

    // the game manager.
    public GameplayManager gameManager;

    // the fish's sprite.
    public SpriteRenderer sprite;

    // the fish's rigidbody.
    public Rigidbody2D rigidBody;

    // the fish's collider.
    public Collider2D bodyCol;

    // movement
    [Header("Movement")]

    // the swimming direction of the fish.
    public Vector2 swimDirec = Vector2.right;

    // the speed that the fish swims at.
    public float swimSpeed = 5.0F;

    // the maximum swim speed
    public float swimSpeedMax = 7.5F;

    // used to change the direction change timer
    public float direcChangeTimer = 0.0F;

    // start time for changing direction.
    public float direcChangeStartTime = 45.0F;

    // hook variables
    [Header("Hook")]

    // the parent transformation of the fish.
    private Player hook;

    // the copy transformation (because parenting didn't work for ome reason).
    public CopyTransform ctform;

    // the points for getting this fish.
    public float worth = 10;

    // updates the fish count when the script is made.
    private void Awake()
    {
        // increases the fish count (maybe move to start)?
        fishCount++;
    }

    // Start is called before the first frame update
    void Start()
    {
        // gets game manager
        if (gameManager == null)
            gameManager = FindObjectOfType<GameplayManager>();

        // gets the sprite renderer
        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();

        // gets the rigidbody.
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody2D>();

        // gets the collider.
        if (bodyCol == null)
            bodyCol = GetComponent<Collider2D>();

        // the ct form
        if (ctform == null)
            ctform = GetComponent<CopyTransform>();
    }

    // on collision 2d enter
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collided with player
        if (collision.gameObject.tag == "Player")
        {
            // gets the fish component.
            Player player = collision.gameObject.GetComponent<Player>();

            // already on hook, so do nothing/
            // TODO: make more efficient.
            if (player == hook)
                return;

            // hook is tangible.
            if (player.IsTangible())
            {
                // checks to see if the hook has room for this fish.
                bool added = player.AttachFish(this);

                // fish added to hook.
                if (added)
                {
                    Hooked(player);
                }

                // save player.
                hook = player;
            }
        }

        // if colliding with the bounds
        if(collision.gameObject.tag == "Bounds")
        {
            // push away from obstruction.
            rigidBody.AddForce(-swimDirec * swimDirec.magnitude * Time.deltaTime, ForceMode2D.Impulse);

            swimDirec = -swimDirec;

            // flip sprite.
            sprite.flipX = (swimDirec.x < 0.0F) ? false : true;

            // // TODO: may not be needed.
            // // random number
            // int rand = Random.Range(0, 2);
            // 
            // // change swim direction
            // switch(rand)
            // {
            //     default:
            //     case 0: // negative rotation
            //         swimDirec = GameplayPhysics.RotateEuler(swimDirec, -120.0F, true);
            //         break;
            // 
            //     case 1: // positive rotation.
            //         swimDirec = GameplayPhysics.RotateEuler(swimDirec, 120.0F, true);
            //         break;
            // }
        }
    }


    // on trigger enter 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // collided with player
        if(collision.tag == "Player")
        {
            // gets the fish component.
            Player player = collision.GetComponent<Player>();

            // already on hook, so do nothing/
            // TODO: make more efficient.
            if (player == hook)
                return;

            // hook is tangible.
            if (player.IsTangible())
            {
                // checks to see if the hook has room for this fish.
                bool added = player.AttachFish(this);

                // fish added to hook.
                if (added)
                {
                    Hooked(player);
                }

                // save player.
                hook = player;
            }

            
        }
    }

    // the fish count.
    public static int FishCount
    {
        get 
        { 
            return fishCount;
        }
    }

    // called when a fish is hooked.
    public void Hooked(Player player)
    {
        // the hook position the fish latches onto
        Vector3 hookPos;

        // sets fish position.
        if (player.hook == null)
            hookPos = player.transform.position;
        else
            hookPos = player.hook.transform.position;

        // set with hook position.
        transform.position = hookPos;

        // sets target and follower
        ctform.SetTarget(player.gameObject);
        ctform.copy = true;

        // rotates fish to be 90 degrees.
        transform.rotation = Quaternion.identity;

        // if the sprite is flipped, rotate the other direction.
        if(sprite.flipX)
            transform.Rotate(Vector3.forward, 90.0F);
        else
            transform.Rotate(Vector3.forward, -90.0F);

        // physics
        bodyCol.isTrigger = true; // change collision to trigger
        rigidBody.velocity = Vector3.zero;
    }

    // called when the fish becomes unhooked from the fishing rod. This causes the fish to let go.
    // TODO: might just destroy the fish.
    public void UnHook()
    {
        // removes the target.
        hook = null;
        ctform.RemoveTarget();
        ctform.copy = false;
        transform.rotation = Quaternion.identity;
        bodyCol.isTrigger = false; // change collision to trigger
    }

    // if on the hook, the original parent is not set as the current parent.
    public bool OnHook()
    {
        return hook != null;
    }

    // called to make the fish swim.
    public void Swim()
    {
        // if the game manager is paused, don't update anything.
        if (gameManager.IsPaused())
            return;

        // reduce timer
        direcChangeTimer -= Time.deltaTime;

        // end of timer
        if (direcChangeTimer <= 0.0F)
        {
            direcChangeTimer = 0.0F;
            
            // face right by default.
            swimDirec = Vector2.right;

            // random orientation
            swimDirec = GameplayPhysics.RotateEuler(swimDirec, Random.Range(0.0f, 360.0F), true);

            // flip sprite.
            sprite.flipX = (swimDirec.x < 0.0F) ? false : true;
        }

        // cap the velocity.
        if(rigidBody.velocity.magnitude >= swimSpeedMax)
        {
            Vector3 newVel = rigidBody.velocity;
            newVel.Normalize();
            newVel *= swimSpeedMax;

            rigidBody.velocity = newVel;
        }

        // add force
        rigidBody.AddForce(swimDirec * swimSpeed * Time.deltaTime, ForceMode2D.Impulse);

        // reset timer.
        direcChangeTimer = direcChangeStartTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("On Hook: " + OnHook());
        
        // if not on the hook, swim.
        if(!OnHook())
            Swim();

        // if the fish is at the sea's surface, the fish's direction is reversed, and it applies force to make them go down.
        if(transform.position.y >= gameManager.waterSurfacePosY)
        {
            // reverses direction and applies force
            swimDirec = GameplayPhysics.RotateEuler(swimDirec, 180.0F, true);

            // flip sprite.
            sprite.flipX = (swimDirec.x < 0.0F) ? false : true;

            rigidBody.AddForce(Vector2.down * swimSpeedMax * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    // decreases the fish count.
    private void OnDestroy()
    {
        fishCount--;
    }
}
