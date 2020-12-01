using UnityEngine;

public class Body : MonoBehaviour
{
    public Body m_Body;

    public void ChangePosition(Vector3 position)
    {
        if (m_Body)
            m_Body.ChangePosition(transform.position);

        transform.position = position;
    }
}


