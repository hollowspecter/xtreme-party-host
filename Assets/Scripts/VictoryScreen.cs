using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class VictoryScreen : MonoBehaviour
{

	public TextMeshProUGUI VictoryText;
	public TextMeshProUGUI Score;
	public String victory = "Epic Party!";
	public String fail = "worst Party EVAR!";

	private void Start()
	{
		if (ScoreManager.instance.Score > 14)
		{
			VictoryText.text = victory;

		}
	
		else
		{
			VictoryText.text = fail;
		}


        Score.text = "" + (int)(ScoreManager.instance.Score * 103f); //insert score
		
	}
	
	
}
