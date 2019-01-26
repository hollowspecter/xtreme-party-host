using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UsePPGen : MonoBehaviour
{
	public PPGenEvent PPGenerator;

	private Transform _childTransform;
	private AudioSource _mouth;
	
	private void Awake()
	{
		_mouth = GetComponent<AudioSource>();
		_childTransform = transform.Find("HatAttachPoint").transform;
		PPGenerator.PPGEn(_childTransform.position, gameObject.transform);

		PPGenerator.volumeRangePos = Random.Range(0.2f, 1f);
		PPGenerator.pitchRangePos = Random.Range(0.5f, 2);
		
		Debug.Log("my volume is: " + PPGenerator.volumeRangePos);
		Debug.Log("my pitch is: " + PPGenerator.pitchRangePos);
	}

	// Update is called once per frame
	protected virtual void Update () 
	{
		if (Input.GetKeyDown("p"))
		{
			PPGenerator.PukingSound(_mouth);
			Debug.Log("Puking");
		}

		if (Input.GetKeyDown("o"))
		{
			PPGenerator.ShoutingSound(_mouth);
			Debug.Log("Shouting");
		}

		if (Input.GetKeyDown("i"))
		{
			PPGenerator.HappySound(_mouth);
			Debug.Log("Happy");
		}

		
	}
}
