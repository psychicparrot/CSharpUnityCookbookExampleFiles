using UnityEngine;
public class ExtendedCustomMonoBehaviour : MonoBehaviour 
{
	[System.NonSerialized] 
	public Transform _TR;

	[System.NonSerialized] 
	public GameObject _GO;
	
	[System.NonSerialized] 
	public Rigidbody _RB;

	[System.NonSerialized] 
	public Rigidbody2D _RB2D;

	[Header("Initialization and ID")]
	public bool didInit;
	public bool canControl;

	[System.NonSerialized] 
	public int id;
	
	[System.NonSerialized]
	public Vector3 tempVEC;
	
	[System.NonSerialized]
	public Transform _tempTR;
	
	public virtual void SetID( int anID )
	{
		id= anID;
	}

	public virtual void GetComponents()
	{
		if (_TR == null)
			_TR = transform;

		if (_GO == null)
			_GO = gameObject;
	}

	public Transform Spawn(Transform spawnObject, Vector3 spawnPosition, Quaternion spawnRotation)
	{
		return Instantiate(spawnObject, spawnPosition, spawnRotation);
	}

	public static bool IsInLayerMask(int layer, LayerMask layermask)
	{
		return layermask == (layermask | (1 << layer));
	}
}
