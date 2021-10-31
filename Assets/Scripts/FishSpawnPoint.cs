using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the fish spawn point.
public class FishSpawnPoint : MonoBehaviour
{
    // offset from object's position
    public Vector3 offset = new Vector3(0.0F, 0.0F, 0.0F);

    // gets the spawn position.
    public Vector3 GetSpawnPosition()
    {
        return transform.position + offset;
    }
}
