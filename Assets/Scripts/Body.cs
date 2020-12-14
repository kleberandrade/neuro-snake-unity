using System.Collections;
using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject m_BodyPrefab;
    public Body m_Body;
    private bool m_FoodEaten;

    private void OnEnable()
    {
        Food.OnFoodEaten += OnFoodEaten;
    }

    private void OnDisable()
    {
        Food.OnFoodEaten -= OnFoodEaten;
    }

    public void OnFoodEaten()
    {
        m_FoodEaten = true;
    }

    public void ChangePosition(Vector3 position)
    {
        if (m_Body)
        {
            m_Body.ChangePosition(transform.position);
        }
        else if (m_FoodEaten)
        {
            var go = Instantiate(m_BodyPrefab, transform.position, Quaternion.identity);
            m_Body = go.GetComponent<Body>();
        }

        transform.position = position;
    }

    public void Destroy()
    {
        StartCoroutine(Destroying());
    }

    private IEnumerator Destroying()
    {
        yield return new WaitForEndOfFrame();
        if (m_Body)
            m_Body.Destroy();
        Destroy(gameObject);
    }
}


