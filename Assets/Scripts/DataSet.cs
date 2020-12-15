using UnityEngine;

[System.Serializable]
public class DataSet
{
    public double[] inputs;
    public double[] output;

    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is DataSet))
            return false;

        DataSet data = (DataSet)obj;
        for (int i = 0; i < data.inputs.Length; i++)
        {
            if (!IsEqual(inputs[i], data.inputs[i]))
                return false; 
        }

        for (int i = 0; i < data.output.Length; i++)
        {
            if(!IsEqual(output[i], data.output[i]))
                return false;
        }

        return true;
    }

    private bool IsEqual(double a, double b)
    {
        if (a >= b - 0.0001 && a <= b + 0.0001)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}