using UnityEngine;
namespace GPC
{
	public class BaseWheelAlignment : ExtendedCustomMonoBehaviour
	{
		public WheelCollider _correspondingCollider;
		public GameObject _slipPrefab;
		public float slipAmountForTireSmoke = 50f;
		private float RotationValue = 0.0f;
		private Transform _colliderTransform;

		void Start()
		{
			_TR = transform;
			_colliderTransform = _correspondingCollider.transform;
		}

		void Update()
		{
			RaycastHit hit;
			Vector3 ColliderCenterPoint = _colliderTransform.TransformPoint(_correspondingCollider.center);

			if (Physics.Raycast(ColliderCenterPoint, -_colliderTransform.up, out hit, _correspondingCollider.suspensionDistance + _correspondingCollider.radius))
			{
				_TR.position = hit.point + (_colliderTransform.up * _correspondingCollider.radius);
			} else {
				_TR.position = ColliderCenterPoint - (_colliderTransform.up * _correspondingCollider.suspensionDistance);
			}

			_TR.rotation = _colliderTransform.rotation * Quaternion.Euler(RotationValue, _correspondingCollider.steerAngle, 0);

			RotationValue += _correspondingCollider.rpm * 6 * Time.deltaTime;

			RotationValue = RotationValue % 360;

			WheelHit correspondingGroundHit = new WheelHit();
			_correspondingCollider.GetGroundHit(out correspondingGroundHit);

			if (Mathf.Abs(correspondingGroundHit.sidewaysSlip) > slipAmountForTireSmoke)
			{
				if (_slipPrefab)
				{
					Spawn(_slipPrefab.transform, correspondingGroundHit.point, Quaternion.identity);
				}
			}

		}
	}
}