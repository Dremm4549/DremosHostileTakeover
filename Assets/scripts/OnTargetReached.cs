using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class OnTargetReached : MonoBehaviour
{
    public float threshhold = 0.02f;
    public Transform target;
    public UnityEvent OnReached;
    bool wasReached = false;

    private void FixedUpdate()
    {
        float distance =  Vector3.Distance(transform.position, target.position);

        if(distance < threshhold && !wasReached)
        {
            //reached target
            OnReached.Invoke();
            wasReached = true;
        }
        else
        {
            if(distance >= threshhold)
            {
                wasReached = false;
            }
        }
    }
}
