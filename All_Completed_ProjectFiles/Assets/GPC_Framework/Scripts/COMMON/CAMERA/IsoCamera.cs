using UnityEngine;
using GPC;
[AddComponentMenu("CSharpBookCode/Common/Cameras/Iso Cam Controller")]

public class IsoCamera : BaseCameraController
{
	public Vector3 targetOffset;
	public float moveSpeed = 5f;

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
