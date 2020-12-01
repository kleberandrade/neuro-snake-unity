using System;
using UnityEngine;

public class Neuron
{
    private double Bias { get; set; } = 1;
    public double LearnRate { get; set; }
    public double[] Inputs { get; set; }
    public double[] Weights { get; set; }
    public double Output { get; set; }
    public double DesiredOutput { get; set; }
    public double Error { get; set; }
    public double BackPropagatedError { get; set; }

    public Neuron(double learnRate)
    {
        LearnRate = learnRate;
    }

    public Neuron(double[] inputs, double learnRate)
    {
        Inputs = inputs;
        LearnRate = learnRate;
    }

    public void RandomWeights()
    {
        System.Random random =new  System.Random(DateTime.Now.Millisecond);
        Weights = new double[Inputs.Length + 1  ];
        for (int i = 0; i < Weights.Length; i++)
            Weights[i] = random.Next(-10000, 10000) / 10000.0;
    }

    public void Forward()
    {
        double sum = Weights[0] * Bias;
        for (int i = 0; i < Inputs.Length; i++)
            sum += Weights[i + 1] * Inputs[i];

        Output = Math.Tanh(sum);
    }

    public void CalculateError()
    {
        Error = DesiredOutput - Output;
    }

    public void CalculateBackPropagatedError() 
    {
        BackPropagatedError = (1.0 - Output * Output) * Error;
    }

    public void WeightsAdjustment()
    {
        Weights[0] += LearnRate * Bias * BackPropagatedError;
        for (int i = 0; i < Inputs.Length; i++)
            Weights[i + 1] += LearnRate * Inputs[i] * BackPropagatedError;
    }

    public void Backward(double[] inputs, double desiredOutput)
    {
        Inputs = inputs;
        DesiredOutput = desiredOutput;
        Forward();
        CalculateError();
        CalculateBackPropagatedError();
        WeightsAdjustment();
    }
}


