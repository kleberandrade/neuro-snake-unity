﻿using UnityEngine;

public class Food : MonoBehaviour
{
    public delegate void OnFoodEatenHandler();
    public static event OnFoodEatenHandler OnFoodEaten;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Snake"))
        {
            if (OnFoodEaten != null)
                OnFoodEaten();

            Destroy(gameObject);
        }
    }
}
