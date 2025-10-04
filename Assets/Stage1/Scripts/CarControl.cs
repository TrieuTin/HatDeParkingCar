using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour
{
    public List<AxleInfo> AxleInfos;
   
    public float maxSteeringAngle;

    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float driftFactor = 0.5f;  

    private float currentMotor;
    private float currentBrake=3000;
    private bool drifting = false;

    private bool isHolding = false;
  
    [SerializeField] Rigidbody rg;

    private WheelCollider leftMotor;
    private WheelCollider rightMotor;

    private WheelCollider leftSterring;
    private WheelCollider rightSterring;

    private float increase=0;

    private void Awake()
    {
        foreach (var item in AxleInfos)
        {
            if (item.Motor)
            {
                leftMotor = item.leftWheel;
                rightMotor = item.rightWheel;
            }
            if (item.sterring)
            {
                leftSterring = item.leftWheel;
                rightSterring = item.rightWheel;
            }
        }
    }

    private void FixedUpdate()
    {
        HandleInput();
        currentMotor = increase * motorForce;
        if (isHolding)
        {
            drifting = true;
            SetDrift(true);

        }
        else
        {
           
            drifting = false;
            SetDrift(false);

        }
        ApplyDrive(maxSteeringAngle);
    }
    void SetDrift(bool state)
    {
        WheelFrictionCurve sidewaysFriction = leftMotor.sidewaysFriction;

        if (state)
            sidewaysFriction.stiffness = driftFactor;  
        else
            sidewaysFriction.stiffness = 1f; 

        leftMotor.sidewaysFriction = sidewaysFriction;
        rightMotor.sidewaysFriction = sidewaysFriction;
    }
    void ApplyDrive(float steering)
    {
        
        leftSterring.steerAngle = steering ;
        rightSterring.steerAngle = steering;

        
        leftMotor.motorTorque = currentMotor;
        rightMotor.motorTorque = currentMotor;

        // phanh
       // leftMotor.brakeTorque = currentBrake;
       // rightMotor.brakeTorque = currentBrake;
    }
    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                isHolding = true;
                increase= 1f;
               
            }
            // release
            else if ((Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) )
            {
                isHolding = false;
                increase = 0;
                
                // check release before 180
                // check get point or gameover
            }
        }
    }
}
[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;

    public bool Motor; //is this wheel attached to motor?
    public bool sterring; //is this wheel apply ?

}
