using UnityEngine;

public class VirtualDeviceInformation : MonoBehaviour
{
    public VirtualDevice VirtualDevice;

    public Vector3 JointAngles = new Vector3();
    public Vector3 Position = new Vector3();

    private void FixedUpdate()
    {
        GetJointAngles();
        GetPosition();
    }

    public void GetJointAngles()
    {
        JointAngles[0] = VirtualDevice.joint0.transform.eulerAngles.y;
        JointAngles[1] = VirtualDevice.joint1.transform.eulerAngles.x;
        JointAngles[2] = VirtualDevice.joint2.transform.eulerAngles.x;
    }

    public void GetPosition()
    {
        Position = VirtualDevice.EndEffector.position;
    }
}
