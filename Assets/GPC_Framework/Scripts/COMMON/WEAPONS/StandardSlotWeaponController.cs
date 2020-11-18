using System;
using UnityEngine;

namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Common/Weapons/Standard Slot Controller")]

	public class StandardSlotWeaponController : BaseWeaponController
	{
		public bool allowWeaponSwitchKeys = true;

		public void Update()
		{
			if (!allowWeaponSwitchKeys)
				return;

			// do weapon selection / switching slots
			// ---------------------------------------
			if (Input.GetKey(KeyCode.Less))
				PrevWeaponSlot(true);
			else if (Input.GetKey(KeyCode.Greater))
				NextWeaponSlot(true);

			// keys 1-9
			string theKey = Input.inputString;
			if (theKey == "")
				return;

			var val = (Char.ConvertToUtf32(theKey, 0)-49);
			if(val>-1 && val<_weapons.Length)
				SetWeaponSlot(val);
		}
	}
}