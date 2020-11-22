using UnityEngine;
using GPC;

public class RacePlayerController : RaceController
{
	// this userID ID number ties up with the player data in UserDataManager
	public int userID;
	public int myRacePosition;
	public float stuckResetTime = 3f;
	public float stuckTimer;
	public bool blockRespawn;
	[Space]
	public BaseWheeledVehicle _myController;
	public BaseAIController _AIController;
	public bool AIControlled;
	private float timedif;
	public LayerMask respawnerLayerMask;

	void Start()
	{
		Init();
	}

	// Update is called once per frame
	public override void Init()
	{
		base.Init();

		ResetLapCounter();
		_myController = GetComponent<BaseWheeledVehicle>();

		didInit = true;
	}

	void LateUpdate()
	{
		// tell race control to see if we're going the wrong way
		CheckWrongWay();

		if (goingWrongWay)
		{
			if (!AIControlled)
				RaceGameManager.instance.UpdateWrongWay(true);

			// if going the wrong way, compare time since wrong way started to see if we need to respawn
			if (timeWrongWayStarted != -1)
				timedif = Time.time - timeWrongWayStarted;

			if (timedif > stuckResetTime)
			{
				// it's been x resetTime seconds in the wrong way, let's respawn this thing!
				Respawn();
			}
		}
		else if (!AIControlled)
		{
			RaceGameManager.instance.UpdateWrongWay(false);
		}

		// stuck timer
		if(_myController.mySpeed<0.1f && _myController.canControl)
		{
			stuckTimer += Time.deltaTime;
			
			if (stuckTimer > stuckResetTime)
			{
				// it's been x resetTime seconds in the wrong way, let's respawn this thing!
				Respawn();
				stuckTimer = 0;
			}
		} else
		{
			stuckTimer = 0;
		}

		UpdateWaypoints();

		// check to see if this is the last player and the race is now complete (and tell game manager if it is!)
		if (GlobalRaceManager.instance.raceAllDone)
		{
			RaceGameManager.instance.RaceComplete();
		}
		else
		{
			// we only update the position during the game. once the race is done, we stop calculating it
			UpdateRacePosition();
		}
	}

	public void UpdateRacePosition()
	{
		// grab our race position from the global race manager
		myRacePosition = GlobalRaceManager.instance.GetPosition(this);
	}

	void Respawn()
	{
		if (blockRespawn)
			return;

		// reset our velocities so that we don't reposition a spinning vehicle
		_myController._RB.velocity = Vector3.zero;
		_myController._RB.angularVelocity = Vector3.zero;

		// get the waypoint to respawn at from the race controller
		Transform _tempTR = GetRespawnWaypointTransform();
		Vector3 tempVEC = _tempTR.position;

		// cast a ray down from the waypoint to try to find the ground
		RaycastHit hit;
		if (Physics.Raycast(tempVEC + (Vector3.up * 10), -Vector3.up, out hit))
		{
			tempVEC.y = hit.point.y + 2.5f;
		}

		_myController._TR.rotation = _tempTR.rotation;
		_myController._TR.position = tempVEC;

		blockRespawn = true;
		Invoke("ResetRespawnBlock", 5f);
	}

	void ResetRespawnBlock()
	{
		blockRespawn = false;
	}

	public override void PlayerFinishedRace()
	{
		if (!AIControlled)
		{
			// make sure we give control over to AI if it's not already
			AIControlled = true;
			GetComponent<RaceInputController>().SetInputType(RaceInputController.InputTypes.noInput);
			GetComponent<AISteeringController>().AIControlled = true;
			_AIController.canControl = true;
		}
	}

	public void SetLock(bool state)
	{
		if (!didInit)
			Init();

		_myController.canControl = !state;
	}

	public override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);

		if (IsInLayerMask(other.gameObject.layer, respawnerLayerMask))
		{
			Respawn();
		}
	}
}
