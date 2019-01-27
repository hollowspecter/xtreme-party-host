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
		//if score is higher than XXX
		VictoryText.text = victory;
		
		//if score is lower than XXX
		VictoryText.text = fail;


		Score.text = ToString(); //insert score

	}
	
	
}
