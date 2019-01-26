using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UsePPGen : MonoBehaviour
{
	public PPGenEvent PPGenerator;
    public int maximumPP = 20;
    public Vector2 minMaxSpawnIntervals = new Vector2(10f, 30f);

	private Transform _childTransform;
	private AudioSource _bellSource;
	private int _personAmt;
    
	
	private void Awake()
	{
		_bellSource = this.GetComponent<AudioSource>();
	}

    private void Start()
    {
        Spawn();
    }

    private void DoInvoke()
    {
        float time = Random.Range(minMaxSpawnIntervals.x, minMaxSpawnIntervals.y);
        Invoke("Spawn", time);
    }

    protected virtual void Spawn()
    {
        if (_personAmt <= maximumPP)
            DoInvoke();

        _bellSource.Play();
        PPGenerator.PPGEn(gameObject.transform.position);
        Debug.Log("Person " + _personAmt + " spawned!");
        _personAmt++;
    }
}
