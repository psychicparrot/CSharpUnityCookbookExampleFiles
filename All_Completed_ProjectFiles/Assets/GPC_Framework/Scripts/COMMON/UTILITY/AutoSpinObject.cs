using UnityEngine;
using System.Collections;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Utility/Auto spin object")]

	public class AutoSpinObject : MonoBehaviour
	{
		public Vector3 spinVector = new Vector3(1, 0, 0);
		private Transform _TR;

		void Start()
		{
			_TR = transform;
		}

		void Update()
		{
			_TR.Rotate(spinVector * Time.deltaTime);
		}
	}
}