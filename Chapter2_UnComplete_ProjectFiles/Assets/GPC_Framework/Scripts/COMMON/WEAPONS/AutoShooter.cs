// this weapon script fires a projectile automatically, with a delay of <fireDelay> seconds in-between

using UnityEngine;
using System.Collections;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Common/Automatic Shooting Weapon Script")]

	public class AutoShooter : BaseWeaponScript
	{
		private Vector3 fireDirection;

		public override void Enable()
		{
			// drop out if firing is disabled
			if (canFire == true)
				return;

			// enable weapon (do things like show the weapons mesh etc.)
			canFire = true;

			// schedule the first fire
			CancelInvoke("FireProjectile");
			InvokeRepeating("FireProjectile", fireDelay, fireDelay);
		}

		public void FireProjectile(int ownerID)
		{
			// drop out if firing is disabled
			if (!canFire)
				return;

			fireDirection = _TR.forward;

			// make our first projectile
			_theProjectile = MakeProjectile(ownerID);

			// point the projectile in the direction we want to fire in
			_theProjectile.LookAt(_TR.position + fireDirection);

			// add some force to move our projectile
			_theProjectile.GetComponent<Rigidbody>().AddRelativeForce(_theProjectile.forward * projectileSpeed, ForceMode.VelocityChange);
		}
	}
}