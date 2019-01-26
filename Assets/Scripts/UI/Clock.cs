using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Clock : MonoBehaviour
{
    [Header("Colors")]
    public Color lineColor;
    public Color glowColor;
    [Header("Components")]
    public Image outline;
    public Image outlineGlow;

    public TextMeshProUGUI clockText;

    DateTime time;
    public float simulationTimeFactor = 20f;

	// Use this for initialization
	void Start ()
    {
        outline.color = lineColor;
        outlineGlow.color = glowColor;
        clockText.color = lineColor;
        clockText.fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, glowColor);
        time = new DateTime(1,1,1, 20, 0, 0);
    }

    private void Update()
    {
        Debug.Log(time.ToShortTimeString());
        time = time.AddMinutes(Time.deltaTime * simulationTimeFactor);
        clockText.text = time.ToString("HH:mm");
    }
}
