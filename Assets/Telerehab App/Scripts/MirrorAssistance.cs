using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAssistance : MonoBehaviour
{
    [SerializeField] private Transform PFLTransform;
    [SerializeField] private GameObject TherapistAnchor;
    [SerializeField] private GameObject MirroredAnchor;

    [SerializeField] private HapticPlugin PILHapticPlugin;
    [SerializeField] private HapticPlugin PFLHapticPlugin;
    //[SerializeField] private HapticPlugin TherapistHapticPlugin;

    [SerializeField] private DeviceInformation PILDeviceInformation;
    [SerializeField] private DeviceInformation PFLDeviceInformation;
    //[SerializeField] private DeviceInformation TherapistDeviceInformation;


    //[SerializeField] private GameObject PFL;
    [SerializeField] private float PFLAssistanceLevel;
    [SerializeField] private float PILAssistanceLevel;

    private void Update()
    {
        MirrorTherapyWithSring();
        //MirrorTherapyWithSetConstantForce();
        //MirrorTherapyWithSetAnchorPosition();
    }

    private void MirrorTherapyWithSring()
    {
        
        PFLHapticPlugin.SpringAnchorObj = TherapistAnchor;
        PFLHapticPlugin.SpringGMag = PFLAssistanceLevel;
        PFLHapticPlugin.EnableSpring();

        MirroredAnchor.transform.position = GetMirroredPosition(TherapistAnchor.transform.position);
        PILHapticPlugin.SpringAnchorObj = MirroredAnchor;
        PILHapticPlugin.SpringGMag = PILAssistanceLevel;
        PILHapticPlugin.EnableSpring();
    }

    private void MirrorTherapyWithSetAnchorPosition()
    {
        Vector3 mirroredPosVec3 = GetMirroredPosition(PFLDeviceInformation.Position);
        double[] mirroredPosDoubleArr = { mirroredPosVec3.x, mirroredPosVec3.y, mirroredPosVec3.z };
        HapticPlugin.setAnchorPosition(PILHapticPlugin.DeviceIdentifier, mirroredPosDoubleArr);
        PILHapticPlugin.SpringGMag = PILAssistanceLevel;
        PILHapticPlugin.EnableSpring();
    }

    private void MirrorTherapyWithSetConstantForce()
    {

        // caclulating error vector with haptic plugin
        Vector3 mirroredPFLPos = GetMirroredPosition(PFLDeviceInformation.Position);
        Vector3 posErrorVector = mirroredPFLPos - PILDeviceInformation.Position;
        Vector3 posErrorVectorNormalized = Vector3.Normalize(posErrorVector);
        Debug.Log(posErrorVectorNormalized);

        // set constant force
        PILHapticPlugin.ConstForceGDir = posErrorVectorNormalized;
        PILHapticPlugin.ConstForceGMag = PILAssistanceLevel;
        PILHapticPlugin.EnableConstantForce();
    }

    private Vector3 GetMirroredPosition(Vector3 toMirror)
    {
        return new Vector3(-toMirror.x, toMirror.y, toMirror.z); 
    }




    //[SerializeField] private HapticPlugin hapticPlugin;
    //[SerializeField] private GameObject PFL;
    //[SerializeField] private float AssistanceLevel;
    //public Vector3 goHere;

    //private void Update()
    //{
    //    SetSpring();
    //}
    //public void SetSpring()
    //{
    //    double[] _goHere = { goHere.x, goHere.y, goHere.z};
    //    HapticPlugin.setAnchorPosition("PIL",_goHere);
    //    //hapticPlugin.SpringAnchorObj = PFL;
    //    //hapticPlugin.SpringGMag = AssistanceLevel;
    //    //hapticPlugin.EnableSpring();
    //}
}
