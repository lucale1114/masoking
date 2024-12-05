using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Action<float> ScoreChanged;

    private float _currentScore;

    public void AddScore(float score)
    {
        _currentScore += score;
        ScoreChanged?.Invoke(_currentScore);
    }
    
    public float GetScore()
    {
        return _currentScore;
    }
    // Update is called once per frame
}
