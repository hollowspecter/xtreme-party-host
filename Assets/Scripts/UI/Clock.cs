using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Clock : MonoBehaviour
{
    public static Clock instance;

    [Header("Colors")]
    public Color lineColor;
    public Color glowColor;
    [Header("Components")]
    public Image outline;
    public Image outlineGlow;

    public TextMeshProUGUI clockText;

    DateTime time;
    public float simulationTimeFactor = 20f;
    public float timeUntilPartyEnd = 300f;
    
    //buffer, so Party People have time to leave/give points.
    [SerializeField] private float _partyEndBuffer = 10f;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        outline.color = lineColor;
        outlineGlow.color = glowColor;
        clockText.color = lineColor;
        clockText.fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, glowColor);
        time = new DateTime(1,1,1, 20, 0, 0);
        timeUntilPartyEnd += _partyEndBuffer;
    }

    private void Update()
    {
        var timeTick = Time.deltaTime * simulationTimeFactor;
        timeUntilPartyEnd = timeUntilPartyEnd - timeTick;
        
        time = time.AddMinutes(timeTick);
        clockText.text = time.ToString("HH:mm");
        
        if (_partyEndBuffer > 0) return;
            //final score is set and ending screen will be shown



    }
    
    
}
