using System.Collections.Generic;
using UnityEngine;

public class DeviceInformation : MonoBehaviour
{
    public HapticPlugin HapticPlugin;
    public Vector3 JointAngles = new Vector3();
    public Vector3 GimbalAngles = new Vector3();
    public Vector3 Position = new Vector3();

    public List<Vector3> GimbalHistory = new List<Vector3>();
    public List<Vector3> JointAngleHistory = new List<Vector3>();

    public Vector3 ConstantForce = new Vector3();
    public Vector3 StiffnessForce = new Vector3();
    public Vector3 ViscosityStiffnessForce = new Vector3();
    public Vector3 DynamicFrictionForce = new Vector3();
    public Vector3 StaticFrictionForce = new Vector3();
    public Vector3 SpringForce = new Vector3();
    public List<float> transformMatrix = new List<float>();

    private void Start()
    {
        //Vector3 SpringGDir = new Vector3(1.29f, -92.3f, -31.99f);
        //float SpringGMag = 0.1f;
        //SetSpring(SpringGDir, SpringGMag);
    }

    private void FixedUpdate()
    {
        transformMatrix.Clear();
        double[] blah = new double[16];
        HapticPlugin.getTransform(HapticPlugin.DeviceIdentifier, blah);
        foreach(double _blah in blah)
        {
            transformMatrix.Add((float)_blah);
        }
        GetJointAngles();
        GetPosition();
        GetLocalForces();
    }

    public void SetSpring(Vector3 SpringGDir, float SpringGMag)
    {
        HapticPlugin.setSpringValues(HapticPlugin.DeviceIdentifier, HapticPlugin.Vector3ToDoubleArray(SpringGDir), SpringGMag);
    }

    public void GetJointAngles()
    {
        double[] tempJointAngles = new double[3];
        double[] tempGibalAngles = new double[3];

        HapticPlugin.getJointAngles(HapticPlugin.DeviceIdentifier, tempJointAngles, tempGibalAngles);

        JointAngles[0] = (float)tempJointAngles[0] * Mathf.Rad2Deg;
        JointAngles[1] = (float)tempJointAngles[1] * Mathf.Rad2Deg;
        JointAngles[2] = (float)tempJointAngles[2] * Mathf.Rad2Deg;
        JointAngleHistory.Add(JointAngles);

        GimbalAngles[0] = (float)tempGibalAngles[0] * Mathf.Rad2Deg;
        GimbalAngles[1] = (float)tempGibalAngles[1] * Mathf.Rad2Deg;
        GimbalAngles[2] = (float)tempGibalAngles[2] * Mathf.Rad2Deg;
        GimbalHistory.Add(GimbalAngles);
    }

    public void GetPosition()
    {
        double[] tempPositionArray = new double[3];
        HapticPlugin.getPosition(HapticPlugin.DeviceIdentifier, tempPositionArray); // mm
        Position[0] = (float)tempPositionArray[0];
        Position[1] = (float)tempPositionArray[1];
        Position[2] = (float)tempPositionArray[2];
    }

    public void GetForce()
    {
        double[] tempForceArray = new double[3];
        HapticPlugin.getCurrentForce(HapticPlugin.DeviceIdentifier, tempForceArray);
        ConstantForce[0] = (float)tempForceArray[0];
        ConstantForce[1] = (float)tempForceArray[1];
        ConstantForce[2] = (float)tempForceArray[2];
    }

    public void GetLocalForces()
    {
        double[] tempStiffnessForceArray = new double[3];
        double[] tempViscosityStiffnessForceArray = new double[3];
        double[] tempDynamicFrictionForceArray = new double[3];
        double[] tempStaticFrictionForceArray = new double[3];
        double[] tempConstantForce = new double[3];
        double[] tempSpringForce = new double[3];

        HapticPlugin.getLocalForces(HapticPlugin.DeviceIdentifier, tempStiffnessForceArray, tempViscosityStiffnessForceArray, tempDynamicFrictionForceArray, tempStaticFrictionForceArray, tempConstantForce, tempSpringForce);
        StiffnessForce = DoubleArrayToVector3(tempStiffnessForceArray);
        ViscosityStiffnessForce = DoubleArrayToVector3(tempViscosityStiffnessForceArray);
        DynamicFrictionForce = DoubleArrayToVector3(tempDynamicFrictionForceArray);
        StaticFrictionForce = DoubleArrayToVector3(tempStaticFrictionForceArray);
        ConstantForce = DoubleArrayToVector3(tempConstantForce);
        SpringForce = DoubleArrayToVector3(tempSpringForce);
    }

    Vector3 DoubleArrayToVector3(double[] doubleArray)
    {
        Vector3 toReturn = new Vector3();
        toReturn[0] = (float)doubleArray[0];
        toReturn[1] = (float)doubleArray[1];
        toReturn[2] = (float)doubleArray[2];
        return toReturn;
    }
}
