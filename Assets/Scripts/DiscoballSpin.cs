
using UnityEngine;

public class DiscoballSpin : MonoBehaviour
{
	public float spinSpeed = 0.5f;

	void FixedUpdate ()
	{
		var ySpin = spinSpeed * Time.time;
		this.transform.eulerAngles += new Vector3(0,ySpin,0);
	}
}
