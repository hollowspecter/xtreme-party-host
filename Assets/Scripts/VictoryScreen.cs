using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{

	public TextMeshProUGUI VictoryText;
	public TextMeshProUGUI Score;
	public float winScore = 10000;
	public String victory = "Epic Party!";
	public String fail = "worst Party EVAR!";

	private void Start()
	{
		var endScore = ScoreManager.instance.Score * 100f;
		if (endScore > winScore)
		{
			VictoryText.text = victory;

		}
	
		else
		{
			VictoryText.text = fail;
		}


        Score.text = "" + (int)endScore; //insert score
		
	}
	
	
}
