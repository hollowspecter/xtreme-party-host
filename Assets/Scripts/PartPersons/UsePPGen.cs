using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UsePPGen : MonoBehaviour
{
	public PPGenEvent PPGenerator;

	private Transform _childTransform;
	private AudioSource _bellSource;
	private int _personAmt;
	
	private void Awake()
	{
		_bellSource = this.GetComponent<AudioSource>();

	}

	// Update is called once per frame
	protected virtual void Update () 
	{
		if (Input.GetKeyDown("0"))
		{
			_bellSource.Play();
			PPGenerator.PPGEn(gameObject.transform.position);
			Debug.Log("Person " + _personAmt + " spawned!");
			_personAmt++;
		}
		
		
	}
}
