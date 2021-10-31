using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// follows a target
public class CopyTransform : MonoBehaviour
{
    // follows the target, copying its position and rotation.
    public GameObject target;

    // have the camera follow.
    public bool follow = true;

    // movement of the attached object.
    [Header("Transformations")]

    // follow the position.
    public bool copyTranslation = true;

    // the last position of the target
    private Vector3 lastTargetPos;

    // follow the rotation.
    public bool copyRotation = true;

    // the last rotation of the target
    private Vector3 lastTargetEulers;

    // follow the target's scale.
    public bool copyScaling = true;

    // most recent target scale.
    private Vector3 lastTargetScale;

    // TODO: make limits 
    // [Header("Limits")]
    // bool useX

    // Start is called before the first frame update
    void Start()
    {
        // target set.
        if(target != null)
        {
            lastTargetPos = target.transform.position;
            lastTargetEulers = target.transform.eulerAngles;
            lastTargetScale = transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // should not follow target.
        if (!follow)
            return;

        if(target == null)
        {
            Debug.LogAssertion("No target set.");
            return;
        }

        // gets target difference between frames.
        Vector3 tlate = target.transform.position - lastTargetPos;
        Vector3 rot = target.transform.eulerAngles - lastTargetEulers;
        
        Vector3 scaling = new Vector3(
            target.transform.localScale.x / lastTargetScale.x,
            target.transform.localScale.y / lastTargetScale.y,
            target.transform.localScale.z / lastTargetScale.z
            );

        // translates, rotates, then scales
        if (copyTranslation)
            transform.Translate(tlate);
        
        if(copyRotation)
            transform.Rotate(rot);

        if (copyScaling)
            transform.localScale = Vector3.Scale(transform.localScale, scaling);

        // gets position, euler, and scale
        lastTargetPos = target.transform.position;
        lastTargetEulers = target.transform.eulerAngles;
        lastTargetScale = target.transform.localScale;
    }
}
