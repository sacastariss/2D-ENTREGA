using UnityEngine;

public class MovimientoTopDown : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 movimiento;

    [Header("Ajustes de Ataque")]
    public float radioAtaque = 1.5f; 
    public KeyCode teclaAtaque = KeyCode.Space; 
    
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorOriginal = spriteRenderer.color;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movimiento = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(teclaAtaque))
        {
            Atacar();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movimiento * velocidad * Time.fixedDeltaTime);
    }

    void Atacar()
    {
        spriteRenderer.color = Color.red;
        Invoke("ResetearColor", 0.15f);

        Collider2D[] objetosTocados = Physics2D.OverlapCircleAll(transform.position, radioAtaque);

        foreach (Collider2D col in objetosTocados)
        {
            IAEnemigo zombie = col.GetComponent<IAEnemigo>();

            if (zombie != null)
            {
                zombie.Morir();
            }
        }
    }

    void ResetearColor()
    {
        spriteRenderer.color = colorOriginal;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radioAtaque);
    }
}