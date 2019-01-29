using UnityEngine;

public class DiscoballSpin : MonoBehaviour
{
	public float spinSpeed = 0.5f;

	private float ySpin;
	private Vector3 _initRot;

	private void Awake()
	{
		_initRot = transform.localEulerAngles;
	}

	void LateUpdate ()
	{
		ySpin = Time.time * spinSpeed;
		this.transform.eulerAngles = new Vector3(_initRot.x,ySpin,_initRot.z);
	}
}
