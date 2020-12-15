using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Brain : MonoBehaviour
{
    public bool m_UseManualInput;
    public float m_TimeRate = 1.0f;
    private Snake m_Snake;

    [Header("Sensors")]
    public GameObject m_FoodSensor;
    public GameObject m_WallSensor;
    public GameObject m_BodySensor;
    public int m_SensorNumber = 8;
    public float m_OffsetAngle = -135.0f;
    public float m_MaxAngle = 270.0f;
    private List<Sensor> m_Sensors = new List<Sensor>();

    [Header("Sample")]
    public int m_BatchSize = 8;
    private int m_BatchCount = 0;
    public List<DataSet> m_Samples = new List<DataSet>();

    [Header("Neural Network")]
    public bool m_CanTrain;
    public bool m_LoadNet;
    public string m_Filename = "net.dat";
    public double m_LearRate = 0.01;
    public int m_HiddenLayerAmount = 50;
    public int m_OutputLayerAmount = 2;
    public MultiLayerPreceptronNetwork m_Net;

    private void Start()
    {
        if (m_LoadNet)
        {
            m_Net = LoadNet();
        } 
        else
        {
            m_Net = new MultiLayerPreceptronNetwork(
                m_SensorNumber * 3,
                m_HiddenLayerAmount,
                m_OutputLayerAmount,
                m_LearRate
            );
        }

        m_Snake = GetComponent<Snake>();
        m_Snake.TimeRate = m_TimeRate;

        for (int i = 0; i < m_SensorNumber; i++)
        {
            //180 / 3 = 60

            var rotation = new Vector3(0, m_MaxAngle / (m_SensorNumber - 1) * i + m_OffsetAngle, 0);
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
        //outputs[2] = m_Snake.Left < 0.5f && m_Snake.Right < 0.5f ? 1.0f : -1.0f;

        double[] inputs = new double[m_Sensors.Count];
        for (int i = 0; i < m_Sensors.Count; i++)
            inputs[i] = m_Sensors[i].GetInverseDistance() * 2.0f - 1.0f;

        var dataset = new DataSet() { inputs = inputs, output = outputs };

        m_Net.m_Inputs = dataset.inputs;
        var output = m_Net.Calculate(dataset.inputs);
        Debug.Log($"Raw: {output[0]} | {output[1]}");

        if (m_CanTrain)
            Train(dataset);
    }

    private void AutoInput()
    {
        var outputs = m_Net.GetOutputs();

        Debug.Log($"Outputs: {outputs[0]} | {outputs[1]}");

        m_Snake.Left  = outputs[0] > 0 ? 1 : 0;
        m_Snake.Right = outputs[1] > 0 ? 1 : 0;
        /*
        if (outputs[2] > 0) 
        {
            m_Snake.Left = 0;
            m_Snake.Right = 0;
        }
        */
    }

    private void Train(DataSet dataset)
    {
        if (!m_Samples.Contains(dataset))//&& (dataset.output[0] > 0.5 || dataset.output[1] > 0.5))
        {
            m_Samples.Add(dataset);
            m_BatchCount++;
        }

        if (m_BatchCount == m_BatchSize)
        {
            Debug.Log($"Training with {m_Samples.Count} samples");

            m_BatchCount = 0;
            foreach (var data in m_Samples)
                m_Net.Backward(data.inputs, data.output);
        }
    }

    private void OnDie()
    {
        //m_Samples.RemoveAt(m_Samples.Count - 1);
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

    private void Update()
    {
        if (m_UseManualInput)
            ManualInput();
        else
            AutoInput();

        if (Input.GetKeyDown(KeyCode.S)) SaveNet();
        if (Input.GetKeyDown(KeyCode.R)) LoadNet();
    }

    private void SaveNet()
    {
        Debug.Log($"Saving net in {LocalPath + m_Filename}");
        using (StreamWriter writer = new StreamWriter(LocalPath + m_Filename))
        {
            string json = JsonUtility.ToJson(m_Net);
            writer.Write(json);
        }
    }

    private MultiLayerPreceptronNetwork LoadNet()
    {
        Debug.Log($"Loading net in {LocalPath + m_Filename}");
        string json = string.Empty;
        using (StreamReader reader = new StreamReader(LocalPath + m_Filename))
        {
            json = reader.ReadToEnd();
        }

        return JsonUtility.FromJson<MultiLayerPreceptronNetwork>(json);
    }

    public static string LocalPath
    {
        get
        {
            string path = "";
#if UNITY_EDITOR
            path = $"{Application.dataPath}/";
#else
            path = $"{Application.dataPath}/../";
#endif
            return path;
        }
    }
}
