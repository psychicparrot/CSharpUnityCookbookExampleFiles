using UnityEngine;
using GPC;
public class BlasterPlayer : BasePlayerStatsController
{
	public LayerMask projectileLayerMask;
	public Transform explosionPrefab;

	[Header("Respawn")]
	public GameObject playerAvatarParent;
	public float respawnTime = 2f;
	public BasePlayerCharacterController _controller;

	[Header("Weapon")]
	public StandardSlotWeaponController _weaponControl;
	public bool allowedToFire = true;

	public override void Init()
	{
		base.Init();
		SetHealth(1);

		_TR = transform;
		_controller = GetComponent<BasePlayerCharacterController>();
	}

	void Update()
	{
		if (_controller._inputController.Fire1)
			_weaponControl.Fire();
	}

	public void OnCollisionEnter(Collision collider)
	{
		if (IsInLayerMask(collider.gameObject.layer, projectileLayerMask) && !_controller.isRespawning)
		{
			ReduceHealth(1);

			if (GetHealth() <= 0)
			{
				Instantiate(explosionPrefab, _TR.position, _TR.rotation);

				// need to respawn, or reload the current level here.. life lost!
				PlayerDestroyed();
			}
		}
	}

	void PlayerDestroyed()
	{
		_controller.isRespawning = true;

		// hide the avatar / visual representation of a player!
		playerAvatarParent.SetActive(false);

		// reduce this player's lives by 1
		ReduceLives(1);

		// tell game manager that the player was destroyed
		BlasterGameManager.instance.PlayerDestroyed();
	}
}
