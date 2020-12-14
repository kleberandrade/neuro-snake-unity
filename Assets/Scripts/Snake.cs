using UnityEngine;

public class Snake : MonoBehaviour
{
    public delegate void OnMoveHandler();
    public static event OnMoveHandler OnMove;
    public static event OnMoveHandler OnDie;

    public float Left { get; set; }
    public float Right { get; set; }
    public float TimeRate { get; set; }

    public Body m_Body;

    private float m_ElapsedTime;
    public float m_MaxTimeRate = 0.5f;
    public float m_MinTimeRate = 0.05f;
    public float m_MaxFoodAmount = 70.0f;
    private int m_FoodAmount = 0;

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
        m_FoodAmount++;
    }

    private void Update()
    {
        TimeRate = Mathf.Lerp(m_MaxTimeRate, m_MinTimeRate, m_FoodAmount / m_MaxFoodAmount);

        m_ElapsedTime += Time.deltaTime;
        if (m_ElapsedTime >= TimeRate)
        {
            if (Left > 0)
                transform.Rotate(Vector3.up, -90.0f);

            if (Right > 0)
                transform.Rotate(Vector3.up, 90.0f);

            if (OnMove != null) OnMove(); 

            m_Body.ChangePosition(transform.position);
            transform.Translate(Vector3.forward);

            Reset();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
            return;

        if (OnDie != null) OnDie();
        m_Body.Destroy();
        Destroy(gameObject);
    }

    private void Reset()
    {
        m_ElapsedTime = 0;
        Left = 0;
        Right = 0;
    }
}
