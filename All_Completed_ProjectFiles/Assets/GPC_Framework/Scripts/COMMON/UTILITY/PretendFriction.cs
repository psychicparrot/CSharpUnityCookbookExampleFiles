using UnityEngine;
namespace GPC
{
	[AddComponentMenu("CSharpBookCode/Utility/Pretend Friction")]

	public class PretendFriction : MonoBehaviour
	{
		private Rigidbody _RB;
		private Transform _TR;
		private float myMass;
		private float slideSpeed;
		private Vector3 velo;
		private Vector3 flatVelo;
		private Vector3 myRight;
		private Vector3 TEMPvec3;

		public float theGrip = 100f;

		void Start()
		{
			// cache some references to our rigidbody, mass and transform
			_RB = GetComponent<Rigidbody>();
			myMass = _RB.mass;
			_TR = transform;
		}

		void FixedUpdate()
		{
			// grab the values we need to calculate grip
			myRight = _TR.right;

			// calculate flat velocity
			velo = _RB.velocity;
			flatVelo.x = velo.x;
			flatVelo.y = 0;
			flatVelo.z = velo.z;

			// calculate how much we are sliding
			slideSpeed = Vector3.Dot(myRight, flatVelo);

			// build a new vector to compensate for the sliding
			TEMPvec3 = myRight * (-slideSpeed * myMass * theGrip);

			// apply the correctional force to the rigidbody
			_RB.AddForce(TEMPvec3 * Time.deltaTime);
		}
	}
}