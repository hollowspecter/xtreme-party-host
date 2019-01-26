using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UsePPSound : MonoBehaviour 
{
	public PPSoundEvent PartyPersonSound;
    
	private AudioSource _mouth;

	private void Awake()
	{
		_mouth = GetComponent<AudioSource>();
		PartyPersonSound.volumeRangePos = Random.Range(0.2f, 1f);
		PartyPersonSound.pitchRangePos = Random.Range(0.5f, 2);
		
		Debug.Log("my volume is: " + PartyPersonSound.volumeRangePos);
		Debug.Log("my pitch is: " + PartyPersonSound.pitchRangePos);
	}

	private void Update()
	{
		if (Input.GetKeyDown("p"))
		{
			PartyPersonSound.PukingSound(_mouth);
			Debug.Log("Puking");
		}

		if (Input.GetKeyDown("o"))
		{
			PartyPersonSound.ShoutingSound(_mouth);
			Debug.Log("Shouting");
		}

		if (Input.GetKeyDown("i"))
		{
			PartyPersonSound.HappySound(_mouth);
			Debug.Log("Happy");
		}
	}
}
