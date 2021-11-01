using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// follows a target
public class CopyTransform : MonoBehaviour
{
    // follows the target, copying its position and rotation.
    [Tooltip("The target to be copied. If setting target after Start(), use SetTarget().")]
    public GameObject target;

    // have the camera follow.
    public bool copy = true;

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

    // EX: if you have time, make these changes.
    // transformation limits
    // [Header("Limits")]
    // // the lower position limit.
    // public Vector3 posLimitLow = new Vector3();
    // 
    // // the higher position limit.
    // public Vector3 posLimitHigh = new Vector3();




    // Start is called before the first frame update
    void Start()
    {
        // sets the target.
        if(target != null)
            SetTarget(target);
    }
    
    // the new target.
    public void SetTarget(GameObject newTarget)
    {
        // wants to remove target.
        if (newTarget == null)
            RemoveTarget();

        target = newTarget;
        lastTargetPos = target.transform.position;
        lastTargetEulers = target.transform.eulerAngles;
        lastTargetScale = transform.localScale;
    }

    // removes the target
    public void RemoveTarget()
    {
        target = null;
    }

    // Update is called once per frame
    void Update()
    {
        // should not follow target.
        if (!copy)
            return;

        // checks if target to follow.
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
        {
            Quaternion origRot = transform.rotation;
            transform.rotation = Quaternion.identity;
            transform.Translate(tlate);
            transform.rotation = origRot;
        }
        
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
