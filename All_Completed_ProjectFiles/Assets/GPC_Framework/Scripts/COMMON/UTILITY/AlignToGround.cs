using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPC
{
	public class AlignToGround : MonoBehaviour
	{
		public LayerMask mask;
		private Transform _TR;

		RaycastHit hit;
		Quaternion targetRotation;

		private int smooth = 10;

		void Start()
		{
			_TR = transform;
		}

		void Update()
		{
			RaycastHit hit;
			if (Physics.Raycast(_TR.position, -Vector3.up, out hit, 2.0f, mask))
			{
				targetRotation = Quaternion.FromToRotation(_TR.up, hit.normal) * _TR.rotation;
				_TR.rotation = Quaternion.Lerp(_TR.rotation, targetRotation, Time.deltaTime * smooth);
			}
		}
	}
}