using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public Transform pressButtonBox;
	
	
	private void Start()
	{
		pressButtonBox.DOLocalRotate(new Vector3(0,0,10f),0.8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBounce);

	}

	void Update () 
	{

		if (Input.anyKeyDown)
		{
			SceneManager.LoadScene("Level1v2",LoadSceneMode.Single);

		}
	}


	
}
