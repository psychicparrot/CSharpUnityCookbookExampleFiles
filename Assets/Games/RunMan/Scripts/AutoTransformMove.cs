using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPC;

public class AutoTransformMove : MonoBehaviour
{
	public float moveSpeed;
	public Vector3 moveVector = Vector3.left;

	private Transform _TR;

	void Start()
	{
		// cache the transform ref
		_TR = transform;
	}

	void Update()
	{
		// keep move speed updated from game manager
		moveSpeed = RunManGameManager.instance.runSpeed;

		// move the transform
		_TR.Translate((moveVector * moveSpeed) * Time.deltaTime);
	}

	public void SetMoveSpeed(float aSpeed)
	{
		// provide method to adjust speed from other scripts
		moveSpeed = aSpeed;
	}
}
