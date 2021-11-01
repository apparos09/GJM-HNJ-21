using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// spawns fish
public class FishSpawner : MonoBehaviour
{
    // the game manager.
    public GameplayManager gameManager;

    // the amount of fish allowed 
    public int fishLimit = 30;

    // if true, the spawner lisetens to the fish limit.
    public bool limiterOn = true;

    // adds fish spawn points.
    public bool addSpawnsOnStart = true;

    // timer for spawning new fish
    [Header("Timer")]

    // the cool down time for spawning a new fish.
    public float cooldownTimer = 0.0F;

    // the start time for the spawn timer's cool down.
    public float cooldownStartTime = 2.50F;

    // if 'true', the cool down spaces out spawns.
    // if false, fishes are spawned instantly.
    public bool useCooldown = true;

    // lists for the spawner
    [Header("Lists")]

    // the list of fishes that can be instantiated.
    public List<GameObject> fishPrefabs = new List<GameObject>();

    // spawn positions for fish
    public List<Vector3> spawnPositions = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        // finds the game manager if it's not set.
        if (gameManager == null)
            gameManager = FindObjectOfType<GameplayManager>();

        // if spawns should be aded on the start.
        if (addSpawnsOnStart)
        {
            // finds all spawn points.
            FishSpawnPoint[] spawns = FindObjectsOfType<FishSpawnPoint>();

            // adds spawn points.
            foreach (FishSpawnPoint spawn in spawns)
                spawnPositions.Add(spawn.GetSpawnPosition());
        }

        // if there are no spawn points, add the world origin as a location.
        if (spawnPositions.Count == 0)
            spawnPositions.Add(new Vector3());
    }

    // adds the fish spawn points.
    public void AddFishSpawnPoint(FishSpawnPoint spawn)
    {
        spawnPositions.Add(spawn.GetSpawnPosition());
    }

    // adds the fish spawn points.
    public void AddFishSpawnPoint(Vector3 spawn)
    {
        spawnPositions.Add(spawn);
    }

    // Update is called once per frame
    void Update()
    {
        // if the game manager is paused, don't update anything.
        if (gameManager.IsPaused())
            return;

        // if the cooldown should be used.
        if (useCooldown && cooldownTimer > 0.0F)
        {
            // reduce timer
            cooldownTimer -= Time.deltaTime;

            // timer at 0
            if (cooldownTimer < 0.0F)
                cooldownTimer = 0.0F;

            // cannot spawn new fish yet.
            if (cooldownTimer > 0.0F)
                return;
        }

        // if more fish can be spawned.
        if(Fish.FishCount < fishLimit && fishPrefabs.Count != 0 && spawnPositions.Count != 0)
        {
            // gets indexes for the fish.
            int fishIndex = Random.Range(0, fishPrefabs.Count);

            // gets index for spawn position.
            int spawnIndex = Random.Range(0, spawnPositions.Count);

            // spawn position.
            Vector3 spawnPos = new Vector3();
            spawnPos = spawnPositions[spawnIndex]; 

            // loads the resource
            GameObject fishTemp = fishPrefabs[fishIndex];
            GameObject newObject = null;

            // checks resource
            if (fishTemp != null) // resource loaded successfully
            {
                // creates new fish.
                newObject = Instantiate(fishTemp);

                // sets position
                newObject.transform.position = spawnPos;

                // if the cooldown timer is being used.
                if (useCooldown)
                    cooldownTimer = cooldownStartTime;

            }
            else // load failed
            {
                Debug.LogError("Fish prefab failed to load.");
            }
            
        }
    }
}
