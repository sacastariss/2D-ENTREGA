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

    public void Morir()
    {
        estaMuerto = true;
        rb.linearVelocity = Vector2.zero;
        rb.freezeRotation = true; // Deja de rotar al morir
        spriteRenderer.sprite = spriteMuerto;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}