using UnityEngine;

public class Snake : MonoBehaviour
{
    public float Left { get; set; }
    public float Right { get; set; }
    public float TimeRate { get; set; }

    private float m_ElapsedTime;
    public Body m_Body;

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;
        if (m_ElapsedTime >= TimeRate)
        {
            if (Left > 0)
                transform.Rotate(Vector3.up, -90.0f);

            if (Right > 0)
                transform.Rotate(Vector3.up, 90.0f);

            m_Body.ChangePosition(transform.position);
            transform.Translate(Vector3.forward);

            Reset();
        }
    }

    private void Reset()
    {
        m_ElapsedTime = 0;
        Left = 0;
        Right = 0;
    }
}
