using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public HapticPlugin hp;

    private void OnCollisionEnter(Collision collision)
    {
        //hp.GrabObject();
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.collider.gameObject.name);
    }
}
