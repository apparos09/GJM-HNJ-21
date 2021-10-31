using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player
public class Player : MonoBehaviour
{
    // the sprite when tangible
    public SpriteRenderer tangibleSprite;

    // the sprite when intangible.
    public SpriteRenderer intangibleSprite;

    // player's rigid body.
    public Rigidbody2D rigidBody;

    // the player's collider.
    public BoxCollider2D boxCol;

    // the player's score
    public float score = 0.0f;

    // returns 'true' when tangible.
    private bool tangible = true;

    // checks to see if the player can phase through things.
    public bool canPhase = true;

    // force and speed cap
    public Vector2 force = new Vector2(1.0F, 1.0F);
    public Vector2 speedLimit = new Vector2(5.0F, 5.0F);

    // TODO: setup water resistance.

    [Header("Fish")]
    // the hook that the fish attaches to. Used for visual connection
    public GameObject hook;

    // list of fishes the player has caught.
    public List<Fish> fishes = new List<Fish>();

    // amount of fish allowed on the hook.
    public int fishLimit = 5;

    // Start is called before the first frame update
    void Start()
    {
        // get sprites
        if(tangibleSprite == null || intangibleSprite == null)
        {
            // gets sprite components in children.
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

            // set as sprite 1
            if (tangibleSprite == null  && sprites.Length >= 1)
                tangibleSprite = sprites[0];

            // set as sprite 2
            if (intangibleSprite == null && sprites.Length >= 2)
                intangibleSprite = sprites[1];

            // re-use sprite 1 is sprite 2 does not exist.
            if (intangibleSprite == null)
                intangibleSprite = tangibleSprite;
        }

        // gets rigid body.
        if (rigidBody == null)
            rigidBody = GetComponent<Rigidbody2D>();

        // gets the box collider.
        if (boxCol == null)
            boxCol = GetComponent<BoxCollider2D>();

        // TODO: setup
        //if(hook == null)


        // object is tangible.
        MakeTangible();
    }

    // on collision enter 2D (used if tangible)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Stage" || collision.gameObject.tag == "Bounds")
        {
            // hit a stage bounds
            if (collision.gameObject.tag == "Bounds")
            {
                MakeTangible(); // may not be needed.
                canPhase = false;
            }

            // release the fish.
            if (fishes.Count > 0)
                ReleaseFishes();
        }
    }

    // on collision exit 2D (used if tangible)
    private void OnCollisionExit2D(Collision2D collision)
    {
        // left a stage bounds
        if (collision.gameObject.tag == "Bounds")
        {
            canPhase = true;
        }
    }

    // on trigger enter 2D (used if intangible) (can only collide with bounds)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // this is part of the stage.
        if(collision.tag == "Bounds")
        {
            // makes solid.
            boxCol.isTrigger = false;
            MakeTangible();
            canPhase = false;

            // releaes the fishes if any are on the hook.
            if (fishes.Count > 0)
                ReleaseFishes();
        }
    }

    // checks to see if the player is tangible.
    public bool IsTangible()
    {
        return tangible;
    }

    // makes the object tangible.
    public void MakeTangible()
    {
        // changes sprites
        tangibleSprite.enabled = true;
        intangibleSprite.enabled = false;
        tangible = true;

        // changes collider
        boxCol.isTrigger = false;
    }

    // checks to see if the player is intangible.
    public bool IsIntangible()
    {
        return !tangible;
    }

    // makes the object intangible.
    public void MakeIntangible()
    {
        // changes sprites
        tangibleSprite.enabled = false;
        intangibleSprite.enabled = true;
        tangible = false;
        
        // changes collider
        boxCol.isTrigger = true;

        // releases fishes on the hook.
        if (fishes.Count > 0)
            ReleaseFishes();

    }

    // adds a fish to the player's list if it's on the hook.
    // if it has too many fish, it returns false.
    public bool AttachFish(Fish fish)
    {
        // adds a fish.
        if(fishes.Count < fishLimit)
        {
            fishes.Add(fish);
            return true;
        }

        return false;
    }

    // catches the fish on the hook.
    public void CatchFish()
    {
        // while there are still fish to catch on the hook.
        while(fishes.Count > 0)
        {
            // gets the fish nad removes it.
            Fish fish = fishes[0];
            fishes.RemoveAt(0);

            // updates score
            if (fish != null)
            {
                score += fish.worth;
                Destroy(fish.gameObject);
            }
        }
    }

    // releashes all the fish in the player's list.
    public void ReleaseFishes()
    {
        // unhooks the fishes and clears the list.
        foreach(Fish fish in fishes)
        {
            if (fish != null)
                fish.UnHook();
        }

        // removes all.
        fishes.Clear();
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

        // turn on intangibility when space is held down.
        if(canPhase)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MakeIntangible();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                MakeTangible();
            }
        }
        
    }

    // fixed update
    private void FixedUpdate()
    {
        // the drag doesn't get applied for the vertical due to gravity being in effect.

        // grabs current velocity
        Vector2 newVel = rigidBody.velocity;
    
        // applies drag factor
        // newVel.Scale(GameplayManager.WaterDrag * Time.fixedDeltaTime);
    
        // check to see that the player is not moving.
        if(Input.GetAxisRaw("Horizontal") == 0.0F)
        {
            newVel.x *= GameplayManager.WaterDrag.x * Time.fixedDeltaTime;
        }

        // stop if moving too slow
        if (newVel.x == GameplayManager.StopSpeed.x)
            newVel.x = 0.0F;

        // sets new velocity.
        rigidBody.velocity = newVel;
    
    }
}
