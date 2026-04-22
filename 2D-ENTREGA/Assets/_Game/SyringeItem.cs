using UnityEngine;

public class SyringeItem : CollectibleItem
{
    protected override void Recolectar()
    {
        Debug.Log("¡Jeringa recolectada!");
        
        // Notificar al GameManager
        if (GameManager.instancia != null)
        {
            GameManager.instancia.SumarJeringa();
        }
        
        base.Recolectar();
    }
}
