using UnityEngine;

using UnityEngine;

public class IAEnemigo : MonoBehaviour
{
    [Header("Configuracion")]
    public float velocidad = 2f;
    public float distanciaDeteccion = 5f;
    public Sprite spriteMuerto;

    [Header("Puntos de Patrulla")]
    public Transform[] puntosPatrulla; 

    private Transform jugador;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int indicePuntoActual = 0;
    private bool estaMuerto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject playerObj = GameObject.Find("Player");
        if (playerObj != null) jugador = playerObj.transform;
    }

    void Update()
    {
        if (estaMuerto || jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);
        Vector2 direccion;

        if (distancia < distanciaDeteccion) {
            direccion = (jugador.position - transform.position).normalized;
            Perseguir(direccion);
        } else {
            Transform punto = puntosPatrulla[indicePuntoActual];
            direccion = (punto.position - transform.position).normalized;
            Patrullar(punto, direccion);
        }

        // --- NUEVO: ROTACIÓN PARA MIRAR AL OBJETIVO ---
       // Cambia la línea de rotación por esta para que mire de frente correctamente:
     if (direccion != Vector2.zero) {
     float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
     rb.rotation = angulo - 270f; // Probamos restando 90 grados
     }
    }

    void Patrullar(Transform punto, Vector2 dir)
    {
        rb.linearVelocity = dir * velocidad;
        if (Vector2.Distance(transform.position, punto.position) < 0.2f) {
            indicePuntoActual = (indicePuntoActual + 1) % puntosPatrulla.Length;
        }
    }

    void Perseguir(Vector2 dir)
    {
        rb.linearVelocity = dir * velocidad;
    }

    // Detectar cuando toca al jugador
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (estaMuerto) return;
        
        MovimientoTopDown jugador = collision.GetComponent<MovimientoTopDown>();
        if (jugador != null)
        {
            jugador.RecibirDaño(1);
            Debug.Log("¡Un zombi ha mordido al jugador!");
        }
    }

    public void Morir()
  {

if (estaMuerto) return; // Evita que se cuente dos veces si le pegas rápido

    // --- NUEVO: AVISAR AL GAME MANAGER ---
    if (GameManager.instancia != null) {
        GameManager.instancia.SumarKill();
    }

    estaMuerto = true;
    rb.linearVelocity = Vector2.zero; 
    rb.bodyType = RigidbodyType2D.Kinematic;
    rb.freezeRotation = true; 
    spriteRenderer.sprite = spriteMuerto;
    GetComponent<Collider2D>().enabled = false; 
    this.enabled = false;

    estaMuerto = true;
    
    // 1. Detenemos cualquier movimiento actual
    rb.linearVelocity = Vector2.zero; 
    
    // 2. ¡ESTA ES LA CLAVE!: Cambiamos el cuerpo a "Kinematic"
    // Esto hace que la física deje de afectarle por completo
    rb.bodyType = RigidbodyType2D.Kinematic;
    
    // 3. Lo visual y colisiones
    rb.freezeRotation = true; 
    spriteRenderer.sprite = spriteMuerto;
    GetComponent<Collider2D>().enabled = false; 
    
    this.enabled = false; // Apagamos el script
  }
}