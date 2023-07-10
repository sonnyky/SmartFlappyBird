using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    private int _currentScore = 0;
    private Player m_Player;
    private void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_Player.OnPlayerSurvived += IncrementScore;
    }

    public void IncrementScore()
    {
        _currentScore++;
        Text scoreText = GetComponent<Text>();
        scoreText.text = "Score: " + _currentScore.ToString();
    }
}
