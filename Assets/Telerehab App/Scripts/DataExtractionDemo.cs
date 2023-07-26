using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;
using UnityEngine.Events;

public class DataExtractionDemo : MonoBehaviour
{
    [SerializeField] private DeviceInformation deviceInformation;
    [SerializeField] private Transform predictionVisualization;

    public HapticPlugin Patient;

    // Declare your models
    public NNModel modelForX;
    public NNModel modelForY;
    public NNModel modelForZ;

    public NNModel modelForAL;

    // Declare worker (think of it as the model executor)
    private IWorker workerForX;
    private IWorker workerForY;
    private IWorker workerForZ;

    private IWorker workerForAL;

    public float predicted_x;
    public float predicted_y;
    public float predicted_z;

    public float predicted_al;

    [SerializeField] private float scale;

    // public float therapist_x_pre;
    // public float therapist_y_pre;
    // public float therapist_z_pre;

    // public float patient_x_pre;
    // public float patient_y_pre;
    // public float patient_z_pre;

    void Start()
    {
        // Load the models
        var runtimeModelForX = ModelLoader.Load(modelForX);
        var runtimeModelForY = ModelLoader.Load(modelForY);
        var runtimeModelForZ = ModelLoader.Load(modelForZ);

        var runtimeModelForAL = ModelLoader.Load(modelForAL);

        // Create a worker for each model
        workerForX = WorkerFactory.CreateWorker(runtimeModelForX);
        workerForY = WorkerFactory.CreateWorker(runtimeModelForY);
        workerForZ = WorkerFactory.CreateWorker(runtimeModelForZ);

        workerForAL = WorkerFactory.CreateWorker(runtimeModelForAL);

        predicted_x = deviceInformation.Position.x;
        predicted_y = deviceInformation.Position.y;
        predicted_z = deviceInformation.Position.z;

        predicted_al = 0;

        // therapist_x_pre = deviceInformation.Position.x - 1;
        // therapist_y_pre = deviceInformation.Position.y - 1;
        // therapist_z_pre = deviceInformation.Position.z - 1;

        // patient_x_pre = deviceInformation.Position.x - 1;
        // patient_y_pre = deviceInformation.Position.y - 1;
        // patient_z_pre = deviceInformation.Position.z - 1;

        // // Here you would probably want to call your prediction function
        // Predict();
    }

