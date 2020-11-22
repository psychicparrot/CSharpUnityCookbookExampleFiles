using System.Collections;
using UnityEngine;

public class TireEffectController : MonoBehaviour
{
	public Rigidbody _RB;
	private Transform _TR;

	private float slideSpeed;
	private Vector3 velo;
	private Vector3 flatVelo;
	private Vector3 myRight;

	[Header("Slide particle Settings")]
	public float minSmokeSpeed = 2f;
	public ParticleSystem theTireParticles;
	public ParticleSystem.EmissionModule emission_module;

	[Header("Slide sound Settings")]
	public float minScreechSpeed = 2f; 
	public float maxSpeed = 3f;
	public float maxVolume = 0.5f;
	public AudioSource tiresAudioSource;

	void Start()
	{
		_TR = _RB.transform;

		emission_module = theTireParticles.emission;
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
		slideSpeed = Mathf.Abs(Vector3.Dot(myRight, flatVelo));

		if (slideSpeed > minSmokeSpeed)
		{
			// enable emission of tire smoke when we're sliding at a speed about smokeSpeed
			emission_module.enabled = true;

			if (slideSpeed > minScreechSpeed)
			{
				// figure out a percentage amount from the current slide speed vs. max slidespeed
				float percentage = slideSpeed / maxSpeed;
				float tireVolume = Mathf.Lerp(0, maxVolume, percentage);

				// set volume of tire screech
				tiresAudioSource.volume = tireVolume;

				// we'll also play some screeching audio here
				if (!tiresAudioSource.isPlaying)
				{
					tiresAudioSource.Play();
				}
			}
		} else
		{
			// as we're not sliding fast enough, stop emission of tire smoke
			emission_module.enabled = false;
		}
	}
}
