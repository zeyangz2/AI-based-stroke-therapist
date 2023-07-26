using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Obstacle : MonoBehaviour
{
    [Range(0f, 1f)]
    public float ObstacleForceMagnitude;

    public bool IsVirtualHitting;
    public Vector3 ObstacleForceDirection;

    public UnityEvent OnFirstHitWall = new UnityEvent();
    public UnityEvent OnStopHittingWall = new UnityEvent();


    private void OnCollisionEnter(Collision collision)
    {
        
        VirtualDeviceCollider virtualDeviceCollider = collision.gameObject.GetComponent<VirtualDeviceCollider>();
        if(virtualDeviceCollider)
        {
            OnFirstHitWall.Invoke();
            Debug.Log(collision.gameObject.name);
            ObstacleForceDirection = -1*collision.GetContact(0).normal.normalized; 
            IsVirtualHitting = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        VirtualDeviceCollider virtualDeviceCollider = collision.gameObject.GetComponent<VirtualDeviceCollider>();
        if (virtualDeviceCollider)
        {
            OnStopHittingWall.Invoke();
            ObstacleForceDirection = new Vector3(0, 0, 0);
            IsVirtualHitting = false;
        }
    }
}
