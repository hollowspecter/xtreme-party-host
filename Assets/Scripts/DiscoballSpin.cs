
using UnityEngine;

public class DiscoballSpin : MonoBehaviour
{
	public float spinSpeed = 0.5f;

	private float ySpin;

	void FixedUpdate ()
	{
		ySpin += Time.deltaTime * spinSpeed;
		this.transform.eulerAngles += new Vector3(0,ySpin,0);
	}
}
