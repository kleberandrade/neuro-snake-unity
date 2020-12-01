using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMLP : MonoBehaviour
{


    private void Start()
    {
        Training();
    }

    private void Training()
    {
        MultiLayerPreceptronNetwork net = new MultiLayerPreceptronNetwork(2, 2, 1, 0.1);
        Debug.Log("Training network...");
        for (int i = 0; i < 200; i++)
        {
            net.Backward(new double[] { -1, -1 }, new double[] { -1 });
            Debug.Log($"{i}: -1, -1 = {net.GetOutputs()[0]}");
            net.Backward(new double[] { -1,  1 }, new double[] {  1 });
            Debug.Log($"{i}: -1,  1 = {net.GetOutputs()[0]}");
            net.Backward(new double[] {  1, -1 }, new double[] {  1 });
            Debug.Log($"{i}:  1, -1 = {net.GetOutputs()[0]}");
            net.Backward(new double[] {  1,  1 }, new double[] { -1 });
            Debug.Log($"{i}:  1,  1 = {net.GetOutputs()[0]}");
            Debug.Log("");
        }
    }

}
