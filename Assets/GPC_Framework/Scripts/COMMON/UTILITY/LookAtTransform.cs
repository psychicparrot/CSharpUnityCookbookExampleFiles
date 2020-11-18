using UnityEngine;
using System.Collections;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Utility/Look At Camera")]

	public class LookAtTransform : MonoBehaviour
	{
		public GameObject target;

		void LateUpdate()
		{
			transform.LookAt(target.transform);
		}
	}
}