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
    
    [Header("Ajustes de Salud")]
    public int salud = 3;
    private bool estaMuerto = false;
    
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
        if (estaMuerto) return; // Si está muerto, no hace nada
        
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

    // Sistema de salud
    public void RecibirDaño(int daño)
    {
        if (estaMuerto) return;
        
        salud -= daño;
        spriteRenderer.color = Color.red;
        Invoke("ResetearColor", 0.1f);
        
        Debug.Log("Personaje recibió daño. Salud: " + salud);
        
        // Actualizar barra de salud en UI
        if (GameManager.instancia != null)
        {
            GameManager.instancia.ActualizarVisualizacionSalud(salud);
        }
        
        if (salud <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        estaMuerto = true;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        spriteRenderer.color = Color.black;
        
        Debug.Log("¡EL PERSONAJE HA MUERTO!");
        
        // Notificar al Game Manager que el juego terminó
        if (GameManager.instancia != null)
        {
            GameManager.instancia.MostrarGameOver();
        }
    }
}