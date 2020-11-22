using UnityEngine;
using System.Collections;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Common/Projectile Controller")]

	public class ProjectileController : MonoBehaviour
	{
		public bool doProjectileHitParticle;
		public GameObject particleEffectPrefab;
		public int ownerType_id;

		private Transform _TR;
		private Vector3 tempVEC;

		public bool shouldFollowGround;
		public float groundHeightOffset = 15f;
		public LayerMask groundLayerMask;

		public int overrideDamageValue;
		public int overridePoints;

		private bool didPlaySound;
		private int whichSoundToPlayOnStart = -1;

		void Start()
		{
			_TR = transform;

			didPlaySound = false;
		}

		// having an SetOwnerType id means we can assign a number to represent a sender (so we know who fired)
		public void SetOwnerType(int aNum)
		{
			ownerType_id = aNum;
			transform.name = aNum.ToString();
		}

		void Update()
		{
			// we play the pew sound here in the Update loop because we want it to have been positioned in the right
			// place when we do it. If we play the sound in Start() it may be that the projectile is not yet set up
			if (!didPlaySound && whichSoundToPlayOnStart!=-1)
			{
				// tell our sound controller to play a pew sound
				BaseSoundManager.instance.PlaySoundByIndex(whichSoundToPlayOnStart, _TR.position);
				// we only want to play the sound once, so set didPlaySound here
				didPlaySound = true;
			}

			if (shouldFollowGround)
			{
				// cast a ray down from the waypoint to try to find the ground
				tempVEC = _TR.position;

				RaycastHit hit;
				if (Physics.Raycast(tempVEC, -Vector3.up, out hit, groundLayerMask))
				{
					tempVEC.y = hit.point.y + groundHeightOffset;
					_TR.position = tempVEC;
				}
			}
		}

		void OnCollisionEnter(Collision col)
		{
			// if we have assigned a particle effect, we will instantiate one when a collision happens.
			if (doProjectileHitParticle)
				Instantiate(particleEffectPrefab, transform.position, Quaternion.identity);

			// destroy this game object after a collision
			Destroy(gameObject);
		}

	}
}