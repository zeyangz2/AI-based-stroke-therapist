using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrainingSession : MonoBehaviour
{
    public Patient Patient;
    public Therapist Therapist;
    public VirtualDevice VirtualDevice;
    public VirtualDeviceInformation VirtualDeviceInformation;
    public Obstacle obstacle;

    public float AssistanceLevel;
    
    [Range(0f, 0.1f)]
    public float DegreeOfAssistance = 0.05f;

    public List<float> TrainingDistances = new List<float>();
    public int FrameNumber;
    public float RecoveryNot = 0f;
    //public float Recovery = 0f;
    [Range(0f, 1f)]
    public float RecoveryFactor = 0f;

    public float AssitiveForceMagnitude;
    public float TherapistObstacleFeedbackForceMagnitude;
    public float PatientObstacleFeedbackForceMagnitude;
    [Range(0f, 20f)]
    public float DistanceTolerance;

    public bool IsTraditional;

    public UnityEvent<float> OnUpdateRecoveryFactor = new UnityEvent<float>();
    public UnityEvent<float> OnUpdateAssistanceLevel = new UnityEvent<float>();

    // Update is called once per frame
    void FixedUpdate()
    {
        ComputeRecoveryFactor();
        ProvideEffectiveForce();
        UpdateVisualizations();
    }

    public void ComputeRecoveryFactor()
    {
        // 1. caluclate distance and append to list
        float distance = Vector3.Distance(Therapist.DeviceInformation.Position, Patient.DeviceInformation.Position);
        // Debug.Log("Distance from training: " + distance.ToString());
        TrainingDistances.Add(distance);

        // 2. calculate error metric using moving average
        if (FrameNumber < 500)
        {
            float recoveryInitial = 0;
            foreach(float trainingDistance in TrainingDistances)
            {
                recoveryInitial += trainingDistance;
            }
            recoveryInitial /= TrainingDistances.Count;
            RecoveryFactor = 1 - (recoveryInitial / RecoveryNot);
        } else
        {
            if(distance < DistanceTolerance)
            {
                distance = 0f;
            }
            List<float> window = TrainingDistances.GetRange(FrameNumber - 500, 500);
            float windowPosErr = 0;
            foreach(float windowDistance in window)
            {
                windowPosErr += windowDistance;
            }
            windowPosErr /= window.Count;
            RecoveryFactor = 1 - (windowPosErr / RecoveryNot);
            //Recovery = recovery;
        }

        RecoveryFactor = Mathf.Clamp01(RecoveryFactor);
        OnUpdateRecoveryFactor.Invoke(RecoveryFactor);
        FrameNumber += 1;
    }

    

    public void InitializeTrainingSession()
    {
        gameObject.SetActive(true);
    }

    public void InitializeTrainingSession(float RecoveryNot)
    {
        gameObject.SetActive(true);
        this.RecoveryNot = RecoveryNot;
    }

    public void StopTrainingSession()
    {
        gameObject.SetActive(false);
    }

    private void UpdateVisualizations()
    {
        //if (Time.frameCount > 1000)
        //{
        //    VirtualDevice.UpdateVirtualVisual(Patient.DeviceInformation.JointAngles, Patient.DeviceInformation.GimbalAngles,
        //                                      Therapist.DeviceInformation.JointAngleHistory[0], Therapist.DeviceInformation.GimbalHistory[0], PatientWeight);
        //}
        //VirtualDevice.UpdateVirtualVisual(Patient.DeviceInformation.JointAngles, Patient.DeviceInformation.GimbalAngles,
        //    Therapist.DeviceInformation.JointAngles, Therapist.DeviceInformation.GimbalAngles, PatientWeight);

        if (Time.frameCount > 1000)
        {
            VirtualDevice.UpdateVirtualStylus(Patient.HapticPlugin.VisualizationMesh.transform, Therapist.HapticPlugin.VisualizationMesh.transform, RecoveryFactor);
        }
        VirtualDevice.UpdateVirtualStylus(Patient.HapticPlugin.VisualizationMesh.transform, Therapist.HapticPlugin.VisualizationMesh.transform, RecoveryFactor);
    }

    void ProvideEffectiveForce()
    {
        if (obstacle.IsVirtualHitting)
        {

            //// caclulating error vector
            //Vector3 error = (1 - RecoveryFactor) * (Therapist.DeviceInformation.Position - Patient.DeviceInformation.Position);
            //Vector3 assitiveForce = error * DegreeOfAssistance;
            //AssitiveForceMagnitude = assitiveForce.magnitude;
            //Vector3 effectiveForceVector = assitiveForce + obstacle.ObstacleForceDirection*obstacle.ObstacleForceMagnitude;


            ////error = therapistInfromation.Position - patientInformation.Position;
            //Vector3 normalizedForceVector = effectiveForceVector.normalized;
            ////normalizedError = error.normalized;
            //float magnitude = effectiveForceVector.magnitude;

            // only supply obstacle forces
            // supplying error vector as a force

            if(IsTraditional)
            {
                Patient.HapticPlugin.ConstForceGDir = obstacle.ObstacleForceDirection;
                Patient.HapticPlugin.ConstForceGMag = obstacle.ObstacleForceMagnitude * RecoveryFactor;
                Patient.HapticPlugin.EnableConstantForce();

                // supplying error vector as a force
                Therapist.HapticPlugin.ConstForceGDir = obstacle.ObstacleForceDirection;
                Therapist.HapticPlugin.ConstForceGMag = obstacle.ObstacleForceMagnitude * (1-RecoveryFactor); // make this a variable
                Therapist.HapticPlugin.EnableConstantForce();
            } else
            {
                Patient.HapticPlugin.ConstForceGDir = obstacle.ObstacleForceDirection;
                Patient.HapticPlugin.ConstForceGMag = obstacle.ObstacleForceMagnitude;
                Patient.HapticPlugin.EnableConstantForce();

                // supplying error vector as a force
                Therapist.HapticPlugin.ConstForceGDir = obstacle.ObstacleForceDirection;
                Therapist.HapticPlugin.ConstForceGMag = obstacle.ObstacleForceMagnitude; // make this a variable
                Therapist.HapticPlugin.EnableConstantForce();
            }
            
        }
        else
        {

            Therapist.HapticPlugin.ConstForceGDir = Vector3.zero;
            Therapist.HapticPlugin.ConstForceGMag = 0f; 
            Therapist.HapticPlugin.DisableContantForce();

            // caclulating error vector
            Vector3 posErrorVector = Therapist.DeviceInformation.Position - Patient.DeviceInformation.Position;


            float inversedRecoveryFactor = 1 - RecoveryFactor;
            Vector3 assistiveForceVector = DegreeOfAssistance * inversedRecoveryFactor * posErrorVector;
            float assistiveForceMagnitude = assistiveForceVector.magnitude;
            AssistanceLevel = assistiveForceMagnitude;
            OnUpdateAssistanceLevel.Invoke(AssistanceLevel);
            Vector3 assistiveForceVectorNormalized = assistiveForceVector.normalized;

            Patient.HapticPlugin.ConstForceGDir = assistiveForceVectorNormalized;
            Patient.HapticPlugin.ConstForceGMag = assistiveForceMagnitude;
            Patient.HapticPlugin.EnableConstantForce();

            //Vector3 error = (1 - RecoveryFactor) * (Therapist.DeviceInformation.Position - Patient.DeviceInformation.Position);
            //AssitiveForceMagnitude = error.magnitude * DegreeOfAssistance;
            ////error = therapistInfromation.Position - patientInformation.Position;
            //Vector3 normalizedError = error.normalized;
            //float magnitude = error.magnitude * DegreeOfAssistance;

            //// supplying error vector as a force
            //Patient.HapticPlugin.ConstForceGDir = normalizedError;
            //Patient.HapticPlugin.ConstForceGMag = magnitude;
            //Patient.HapticPlugin.EnableConstantForce();
        }


        //// caclulating error vector
        //Vector3 error = (1 - PatientWeight) * (Therapist.DeviceInformation.Position - Patient.DeviceInformation.Position);
        ////error = therapistInfromation.Position - patientInformation.Position;
        //Vector3 normalizedError = error.normalized;
        //float scaledMagnitude = error.magnitude * DegreeOfAssistance;

        //// supplying error vector as a force
        //Patient.HapticPlugin.ConstForceGDir = normalizedError;
        //Patient.HapticPlugin.ConstForceGMag = scaledMagnitude;
        //Patient.HapticPlugin.EnableConstantForce();
    }
}
