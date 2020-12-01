using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    private Text m_Text;
    private int m_Level = 0;

    private void Start()
    {
        m_Text = GetComponent<Text>();
        m_Text.text = m_Level.ToString("00");
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
        m_Level++;
        m_Text.text = m_Level.ToString("00");
    }
}
