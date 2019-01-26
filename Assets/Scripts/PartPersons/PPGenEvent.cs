using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "PP Gen")]
public class PPGenEvent : PartyPersonGenerator
{
	public GameObject[] HatPool;
	public AudioClip[] PukePool;
	public AudioClip[] ShoutingPool;
	public AudioClip[] HappyPool;
	public float rRange = 0.8f;

	[Range(0, 1)] public float volumeRangePos;
	[Range(0.5f, 2)] public float pitchRangePos;

	[Range(-20, 20)] private Vector3 _randomAngles;


	public override void PPGEn(Vector3 hatAttachPoint, Transform parentTransform)
	{
		if (HatPool.Length == 0) return;

		var randomHatPick = Random.Range(0, HatPool.Length);
		var spawnPos = hatAttachPoint;

		var newHat = Instantiate(HatPool[randomHatPick], spawnPos, Quaternion.identity);
		newHat.transform.parent = parentTransform;

	}

	public override void PukingSound(AudioSource source)
	{
		if (PukePool.Length == 0) return;

		source.clip = PukePool[Random.Range(0, PukePool.Length)];
		source.volume = Random.Range(volumeRangePos - rRange, volumeRangePos + rRange);
		source.pitch = Random.Range(pitchRangePos - rRange, pitchRangePos + rRange);
		source.Play();
	}

	public override void ShoutingSound(AudioSource source)
	{
		if (ShoutingPool.Length == 0) return;
		
		source.clip = ShoutingPool[Random.Range(0, ShoutingPool.Length)];
		source.volume = Random.Range(volumeRangePos - rRange, volumeRangePos + rRange);
		source.pitch = Random.Range(pitchRangePos - rRange, pitchRangePos + rRange);
		source.Play();

	}

	public override void HappySound(AudioSource source)
	{
		if (HappyPool.Length == 0) return;
		
		source.clip = HappyPool[Random.Range(0, HappyPool.Length)];
		source.volume = Random.Range(volumeRangePos - rRange, volumeRangePos + rRange);
		source.pitch = Random.Range(pitchRangePos - rRange, pitchRangePos + rRange);
		source.Play();
		
	}
}
