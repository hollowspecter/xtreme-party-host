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

    public void Puke() { PartyPersonSound.PukingSound(_mouth); }

    public void Shout() { PartyPersonSound.ShoutingSound(_mouth); }

    public void Happy() { PartyPersonSound.HappySound(_mouth); }


}
