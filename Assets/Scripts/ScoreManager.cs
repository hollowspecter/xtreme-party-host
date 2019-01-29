using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance = null;

    public float Score = 0f;
    private float _functionalScore;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        //hope this helps fixing the score
        if (!float.IsInfinity(Score))
        {
            _functionalScore = Score;
        }
        else
        {
            Score = _functionalScore;
        }
            
    }
}
