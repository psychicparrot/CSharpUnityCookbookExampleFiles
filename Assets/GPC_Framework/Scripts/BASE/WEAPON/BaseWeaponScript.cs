using UnityEngine;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Base Weapon Script")]

	public class BaseWeaponScript : ExtendedCustomMonoBehaviour
	{
		public bool canFire;
		public int ammo = 100;
		public int maxAmmo = 100;
		public bool isInfiniteAmmo;
		public GameObject _projectilePrefab;
		public Collider _parentCollider;
		public Transform _spawnPositionTR;
		public float fireDelay = 0.2f;
		public float projectileSpeed = 10f;
		public bool inheritVelocity;

		[System.NonSerialized]
		public Transform _theProjectile;

		private bool isLoaded;

		public virtual void Start()
		{
			Init();
		}

		public virtual void Init()
		{
			_TR = transform;
			Reloaded();
		}

		public virtual void Enable()
		{
			canFire = true;
		}

		public virtual void Disable()
		{
			canFire = false;
		}

		public virtual void Reloaded()
		{
			// the 'isLoaded' var tells us if this weapon is loaded and ready to fire
			isLoaded = true;
		}

		public virtual void SetCollider(Collider aCollider)
		{
			_parentCollider = aCollider;
		}

		public virtual void Fire(Vector3 aDirection, int ownerID)
		{
			// be sure to check canFire so that the weapon can be enabled or disabled as required!
			if (!canFire)
				return;

			// if the weapon is not loaded, drop out
			if (!isLoaded)
				return;

			// if we're out of ammo and we do not have infinite ammo, drop out..
			if (ammo <= 0 && !isInfiniteAmmo)
				return;

			// decrease ammo
			ammo--;

			// generate the actual projectile
			FireProjectile(aDirection, ownerID);

			// we need to reload before we can fire again
			isLoaded = false;

			CancelInvoke("Reloaded");
			Invoke("Reloaded", fireDelay);
		}

		public virtual void FireProjectile(Vector3 fireDirection, int ownerID)
		{
			// make our first projectile
			_theProjectile = MakeProjectile(ownerID);

			// direct the projectile toward the direction of fire
			_theProjectile.LookAt(_theProjectile.position + fireDirection);

			// add force to move our projectile
			_theProjectile.GetComponent<Rigidbody>().velocity = fireDirection * projectileSpeed;
		}

		public virtual Transform MakeProjectile(int ownerID)
		{
			// create a projectile
			_theProjectile = Spawn(_projectilePrefab.transform, _spawnPositionTR.position, _spawnPositionTR.rotation);
			_theProjectile.SendMessage("SetOwnerType", ownerID, SendMessageOptions.RequireReceiver);

			if (_parentCollider != null)
			{
				// disable collision between 'us' and our projectile so as not to hit ourselves with it!
				Physics.IgnoreCollision(_theProjectile.GetComponent<Collider>(), _parentCollider);
			}

			return _theProjectile;
		}
	}
}