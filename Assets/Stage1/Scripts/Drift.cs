using System;
using UnityEngine;

public class Drift : MonoBehaviour
{
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;
    public float driftFactor = 0.95f;
    public float maxDriftSlip = 0.5f;

    void FixedUpdate()
    {
        ApplyDrift(rearLeftWheel);
        ApplyDrift(rearRightWheel);
    }

    void ApplyDrift(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            Vector3 localVelocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().linearVelocity);
            float slip = Mathf.Clamp(localVelocity.x * driftFactor, -maxDriftSlip, maxDriftSlip);

            WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;
            sidewaysFriction.stiffness = 1.0f - Mathf.Abs(slip);
            wheel.sidewaysFriction = sidewaysFriction;
        }
    }

}
