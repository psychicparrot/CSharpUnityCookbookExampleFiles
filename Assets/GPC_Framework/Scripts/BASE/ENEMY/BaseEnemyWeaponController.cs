using UnityEngine;
using AIAttackStates;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/AI Weapon Controller")]

	public class BaseEnemyWeaponController : ExtendedCustomMonoBehaviour
	{
		public bool doFire;
		public bool onlyFireWhenOnscreen;
		public int pointsValue = 50;
		public int thisEnemyStrength = 1;
		public bool thisGameObjectShouldFire;
		public Renderer _rendererToTestAgainst;
		public StandardSlotWeaponController _slotWeaponController;
		private bool canFire;
		public float fireDelayTime = 1f;

		// default action is to attack nothing
		public AIAttackState currentState = AIAttackState.random_fire;
		public string tagOfTargetsToShootAt;

		public void Init()
		{
			// cache our transform
			_TR = transform;
			_GO = gameObject;

			if (_slotWeaponController == null)
			{
				_slotWeaponController = _GO.GetComponent<StandardSlotWeaponController>();
			}

			if (_rendererToTestAgainst == null)
			{
				_rendererToTestAgainst = _GO.GetComponentInChildren<Renderer>();
			}

			canFire = true;
			didInit = true;
		}

		private RaycastHit rayHit;

		public void Update()
		{
			if (!didInit)
				Init();

			if (!canControl)
				return;

			Firing();
		}

		void Firing()
		{
			if (!didInit)
				Init();

			if (thisGameObjectShouldFire)
			{
				// we use doFire to determine whether or not to fire right now
				doFire = false;

				// canFire is used to control a delay between firing
				if (canFire)
				{
					if (currentState == AIAttackState.random_fire)
					{
						// if the random number is over x, fire
						if (Random.Range(0, 100) > 98)
						{
							doFire = true;
						}
					}
					else if (currentState == AIAttackState.look_and_destroy)
					{
						if (Physics.Raycast(_TR.position, _TR.forward, out rayHit))
						{
							// is it an opponent to be shot at?
							if (rayHit.transform.CompareTag(tagOfTargetsToShootAt))
							{
								//	we have a match on the tag, so let's shoot at it
								doFire = true;
							}
						}

					} else {
						// if we're not set to random fire or look and destroy, just fire whenever we can
						doFire = true;
					}
				}

				if (doFire)
				{
					// we only want to fire if we are on-screen, visible on the main camera
					if (onlyFireWhenOnscreen && !_rendererToTestAgainst.IsVisibleFrom(Camera.main))
					{
						doFire = false;
						return;
					}

					// tell weapon control to fire, if we have a weapon controller
					if (_slotWeaponController != null)
					{
						// tell weapon to fire
						_slotWeaponController.Fire();
					}
					// set a flag to disable firing temporarily (providing a delay between firing)
					canFire = false;
					CancelInvoke("ResetFire");
					Invoke("ResetFire", fireDelayTime);
				}
			}
		}

		public void ResetFire()
		{
			canFire = true;
		}

	}
}