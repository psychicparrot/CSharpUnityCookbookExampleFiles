using UnityEngine;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Common/Spawn Prefabs (no path following)")]

	public class Spawner : ExtendedCustomMonoBehaviour
	{
		public bool distanceBasedSpawnStart;
		public float distanceFromCameraToSpawnAt = 35f;
		public bool shouldAutoStartSpawningOnLoad;
		public float timeBetweenSpawns = 1;
		public bool startDelay;
		public int totalAmountToSpawn = 10;
		public bool shouldRepeatWaves;
		public bool shouldRandomizeSpawnTime;
		public float minimumSpawnTimeGap = 0.5f;
		public GameObject[] _spawnObjectPrefabs;
		private int spawnCounter = 0;
		private int currentObjectNum;
		private Transform _cameraTransform;
		private bool spawning;

		public virtual void Start()
		{
			// cache ref to our transform
			_TR = transform;

			if(_cameraTransform==null)
				_cameraTransform = Camera.main.transform;

			if (shouldAutoStartSpawningOnLoad)
				StartSpawn();
		}

		public void OnDrawGizmos()
		{
			if (distanceBasedSpawnStart)
			{
				Gizmos.color = new Color(0, 0, 1, 0.5f);
				Gizmos.DrawSphere(transform.position, distanceFromCameraToSpawnAt);
			} else
			{
				Gizmos.color = new Color(0, 1, 1, 1f);
				Gizmos.DrawSphere(transform.position, 1f);
			}
		}


		public void Update()
		{
			if (_TR == null || _cameraTransform == null)
				return;

			float aDist = Vector3.Distance(_TR.position, _cameraTransform.position);

			if (distanceBasedSpawnStart && !spawning && aDist < distanceFromCameraToSpawnAt)
			{
				StartSpawn();
				spawning = true;
			}
		}

		void StartSpawn()
		{
			StartWave(totalAmountToSpawn, timeBetweenSpawns);
		}

		public void StartWave(int HowMany, float timeBetweenSpawns)
		{
			spawnCounter = 0;
			totalAmountToSpawn = HowMany;

			// reset 
			currentObjectNum = 0;

			CancelInvoke("doSpawn");

			// the option is there to spawn at random times, or at fixed intervals...
			if (shouldRandomizeSpawnTime)
			{
				// do a randomly timed invoke call, based on the times set up in the inspector
				Invoke("doSpawn", Random.Range(minimumSpawnTimeGap, timeBetweenSpawns));
			}
			else
			{
				// do a regularly scheduled invoke call based on times set in the inspector
				InvokeRepeating("doSpawn", timeBetweenSpawns, timeBetweenSpawns);
			}
		}

		void doSpawn()
		{
			if (spawnCounter >= totalAmountToSpawn)
			{
				// if we are in 'repeat' mode, we will just reset the value of spawnCounter to 0 and carry on spawning
				if (shouldRepeatWaves)
				{
					spawnCounter = 0;
				}
				else
				{
					// as we are not going to repeat the wave (shouldRepeatWaves=false) we need to drop out here and disable this script
					CancelInvoke("doSpawn");
					this.enabled = false;
					return;
				}
			}

			// create an object
			Spawn(_spawnObjectPrefabs[currentObjectNum].transform, _TR.position, Quaternion.identity);
			spawnCounter++;
			currentObjectNum++;

			// check to see if we've reached the end of the spawn objects array
			if (currentObjectNum > _spawnObjectPrefabs.Length - 1)
				currentObjectNum = 0;

			if (shouldRandomizeSpawnTime)
			{
				// cancel invoke for safety
				CancelInvoke("doSpawn");

				// schedule the next random spawn
				Invoke("doSpawn", Random.Range(minimumSpawnTimeGap, timeBetweenSpawns));
			}
		}
	}
}