using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "PP Gen/Person")]
public class PPGenEvent : PartyPersonGenerator
{
	public GameObject PartyPerson;
	public GameObject[] HatPool;


	[Range(-20, 20)] private Vector3 _randomAngles;


	public override void PPGEn(Vector3 spawnPoint)
	{
		var PPCharacter = Instantiate(PartyPerson, spawnPoint, Quaternion.identity);

		
		if (HatPool.Length == 0) return;
		
		var randomHatPick = Random.Range(0, HatPool.Length);
		var hatSpawnPos = PPCharacter.transform.Find("HatAttachPoint").transform.position;  //hatAttachPoint; Find("HatAttachPoint").transform

		var newHat = Instantiate(HatPool[randomHatPick], hatSpawnPos, Quaternion.identity);
		newHat.transform.parent = PPCharacter.transform;

	}

}
