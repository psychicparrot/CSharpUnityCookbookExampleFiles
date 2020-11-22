using UnityEngine;
using System.Collections.Generic;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Base/Base Weapon Controller")]

	public class BaseWeaponController : ExtendedCustomMonoBehaviour
	{
		public GameObject[] _weapons;
		public int selectedWeaponSlot;
		public int lastSelectedWeaponSlot;
		public Transform _weaponMountPoint;
		private List<GameObject> _weaponSlots;
		private List<BaseWeaponScript> _weaponScripts;
		private BaseWeaponScript _TEMPWeapon;
		private int ownerNum;
		public bool useForceVectorDirection;
		public Vector3 forceVector;
		private Vector3 theDir;

		public void Start()
		{
			// default to the first weapon slot
			selectedWeaponSlot = 0;
			lastSelectedWeaponSlot = -1;

			// initialize weapon list List
			_weaponSlots = new List<GameObject>();

			// initialize weapon scripts List
			_weaponScripts = new List<BaseWeaponScript>();

			_TR = transform;

			if (_weaponMountPoint == null)
				_weaponMountPoint = _TR;

			for (int i = 0; i < _weapons.Length; i++)
			{
				GameObject _tempGO = (GameObject)Instantiate(_weapons[i], _weaponMountPoint.position, _weaponMountPoint.rotation);
				_tempGO.transform.parent = _weaponMountPoint;
				_tempGO.layer = _weaponMountPoint.gameObject.layer;
				_tempGO.transform.position = _weaponMountPoint.position;
				_tempGO.transform.rotation = _weaponMountPoint.rotation;

				// store a reference to the gameObject in a List
				_weaponSlots.Add(_tempGO);

				// grab a reference to the weapon script attached to the weapon and store the reference in a List
				_TEMPWeapon = _tempGO.GetComponent<BaseWeaponScript>();
				_weaponScripts.Add(_TEMPWeapon);

				// disable the weapon
				_tempGO.SetActive(false);
			}

			// now we set the default selected weapon to visible
			SetWeaponSlot(0);
		}

		public void SetOwner(int aNum)
		{
			// used to identify the object firing, if required
			ownerNum = aNum;
		}

		public virtual void SetWeaponSlot(int slotNum)
		{
			// if the selected weapon is already this one, drop out!
			if (slotNum == lastSelectedWeaponSlot)
				return;

			// disable the current weapon
			DisableCurrentWeapon();

			// set our current weapon to the one passed in
			selectedWeaponSlot = slotNum;

			// make sure sensible values are getting passed in
			if (selectedWeaponSlot < 0)
				selectedWeaponSlot = _weaponSlots.Count - 1;

			// make sure that the weapon slot isn't higher than the total number of weapons in our list
			if (selectedWeaponSlot > _weaponSlots.Count - 1)
				selectedWeaponSlot = _weaponSlots.Count - 1;

			// we store this selected slot to use to prevent duplicate weapon slot setting
			lastSelectedWeaponSlot = selectedWeaponSlot;

			// enable the newly selected weapon
			EnableCurrentWeapon();
		}

		public virtual void NextWeaponSlot(bool shouldLoop)
		{
			// disable the current weapon
			DisableCurrentWeapon();

			// next slot
			selectedWeaponSlot++;

			// make sure that the slot isn't higher than the total number of weapons in our list
			if (selectedWeaponSlot >= _weaponScripts.Count)
			{
				if (shouldLoop)
				{
					selectedWeaponSlot = 0;
				}
				else
				{
					selectedWeaponSlot = _weaponScripts.Count - 1;
				}
			}

			// we store this selected slot to use to prevent duplicate weapon slot setting
			lastSelectedWeaponSlot = selectedWeaponSlot;

			// enable the newly selected weapon
			EnableCurrentWeapon();
		}

		public virtual void PrevWeaponSlot(bool shouldLoop)
		{
			// disable the current weapon
			DisableCurrentWeapon();

			// prev slot
			selectedWeaponSlot--;

			// make sure that the slot is a sensible number
			if (selectedWeaponSlot < 0)
			{
				if (shouldLoop)
				{
					selectedWeaponSlot = _weaponScripts.Count - 1;
				}
				else
				{
					selectedWeaponSlot = 0;
				}
			}

			// we store this selected slot to use to prevent duplicate weapon slot setting
			lastSelectedWeaponSlot = selectedWeaponSlot;

			// enable the newly selected weapon
			EnableCurrentWeapon();
		}


		public virtual void DisableCurrentWeapon()
		{
			if (_weaponScripts.Count == 0)
				return;

			// grab reference to currently selected weapon script
			_TEMPWeapon = (BaseWeaponScript)_weaponScripts[selectedWeaponSlot];

			// now tell the script to disable itself
			_TEMPWeapon.Disable();

			// grab reference to the weapon's gameObject and disable that, too
			GameObject _tempGO = (GameObject)_weaponSlots[selectedWeaponSlot];
			_tempGO.SetActive(false);
		}

		public virtual void EnableCurrentWeapon()
		{
			if (_weaponScripts.Count == 0)
				return;

			// grab reference to currently selected weapon
			_TEMPWeapon = (BaseWeaponScript)_weaponScripts[selectedWeaponSlot];

			// now tell the script to enable itself
			_TEMPWeapon.Enable();

			GameObject _tempGO = (GameObject)_weaponSlots[selectedWeaponSlot];
			_tempGO.SetActive(true);
		}

		public virtual void Fire()
		{
			if (_weaponScripts == null)
				return;
			if (_weaponScripts.Count == 0)
				return;

			// find the weapon in the currently selected slot
			_TEMPWeapon = (BaseWeaponScript)_weaponScripts[selectedWeaponSlot];

			theDir = _TR.forward;

			if (useForceVectorDirection)
				theDir = forceVector;

			// fire the projectile
			_TEMPWeapon.Fire(theDir, ownerNum);
		}

	}
}