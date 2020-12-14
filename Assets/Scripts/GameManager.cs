using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_Food;
    private List<float> m_SlotX = new List<float>();
    private List<float> m_SlotZ = new List<float>();

    private void Start()
    {
        var x = -17.5f;
        while (x < 18) m_SlotX.Add(x++);
        var z = -9.5f;    
        while (z < 10) m_SlotZ.Add(z++);
        OnSpawnFood();
    }

    private void OnEnable()
    {
        Food.OnFoodEaten += OnSpawnFood;
    }

    private void OnDisable()
    {
        Food.OnFoodEaten -= OnSpawnFood;
    }


    public void OnSpawnFood()
    {
        var x = m_SlotX[Random.Range(0, m_SlotX.Count)];
        var z = m_SlotZ[Random.Range(0, m_SlotZ.Count)];
        var position = new Vector3(x, 0, z);
        Instantiate(m_Food, position, Quaternion.identity);
    }
}
