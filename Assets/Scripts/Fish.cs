using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the script for the fish.
public class Fish : MonoBehaviour
{
    // the amount of fish in the scene.
    static int fishCount = 0;

    // the fishe's collider.
    public Collider2D bodyCol;

    // the parent transformation of the fish.
    private Player hook;

    // the copy transformation (because parenting didn't work for ome reason).
    public CopyTransform ctform;

    // the points for getting this fish.
    public float worth = 10;

    // the swimming direction of the fish.
    public Vector2 swimDirec = Vector2.right;

    // updates the fish count when the script is made.
    private void Awake()
    {
        // increases the fish count (maybe move to start)?
        fishCount++;
    }

    // Start is called before the first frame update
    void Start()
    {
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
        transform.Rotate(Vector3.forward, 90.0F);
        bodyCol.isTrigger = true; // change collision to trigger
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

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("On Hook: " + OnHook());
    }

    // decreases the fish count.
    private void OnDestroy()
    {
        fishCount--;
    }
}
