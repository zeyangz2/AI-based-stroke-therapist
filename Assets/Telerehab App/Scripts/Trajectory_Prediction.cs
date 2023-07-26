using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory_Prediction : MonoBehaviour
{
    [SerializeField] private DeviceInformation deviceInformation;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Joint Angles: " + deviceInformation.JointAngles);
        Debug.Log("Gimbal Angles: " + deviceInformation.GimbalAngles);
        Debug.Log("-------------------------------");
    }
}
