using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player
public class Player : MonoBehaviour
{
    // player's rigid body.
    public Rigidbody2D rigidBody;
    
    // force and speed cap
    public Vector2 force = new Vector2(1.0F, 1.0F);
    public Vector2 speedLimit = new Vector2(5.0F, 5.0F);

    // TODO: setup water resistance.

    // Start is called before the first frame update
    void Start()
    {
        // gets rigid body.
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // gets the directions of movement.
        float xDirec = Input.GetAxisRaw("Horizontal");
        float yDirec = Input.GetAxisRaw("Vertical");

        // horizontal movement
        if (xDirec != 0.0F)
        {
            rigidBody.AddForce(transform.right * (force.x * xDirec * Time.deltaTime), ForceMode2D.Impulse);
        }

        // vertical movement
        if (yDirec != 0.0F)
        {
            rigidBody.AddForce(transform.up * (force.y * yDirec * Time.deltaTime), ForceMode2D.Impulse);
        }

        // final velocity.
        Vector2 finalVel = rigidBody.velocity;

        // clamps speed on x
        if (Mathf.Abs(rigidBody.velocity.x) > speedLimit.x)
            finalVel.x = Mathf.Clamp(rigidBody.velocity.x, -speedLimit.x, speedLimit.x);

        // clamps speed on y
        if (Mathf.Abs(rigidBody.velocity.y) > speedLimit.y)
            finalVel.y = Mathf.Clamp(rigidBody.velocity.y, -speedLimit.y, speedLimit.y);

        // sets final velocity.
        rigidBody.velocity = finalVel;
    }

    // fixed update
    private void FixedUpdate()
    {
        // grabs current velocity
        Vector2 newVel = rigidBody.velocity;

        // applies drag factor
        newVel.Scale(GameplayManager.WaterDrag * Time.fixedDeltaTime);

        // sets new velocity.
        rigidBody.velocity = newVel;

    }
}
