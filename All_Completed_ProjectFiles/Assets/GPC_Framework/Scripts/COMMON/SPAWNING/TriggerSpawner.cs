using UnityEngine;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Utility/Trigger Spawner")]

	public class TriggerSpawner : Spawner
	{
		public GameObject ObjectToSpawnOnTrigger;
		public Vector3 offsetPosition;

		public bool onlySpawnOnce;
		public LayerMask triggerLayerMask;

		void OnTriggerEnter(Collider other)
		{
			// make sure that the layer of the object entering our trigger is the one to cause the boss to spawn
			if (IsInLayerMask(other.gameObject.layer, triggerLayerMask))
				return;

			// instantiate the object to spawn on trigger enter
			Spawn(ObjectToSpawnOnTrigger.transform, _TR.position + offsetPosition, Quaternion.identity);

			// if we are to only spawn once, destroy this gameobject after spawn occurs
			if (onlySpawnOnce)
				Destroy(gameObject);
		}
	}
}