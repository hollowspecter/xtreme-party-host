using UnityEngine;

public abstract class PartyPersonGenerator : ScriptableObject
{
	public abstract void PPGEn(Transform spawnPoint, float maxStayAmt);

}