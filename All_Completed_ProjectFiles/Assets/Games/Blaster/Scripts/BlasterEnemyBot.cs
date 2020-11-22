using UnityEngine;
using GPC;

public class BlasterEnemyBot : AIBotController
{
	[Header("Game specific")]
	public bool isRespawning;
	public bool isBoss;

	[Header("Weapons and hits")]
	public BaseEnemyWeaponController _weaponController;
	public LayerMask projectileLayerMask;
	public Transform _explosionPrefab;

	public override void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();
		// now get on and chase it!
		SetAIState(AIStates.AIState.chasing_target);
	}

	public override void Update()
	{
		base.Update();
	}

	public void OnCollisionEnter(Collision collider)
	{
		if (IsInLayerMask(collider.gameObject.layer, projectileLayerMask))
		{
			ReduceHealth(1);
			if (GetHealth() <= 0)
			{
				Instantiate(_explosionPrefab, _TR.position, _TR.rotation);
				BlasterGameManager.instance.EnemyDestroyed();
				Destroy(gameObject);
			}
		}
	}
}
