using UnityEngine;

public abstract class PartyPersonGenerator : ScriptableObject
{
	public abstract void PPGEn(Vector3 hatAttachPoint, Transform parentTransform);

	public abstract void PukingSound(AudioSource audioSource);
	public abstract void ShoutingSound(AudioSource audioSource);
	public abstract void HappySound(AudioSource audioSource);
}


// class generator SO abstract
//generate
// needs hat pool
//random angle
//needs audio
//random range for audio pitch (min + randompitch, 