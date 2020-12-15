[System.Serializable]
public class MultiLayerPreceptronNetwork 
{
    public double m_LearnRate;
    public double[] m_Inputs;
    public Neuron[] m_HiddenLayer;
    public Neuron[] m_OutputLayer;
    public double[] m_DesiredOutputs;

    public MultiLayerPreceptronNetwork() { }

    public MultiLayerPreceptronNetwork(int inputLayerAmount, int hiddenLayerAmount, int outLayerAmount, double learnRate)
    {
        m_LearnRate = learnRate;
        m_HiddenLayer = new Neuron[hiddenLayerAmount];
        for (int i = 0; i < hiddenLayerAmount; i++)
        {
            m_HiddenLayer[i] = new Neuron(m_LearnRate);
            m_HiddenLayer[i].m_Inputs = new double[inputLayerAmount];
            m_HiddenLayer[i].RandomWeights();
        }

        m_OutputLayer = new Neuron[outLayerAmount];
        for (int i = 0; i < outLayerAmount; i++)
        {
            m_OutputLayer[i] = new Neuron(m_LearnRate);
            m_OutputLayer[i].m_Inputs = new double[hiddenLayerAmount];
            m_OutputLayer[i].RandomWeights();
        }
    }

    public void Forward()
    {
        double[] hiddenOutput = new double[m_HiddenLayer.Length];
        for (int i = 0; i < m_HiddenLayer.Length; i++)
        {
            m_HiddenLayer[i].Forward();
            hiddenOutput[i] = m_HiddenLayer[i].m_Output;
        }

        SetInputOnOutputLayer(hiddenOutput);
        for (int i = 0; i < m_OutputLayer.Length; i++)
        {
            m_OutputLayer[i].Forward();
        }
    }

    public void SetInputOnOutputLayer(double[] inputs)
    {
        for (int i = 0; i < m_OutputLayer.Length; i++)
            m_OutputLayer[i].m_Inputs = inputs;
    }

    public double[] GetOutputs()
    {
        double[] outputs = new double[m_OutputLayer.Length];
        for (int i = 0; i < m_OutputLayer.Length; i++)
            outputs[i] = m_OutputLayer[i].m_Output;
        return outputs;
    }

    public void SetInputOnHiddenLayer(double[] inputs)
    {
        for (int i = 0; i < m_HiddenLayer.Length; i++)
            m_HiddenLayer[i].m_Inputs = inputs;
    }

    public double[] Calculate(double[] inputs)
    {
        SetInputOnHiddenLayer(inputs);
        Forward();
        return GetOutputs();
    }

    private void CalculateHiddenLayerErrors()
    {
        for (int i = 0; i < m_HiddenLayer.Length; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < m_OutputLayer.Length; j++)
                sum += m_OutputLayer[j].m_BackPropagatedError * m_OutputLayer[j].m_Weights[i + 1];

            m_HiddenLayer[i].m_Error = sum;
            m_HiddenLayer[i].CalculateBackPropagatedError();
        }
    }

    public void Backward(double[] inputs, double[] desiredOutputs)
    {
        m_Inputs = inputs;
        SetInputOnHiddenLayer(inputs);
        m_DesiredOutputs = desiredOutputs;
        Forward();

        for (int i = 0; i < m_OutputLayer.Length; i++)
        {
            m_OutputLayer[i].m_DesiredOutput = desiredOutputs[i];
            m_OutputLayer[i].CalculateError();
            m_OutputLayer[i].CalculateBackPropagatedError();
        }

        CalculateHiddenLayerErrors();
        for (int i = 0; i < m_HiddenLayer.Length; i++)
            m_HiddenLayer[i].WeightsAdjustment();

        for (int i = 0; i < m_OutputLayer.Length; i++)
            m_OutputLayer[i].WeightsAdjustment();
    }
}
