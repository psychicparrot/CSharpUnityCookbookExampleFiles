using UnityEngine;
using System.Collections;

namespace GPC
{
	public class AutomaticDestroyObject : MonoBehaviour
	{
		public float timeBeforeObjectDestroys;

		void Start()
		{
			// the function destroyGO() will be called in timeBeforeObjectDestroys seconds
			Invoke("destroyGO", timeBeforeObjectDestroys);
		}

		void destroyGO()
		{
			// destroy this gameObject
			Destroy(gameObject);
		}
	}
}