using UnityEngine;
public class CrashSound : MonoBehaviour
{
	public AudioSource _crashAudioSource;
	public float magnitudeToPlay = 2f;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > magnitudeToPlay)
		{
			float aVol = 1 / magnitudeToPlay;
			_crashAudioSource.volume = 2 * aVol;
			_crashAudioSource.pitch = 1 + Random.Range(-0.5f, 0.5f);
			_crashAudioSource.Play();
		}
	}
}
