using UnityEngine;

public class MovimientoTopDown : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 movimiento;

    void Start()
    {
        // Esto busca el componente Rigidbody2D que le pusimos al Player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura el movimiento (W,A,S,D o Flechas)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movimiento = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Mueve el cuerpo físico
        rb.MovePosition(rb.position + movimiento * velocidad * Time.fixedDeltaTime);
    }
}
