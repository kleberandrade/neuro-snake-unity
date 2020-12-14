using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public bool m_UseManualInput;
    public float m_TimeRate = 1.0f;
    private Snake m_Snake;

    [Header("Sensors")]
    public GameObject m_FoodSensor;
    public GameObject m_WallSensor;
    public GameObject m_BodySensor;
    public int m_Amount = 8;
    private List<Sensor> m_Sensors = new List<Sensor>();

    [Header("Sample")]
    public int m_BatchSize = 8;
    private int m_BatchCount = 0;
    public HashSet<DataSet> m_Samples = new HashSet<DataSet>();

    [Header("Neural Network")]
    private MultiLayerPreceptronNetwork m_Net;

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

    private void OnEnable()
    {
        Snake.OnMove += OnMove;
        Snake.OnDie += OnDie;
    }

    private void OnDisable()
    {
        Snake.OnMove -= OnMove;
        Snake.OnDie -= OnDie;
    }

    private void OnMove()
    {
        double[] outputs = new double[3];
        outputs[0] = m_Snake.Left > 0.5f ? 1.0f : -1.0f;
        outputs[1] = m_Snake.Right > 0.5f ? 1.0f : -1.0f;
        outputs[0] = m_Snake.Left < 0.5f && m_Snake.Right < 0.5f ? 1.0f : -1.0f;

        double[] inputs = new double[m_Sensors.Count];
        for (int i = 0; i < m_Sensors.Count; i++)
            inputs[i] = m_Sensors[i].GetInverseDistance() * 2.0f - 1.0f;

        m_Samples.Add(new DataSet()
        {
            inputs = inputs,
            output = outputs
        });

        Debug.Log($"SET {m_Samples.Count}");
    }

    private void OnDie()
    {
        //m_Samples.RemoveAt(m_Samples.Count - 1);
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
