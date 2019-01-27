using System;
using Boo.Lang.Environments;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "PP Gen/Person")]
public class PPGenEvent : PartyPersonGenerator
{
	public GameObject PartyPerson;
	public GameObject[] HatPool;
	public Vector2 StayRange;
	public float myStayAmt;

	private float _minStayAmt = 30f;

	[Range(-20, 20)] private Vector3 _randomAngles;


	public override void PPGEn(Vector3 spawnPoint, float maxStayAmt)
	{
		var PPCharacter = Instantiate(PartyPerson, spawnPoint, Quaternion.identity);
		
		//Give each person a stay Amt
		myStayAmt = Random.Range(_minStayAmt, maxStayAmt);
		
		//VIVI Setze myStayAmt in den jeweilig gespawnten NPC
		if (HatPool.Length == 0) return;
		  

        var hatpointTransform = PPCharacter.GetComponent<ActionManager>().hatpoint.transform;
        var randomHatPick = Random.Range(0, HatPool.Length);
        var hatSpawnPos = hatpointTransform.position;
        //var hatSpawnPos = PPCharacter.transform.Find("HatAttachPoint").transform.position;  //hatAttachPoint; Find("HatAttachPoint").transform

		var newHat = Instantiate(HatPool[randomHatPick], hatSpawnPos, Quaternion.identity);
        newHat.transform.parent = hatpointTransform;

	}

}
