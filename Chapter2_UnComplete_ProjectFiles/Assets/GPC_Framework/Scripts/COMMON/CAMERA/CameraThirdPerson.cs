using UnityEngine;
using System.Collections;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Common/Cameras/Third Person Cam Controller")]

	public class CameraThirdPerson : BaseCameraController
	{
		public float distance = 20.0f;
		public float height = 5.0f;
		public float heightDamping = 2.0f;

		public float lookAtHeight = 0.0f;

		public float rotationSnapTime = 0.3F;

		public float distanceSnapTime;

		public Vector3 lookAtAdjustVector;

		private float usedDistance;

		float wantedRotationAngle;
		float wantedHeight;

		float currentRotationAngle;
		float currentHeight;

		Quaternion currentRotation;
		Vector3 wantedPosition;

		private float yVelocity = 0.0F;
		private float zVelocity = 0.0F;

		void Update()
		{
			if (_cameraTarget == null)
				return;

			wantedHeight = _cameraTarget.position.y + height;
			currentHeight = _TR.position.y;

			wantedRotationAngle = _cameraTarget.eulerAngles.y;
			currentRotationAngle = _TR.eulerAngles.y;

			currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);

			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			wantedPosition = _cameraTarget.position;
			wantedPosition.y = currentHeight;

			usedDistance = Mathf.SmoothDampAngle(usedDistance, distance, ref zVelocity, distanceSnapTime);

			wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);

			_TR.position = wantedPosition;
			_TR.LookAt(_cameraTarget.position);
			_TR.Rotate(lookAtAdjustVector);
		}

	}
}