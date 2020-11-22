using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPC;

public class RunManCharacterController : BasePlayer2DPlatformCharacter
{
	public BaseSoundManager _soundControl;
	public Animator _RunManAnimator;
	public bool isRunning;

	public BasePlayerStatsController _playerStats;
	void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		isRunning = false;
		allow_jump = true;
		allow_right = true;
		allow_left = true;

		_soundControl = GetComponent<BaseSoundManager>();
	}

	public void Update()
	{
		// if we're running (ie game has started), we're on the ground and the animation that's playing isn't running.. play the run animation!
		if (isOnGround && isRunning && !_RunManAnimator.GetCurrentAnimatorStateInfo(0).IsName("RunMan_Run"))
		{
			StartRunAnimation();
		}
	}

	public override void LateUpdate()
	{
		if(canControl)
			base.LateUpdate();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		_soundControl.PlaySoundByIndex(1);
		RunManGameManager.instance.PlayerFell();
	}

	public void StartRunAnimation()
	{
		isRunning = true;
		_RunManAnimator.SetTrigger("Run");
	}

	public override void Jump()
	{
		base.Jump();
		_soundControl.PlaySoundByIndex(0);
		_RunManAnimator.SetTrigger("Jump");
	}
}
