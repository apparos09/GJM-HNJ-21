using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the script for the fish.
public class Fish : MonoBehaviour
{
    // the parent transformation of the fish.
    private Player hook;

    // the copy transformation (because parenting didn't work for ome reason).
    public CopyTransform ctform;

    // Start is called before the first frame update
    void Start()
    {
        // the ct form
        if (ctform == null)
            ctform = GetComponent<CopyTransform>();
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

            // checks to see if the hook has room for this fish.
            bool added = player.AddFish(this);

            // fish added to hook.
            if(added)
            {
                Hooked(player);
            }

            // save player.
            hook = player;
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
        // transform.rotation = Quaternion.identity;
        // transform.Rotate(Vector3.forward, 90.0F);
    }

    // if on the hook, the original parent is not set as the current parent.
    public bool OnHook()
    {
        return hook != null;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("On Hook: " + OnHook());
    }
}
