using UnityEngine;

[CreateAssetMenu(menuName = "PP Gen/Sound Handler")]
public class PPSoundEvent : PPSound 
{
	public AudioClip[] PukePool;
	public AudioClip[] ShoutingPool;
	public AudioClip[] HappyPool;
    public AudioClip PizzaCrunch;
    public AudioClip BackgroundChatter;
    public AudioClip BeerBotteClinging;
    public AudioClip FridgeBeer;
	public float rRange = 0.5f;

	public float volumeRangePos;
	public float pitchRangePos;
	

    public virtual void PlaySound(AudioSource source, AudioClip clip, float volumeScale)
    {
        source.PlayOneShot(clip, volumeScale);
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
