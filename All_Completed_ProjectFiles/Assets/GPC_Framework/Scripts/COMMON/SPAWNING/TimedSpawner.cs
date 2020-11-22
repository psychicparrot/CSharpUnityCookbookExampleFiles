using UnityEngine;

namespace GPC
{
	public class TimedSpawner : Spawner
	{
		public float initialSpawnDelay;
		public float randomDelayMax = 0.5f;

		[Space]
		public bool waitForCall;

		private Vector3 myPos;
		private Quaternion myRot;

		[Space]
		public Transform _prefabToSpawn;

		public override void Start()
		{
			base.Start();

			if (waitForCall)
				return;

			StartSpawnTimer();
		}

		public void StartSpawnTimer()
		{
			// start out with the value of spawnDelay to decide when to spawn..
			float spawnTime = initialSpawnDelay;

			if (shouldRandomizeSpawnTime)
				spawnTime += Random.Range(0, randomDelayMax);

			Invoke("SpawnAndDestroy", spawnTime);
		}

		public void SpawnAndDestroy()
		{
			Spawn(_prefabToSpawn, _TR.position, _TR.rotation);
			Destroy(gameObject);
		}
	}
}