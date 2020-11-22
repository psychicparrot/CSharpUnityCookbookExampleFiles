using UnityEngine;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Camera Controller")]
	public class BaseCameraController : MonoBehaviour
	{
		public Transform _cameraTarget;
		public Transform _TR;

		private void Awake()
		{
			if (_TR == null)
				_TR = transform;
		}

		public virtual void SetTarget(Transform aTarget)
		{
			_cameraTarget = aTarget;
		}
	}
}