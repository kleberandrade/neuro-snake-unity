﻿using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    private Text m_Text;
    private int m_Score = 0;

    private void Start()
    {
        m_Text = GetComponent<Text>();
        m_Text.text = m_Score.ToString("0000"); 
    }

    private void OnEnable()
    {
        Food.OnFoodEaten += OnFoodEaten;
    }

    private void OnDisable()
    {
        Food.OnFoodEaten -= OnFoodEaten;
    }

    private void OnFoodEaten()
    {
        m_Score += 10;
        m_Text.text = m_Score.ToString("0000");
    }
}
