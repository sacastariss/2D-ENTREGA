using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Configuración")]
    public float rotationSpeed = 180f;
    public float bobSpeed = 2f;
    public float bobHeight = 0.3f;
    
    private Vector3 startPosition;
    private bool recolectado = false;

    void Start()
    {
        startPosition = transform.position;
        
        // Asegurar que tiene un collider como trigger
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogError("CollectibleItem en " + gameObject.name + " necesita un Collider2D");
            return;
        }
        collider.isTrigger = true;
    }

    void Update()
    {
        if (recolectado) return;
        
        // Rotación suave
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
        // Movimiento de arriba/abajo
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (recolectado) return;
        
        MovimientoTopDown jugador = collision.GetComponent<MovimientoTopDown>();
        if (jugador != null)
        {
            Recolectar();
        }
    }

    protected virtual void Recolectar()
    {
        recolectado = true;
        Debug.Log("Item recolectado: " + gameObject.name);
        Destroy(gameObject);
    }
}