    void Predict()
    {   
        Vector3 JointAngles = deviceInformation.JointAngles;
        Vector3 GimbalAngles = deviceInformation.GimbalAngles;
        Vector3 Position = deviceInformation.Position;

        float[] inputData_x = new float[10] { predicted_x, JointAngles.x, JointAngles.y, JointAngles.z, 
                                        GimbalAngles.x, GimbalAngles.y, GimbalAngles.z, 
                                        Position.x, Position.y, Position.z};
        float[] inputData_y = new float[10] { predicted_y, JointAngles.x, JointAngles.y, JointAngles.z, 
                                        GimbalAngles.x, GimbalAngles.y, GimbalAngles.z, 
                                        Position.x, Position.y, Position.z};
        float[] inputData_z = new float[10] { predicted_z, JointAngles.x, JointAngles.y, JointAngles.z, 
                                        GimbalAngles.x, GimbalAngles.y, GimbalAngles.z, 
                                        Position.x, Position.y, Position.z};
        // float[] inputData_al = new float[7] { predicted_al, predicted_x / therapist_x_pre - 1,
        //                                predicted_y / therapist_y_pre - 1, predicted_z / therapist_z_pre - 1, 
        //                                Position.x / patient_x_pre - 1, Position.y / patient_y_pre - 1, Position.z / patient_z_pre - 1};
        float[] inputData_al = new float[7] { predicted_al, predicted_x, predicted_y, predicted_z, 
                                        Position.x, Position.y, Position.z};
        // // Input data for x prediction
        // float[] inputData_x = new float[12] {96.8208f, 48.58064f, 29.62018f, 21.03589f, 63.50977f, 26.5634f, -77.92664f, 122.8243f, -35.20501f, -59.99204f, 0.5624153f, 0.2515338f};
        // // Input data for y prediction
        // float[] inputData_y = new float[12] {2.533254f, 48.58064f, 29.62018f, 21.03589f, 63.50977f, 26.5634f, -77.92664f, 122.8243f, -35.20501f, -59.99204f, 0.5624153f, 0.2515338f};
        // // Input data for z prediction
        // float[] inputData_z = new float[12] {-56.20518f, 48.58064f, 29.62018f, 21.03589f, 63.50977f, 26.5634f, -77.92664f, 122.8243f, -35.20501f, -59.99204f, 0.5624153f, 0.2515338f};

        // // Convert the input data to Tensors
        // var inputTensor_x = new Tensor(1, 1, 12, 1, inputData_x);
        // var inputTensor_y = new Tensor(1, 1, 12, 1, inputData_y);
        // var inputTensor_z = new Tensor(1, 1, 12, 1, inputData_z);


        // // Run the models
        // workerForX.Execute(inputTensor_x);
        // workerForY.Execute(inputTensor_y);
        // workerForZ.Execute(inputTensor_z);

        // // Fetch the results (for simplicity, we just fetch the first element)
        // predicted_x = workerForX.PeekOutput()[0];
        // predicted_y = workerForY.PeekOutput()[0];
        // predicted_z = workerForZ.PeekOutput()[0];


        // therapist_x_pre = predicted_x;
        // therapist_y_pre = predicted_y;
        // therapist_z_pre = predicted_z;
        // patient_x_pre = Position.x;
        // patient_y_pre = Position.y;
        // patient_z_pre = Position.z;

        // Convert the input data to Tensors, using for dispose tensors
        using (var inputTensor_x = new Tensor(1, 1, 10, 1, inputData_x))
        using (var inputTensor_y = new Tensor(1, 1, 10, 1, inputData_y))
        using (var inputTensor_z = new Tensor(1, 1, 10, 1, inputData_z))
        using (var inputTensor_al = new Tensor(1, 1, 7, 1, inputData_al))
        {
            // Run the models
            workerForX.Execute(inputTensor_x);
            workerForY.Execute(inputTensor_y);
            workerForZ.Execute(inputTensor_z);

            workerForAL.Execute(inputTensor_al);

            // Fetch the results (for simplicity, we just fetch the first element)
            predicted_x = workerForX.PeekOutput()[0];
            predicted_y = workerForY.PeekOutput()[0];
            predicted_z = workerForZ.PeekOutput()[0];
            predicted_al = workerForAL.PeekOutput()[0];
        }

        Debug.Log("predicted assistant level: " + (float)(predicted_al * 0.11242022 + 0.26366002));
        Debug.Log("predicted:" + predicted_x +" " + predicted_y + " " + predicted_z);

        float predicted_x_vis = predicted_x/1000;
        float predicted_y_vis = predicted_y/1000;
        float predicted_z_vis = predicted_z/1000;
        Vector3 prediction = new Vector3(predicted_x_vis, predicted_y_vis, predicted_z_vis);
        // 2. return the prediction
        predictionVisualization.position = prediction;


        Vector3 shpere_prediction = new Vector3(predicted_x, predicted_y, predicted_z);

        Patient.ConstForceGDir = shpere_prediction - deviceInformation.Position;
        Patient.ConstForceGMag = (float)(scale * ( predicted_al * 0.11242022 + 0.26366002));
        Patient.EnableConstantForce();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Joint Angles: " + deviceInformation.JointAngles);
        Debug.Log("Gimbal Angles: " + deviceInformation.GimbalAngles);
        Debug.Log("position: " + deviceInformation.Position);
        Debug.Log("------------------------------------");
        // Predict();

        Predict();
        // Supply();
    }

    void Supply() {
        float predicted_x_vis = predicted_x;
        float predicted_y_vis = predicted_y;
        float predicted_z_vis = predicted_z;
        Vector3 shpere_prediction = new Vector3(predicted_x_vis, predicted_y_vis, predicted_z_vis);

        Patient.ConstForceGDir = shpere_prediction - deviceInformation.Position;
        Patient.ConstForceGMag = (float)(0.015 * ( predicted_al * 0.11242022 + 0.26366002));
        Patient.EnableConstantForce();
    }

    // private Vector3 Predict()
    // {
    //     // 1. do the prediction
    //     Vector3 prediction = Vector3.zero;
    //     // 2. return the prediction
    //     predictionVisualization.position = prediction;
    //     return prediction;
    // }

    void OnDestroy()
    {
        // Don't forget to dispose of the workers when you're done
        workerForX.Dispose();
        workerForY.Dispose();
        workerForZ.Dispose();
        workerForAL.Dispose();
    }
}
