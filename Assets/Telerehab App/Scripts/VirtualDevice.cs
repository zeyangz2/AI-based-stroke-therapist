using UnityEngine;

public class VirtualDevice : MonoBehaviour
{
    public GameObject joint0;
    public GameObject joint1;
    public GameObject joint2;
    public GameObject joint3;
    public GameObject joint4;
    public GameObject joint5;

    public Transform EndEffector;

    public Transform virtualStylus;

    public void UpdateVirtualStylus(Transform patientStylus, Transform therapistStylus, float patientWeight)
    {
        Vector3 weightedPatientPos = patientStylus.localPosition * patientWeight;
        Vector3 weightedTherapistPos = therapistStylus.localPosition * (1 - patientWeight);

        Vector3 weightedPatientRot = patientStylus.localRotation.eulerAngles * patientWeight;
        Vector3 weightedTherapistRot = therapistStylus.localRotation.eulerAngles * (1 - patientWeight);
        Quaternion averageRot = Quaternion.Euler(weightedPatientRot + weightedTherapistRot);

        virtualStylus.transform.localPosition = weightedPatientPos + weightedTherapistPos;
        virtualStylus.transform.localRotation = averageRot;
    }

    public void UpdateVirtualVisual(Vector3 patientJointAngles, Vector3 patientGimbalAngles, Vector3 therapistJointAngles, Vector3 therapistGimbalAngles, float patientWeight)
    {
        Vector3 j0 = new Vector3(0.0f, patientJointAngles[0], 0.0f) * patientWeight + new Vector3(0.0f, therapistJointAngles[0], 0.0f) * (1.0f - patientWeight);
        
        if (!float.IsNaN(j0[0]) && !float.IsNaN(j0[1]) && !float.IsNaN(j0[2]))
        {
            joint0.transform.localRotation = Quaternion.Euler(j0);
        }


        Vector3 j1 = new Vector3(patientJointAngles[1] * -1f, 0.0f, 0.0f) * patientWeight + new Vector3(therapistJointAngles[1] * -1f, 0.0f, 0.0f) * (1.0f - patientWeight);
        if (!float.IsNaN(j1.magnitude))
        {
            joint1.transform.localRotation = Quaternion.Euler(j1);
        }


        Vector3 j2 = new Vector3((patientJointAngles[2] - patientJointAngles[1]) * -1f, 0.0f, 0.0f) * patientWeight + new Vector3((therapistJointAngles[2] - therapistJointAngles[1]) * -1f, 0.0f, 0.0f) * (1.0f - patientWeight);
        if (!float.IsNaN(j2.magnitude))
        {
            joint2.transform.localRotation = Quaternion.Euler(j2);
        }


        Vector3 j3 = new Vector3(0.0f, patientGimbalAngles[0] * -1.0f, 0.0f) * patientWeight + new Vector3(0.0f, therapistGimbalAngles[0] * -1.0f, 0.0f) * (1.0f - patientWeight);
        if (!float.IsNaN(j3.magnitude))
        {
            joint3.transform.localRotation = Quaternion.Euler(j3);
        }


        Vector3 j4 = new Vector3(patientGimbalAngles[1] * -1.0f, 0.0f, 0.0f) * patientWeight + new Vector3(therapistGimbalAngles[1] * -1.0f, 0.0f, 0.0f) * (1.0f - patientWeight);
        if (!float.IsNaN(j4.magnitude))
        {
            joint4.transform.localRotation = Quaternion.Euler(j4);
        }


        Vector3 j5 = new Vector3(0.0f, 0.0f, patientGimbalAngles[2]) * patientWeight + new Vector3(0.0f, 0.0f, therapistGimbalAngles[2]) * (1.0f - patientWeight);
        if (!float.IsNaN(j5.magnitude))
        {
            joint5.transform.localRotation = Quaternion.Euler(j5);
        }
    }
}
