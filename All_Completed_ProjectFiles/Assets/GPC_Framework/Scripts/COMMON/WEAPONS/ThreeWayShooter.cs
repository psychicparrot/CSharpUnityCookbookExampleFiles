// this script simply fires a projectile when the fire button is pressed

using UnityEngine;
using System.Collections;

namespace GPC
{

	[AddComponentMenu("CSharpBookCode/Common/Weapons/Three-way Projectile Shooter WeaponScript")]

	public class ThreeWayShooter : BaseWeaponScript
	{
		private Vector3 offsetSideFireVector;

		public override void FireProjectile(Vector3 fireDirection, int ownerID)
		{
			// make our first projectile
			_theProjectile = MakeProjectile(ownerID);

			// point the projectile in the direction we want to fire in
			_theProjectile.LookAt(_theProjectile.position + fireDirection);

			// add some force to move our projectile
			_theProjectile.GetComponent<Rigidbody>().velocity = fireDirection * projectileSpeed;

			// -----------------------------------------------------

			offsetSideFireVector = new Vector3(fireDirection.z * 45, 0, 0);

			// make our second projectile
			_theProjectile = MakeProjectile(ownerID);

			// point the projectile in the direction we want to fire in
			_theProjectile.LookAt(_theProjectile.position + fireDirection);

			// rotate it a little to the side
			_theProjectile.Rotate(0, 25, 0);

			// add some force to move our projectile
			_theProjectile.GetComponent<Rigidbody>().velocity = offsetSideFireVector + fireDirection * projectileSpeed;

			// -----------------------------------------------------

			// make our second projectile
			_theProjectile = MakeProjectile(ownerID);

			// point the projectile in the direction we want to fire in
			_theProjectile.LookAt(_theProjectile.position + fireDirection);

			// rotate it a little to the side
			_theProjectile.Rotate(0, -25, 0);

			// add some force to move our projectile
			_theProjectile.GetComponent<Rigidbody>().velocity = -offsetSideFireVector + fireDirection * projectileSpeed;

			// -----------------------------------------------------

			// tell our sound controller to play a pew sound
			//BaseSoundController.instance.PlaySoundByIndex(0,_theProjectile.position);	
		}
	}
}