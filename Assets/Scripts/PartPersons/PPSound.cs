using UnityEngine;

public abstract class PPSound : ScriptableObject
{
    public abstract void PukingSound(AudioSource audioSource);
    public abstract void ShoutingSound(AudioSource audioSource);
    public abstract void HappySound(AudioSource audioSource);
}
