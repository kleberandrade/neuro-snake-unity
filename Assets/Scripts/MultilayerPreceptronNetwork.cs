public class MultiLayerPreceptronNetwork 
{
    public double LearnRate { get; set; }
    public double[] Inputs { get; set; }
    public Neuron[] HiddenLayer { get; set; }
    public Neuron[] OutputLayer { get; set; }
    public double[] DesiredOutputs { get; set; }

    public MultiLayerPreceptronNetwork(int inputLayerAmount, int hiddenLayerAmount, int outLayerAmount, double learnRate)
    {
        LearnRate = learnRate;
        HiddenLayer = new Neuron[hiddenLayerAmount];
        for (int i = 0; i < hiddenLayerAmount; i++)
        {
            HiddenLayer[i] = new Neuron(LearnRate);
            HiddenLayer[i].Inputs = new double[inputLayerAmount];
            HiddenLayer[i].RandomWeights();
        }

        OutputLayer = new Neuron[outLayerAmount];
        for (int i = 0; i < outLayerAmount; i++)
        {
            OutputLayer[i] = new Neuron(LearnRate);
            OutputLayer[i].Inputs = new double[hiddenLayerAmount];
            OutputLayer[i].RandomWeights();
        }
    }

    public void Forward()
    {
        double[] hiddenOutput = new double[HiddenLayer.Length];
        for (int i = 0; i < HiddenLayer.Length; i++)
        {
            HiddenLayer[i].Forward();
            hiddenOutput[i] = HiddenLayer[i].Output;
        }

        SetInputOnOutputLayer(hiddenOutput);
        for (int i = 0; i < OutputLayer.Length; i++)
        {
            OutputLayer[i].Forward();
        }
    }

    public void SetInputOnOutputLayer(double[] inputs)
    {
        for (int i = 0; i < OutputLayer.Length; i++)
            OutputLayer[i].Inputs = inputs;
    }

    public double[] GetOutputs()
    {
        double[] outputs = new double[OutputLayer.Length];
        for (int i = 0; i < OutputLayer.Length; i++)
            outputs[i] = OutputLayer[i].Output;
        return outputs;
    }

    public void SetInputOnHiddenLayer(double[] inputs)
    {
        for (int i = 0; i < HiddenLayer.Length; i++)
            HiddenLayer[i].Inputs = inputs;
    }

    public double[] Calculate(double[] inputs)
    {
        SetInputOnHiddenLayer(inputs);
        Forward();
        return GetOutputs();
    }

    private void CalculateHiddenLayerErrors()
    {
        for (int i = 0; i < HiddenLayer.Length; i++)
        {
            double sum = 0.0;
            for (int j = 0; j < OutputLayer.Length; j++)
                sum += OutputLayer[j].BackPropagatedError * OutputLayer[j].Weights[i + 1];

            HiddenLayer[i].Error = sum;
            HiddenLayer[i].CalculateBackPropagatedError();
        }
    }

    public void Backward(double[] inputs, double[] desiredOutputs)
    {
        Inputs = inputs;
        SetInputOnHiddenLayer(inputs);
        DesiredOutputs = desiredOutputs;
        Forward();

        for (int i = 0; i < OutputLayer.Length; i++)
        {
            OutputLayer[i].DesiredOutput = desiredOutputs[i];
            OutputLayer[i].CalculateError();
            OutputLayer[i].CalculateBackPropagatedError();
        }

        CalculateHiddenLayerErrors();
        for (int i = 0; i < HiddenLayer.Length; i++)
            HiddenLayer[i].WeightsAdjustment();

        for (int i = 0; i < OutputLayer.Length; i++)
            OutputLayer[i].WeightsAdjustment();
    }
}
