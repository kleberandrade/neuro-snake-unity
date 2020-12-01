using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public bool m_UseManualInput;
    public float m_TimeRate = 1.0f;
    private Snake m_Snake;

    [Header("Body")]
    public Body m_LastBody;

    [Header("Sensors")]
    public GameObject m_FoodSensor;
    public GameObject m_WallSensor;
    public GameObject m_BodySensor;
    public int m_Amount = 8;
    private List<Sensor> m_Sensors = new List<Sensor>();

    private void Start()
    {
        m_Snake = GetComponent<Snake>();
        m_Snake.TimeRate = m_TimeRate;

        for (int i = 0; i < m_Amount; i++)
        {
            var rotation = new Vector3(0, 360.0f / m_Amount * i, 0);
            var position = transform.position;
            var foodSensor = Instantiate(m_FoodSensor, position, Quaternion.Euler(rotation), transform);
            var wallSensor = Instantiate(m_WallSensor, position - new Vector3(0, 0.25f, 0), Quaternion.Euler(rotation), transform);
            var bodySensor = Instantiate(m_BodySensor, position + new Vector3(0, 0.25f, 0), Quaternion.Euler(rotation), transform);

            m_Sensors.Add(foodSensor.GetComponent<Sensor>());
            m_Sensors.Add(wallSensor.GetComponent<Sensor>());
            m_Sensors.Add(bodySensor.GetComponent<Sensor>());
        }
    }


    private void Update()
    {
        if (m_UseManualInput) ManualInput();
    }

    private void ManualInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_Snake.Left = 1.0f;
            m_Snake.Right = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_Snake.Left = 0.0f;
            m_Snake.Right = 1.0f;
        }
    }
}
