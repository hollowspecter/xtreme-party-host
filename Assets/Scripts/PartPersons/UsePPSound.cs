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
		
		//Debug.Log("my volume is: " + PartyPersonSound.volumeRangePos);
		//Debug.Log("my pitch is: " + PartyPersonSound.pitchRangePos);
	}

    public void Puke() { PartyPersonSound.PukingSound(_mouth); }

    public void Shout() { PartyPersonSound.ShoutingSound(_mouth); }

    public void Happy() { PartyPersonSound.HappySound(_mouth); }

    public void PlayPizzaCrunch() { PartyPersonSound.PlaySound(_mouth, PartyPersonSound.PizzaCrunch, 1f); }
    public void PlayBackgroundChatter() { PartyPersonSound.PlaySound(_mouth, PartyPersonSound.BackgroundChatter, 1f); }
    public void PlayBeerBotteClinging() { PartyPersonSound.PlaySound(_mouth, PartyPersonSound.BeerBotteClinging, 1f); }
    public void PlayFridgeBeer() { PartyPersonSound.PlaySound(_mouth, PartyPersonSound.FridgeBeer, 0.7f); }
    public void PlayTelephoneCall() { PartyPersonSound.PlaySound(_mouth, PartyPersonSound.TelephoneCall, 1f); }
    public void PlayPizzaHere() { PartyPersonSound.PlaySound(_mouth, PartyPersonSound.PizzaHere, 1f); }
    public void PlayBeerHere() { PartyPersonSound.PlaySound(_mouth, PartyPersonSound.BeerHere, 1f); }
}
