using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPC;

public class FakeWheels : MonoBehaviour
{
	public Rigidbody _RB;

	[Space]
	public Transform wheel_FL;
	public Transform wheel_FR;
	public Transform wheel_RL;
	public Transform wheel_RR;

	void Start()
    {
		_RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
		Vector3 velocity = _RB.velocity * 2f;
		Vector3 localVel = transform.InverseTransformDirection(velocity);

		wheel_FR.Rotate(localVel.z, 0, 0);
		wheel_FL.Rotate(localVel.z, 0, 0);
		wheel_RR.Rotate(localVel.z, 0, 0);
		wheel_RL.Rotate(localVel.z, 0, 0);

		//float turnSpeed = localVel.y;
		//Debug.Log("turn=" + turnSpeed);

		//Vector3 tempVEC = wheel_FL.localEulerAngles;
		//tempVEC.y = (turnSpeed * turnMultiplier);
		//wheel_FL.localEulerAngles = tempVEC;
		//wheel_FR.localEulerAngles = tempVEC;

	}
}
