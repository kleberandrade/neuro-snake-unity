using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float m_Distance = 7.0f;
    public LayerMask m_Layer;
    public Color m_Color = Color.red;
    private RaycastHit m_Hit;

    private void FixedUpdate()
    {
        GetDistance();
    }

    public float GetDistance()
    {
        if (Physics.Raycast(transform.position, transform.forward, out m_Hit, m_Distance, m_Layer))
        {
            Debug.DrawRay(transform.position, transform.forward * m_Hit.distance, m_Color);
            return Mathf.Clamp(Vector3.Distance(transform.position, m_Hit.point), 0.0f, m_Distance) / m_Distance;
        }

        Debug.DrawRay(transform.position, transform.forward * m_Distance, m_Color);
        return 1.0f;
    }

    public float GetInverseDistance()
    {
        return 1.0f - GetDistance();
    }
}
