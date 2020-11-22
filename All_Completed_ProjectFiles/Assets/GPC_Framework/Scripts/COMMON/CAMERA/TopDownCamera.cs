using UnityEngine;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Common/Cameras/Top Down Cam Controller")]

	public class TopDownCamera : BaseCameraController
	{
		public Vector3 targetOffset;
		public float moveSpeed = 2f;

		public float maxHeight;
		public float minHeight;

		void Start()
		{
			_TR = transform;
		}

		void LateUpdate()
		{
			if (_cameraTarget != null)
				_TR.position = Vector3.Lerp(_TR.position, _cameraTarget.position + targetOffset, moveSpeed * Time.deltaTime);
		}
	}
}