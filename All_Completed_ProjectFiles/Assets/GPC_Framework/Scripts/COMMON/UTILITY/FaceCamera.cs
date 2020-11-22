using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPC
{
	public class FaceCamera : MonoBehaviour
	{
		public Transform _TR;

		void Start()
		{
			_TR = transform;
		}

		void Update()
		{
			_TR.LookAt(Camera.main.transform, Camera.main.transform.up);
		}
	}
}