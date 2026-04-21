using UnityEngine;
using TMPro; // Importante para controlar el texto

public class GameManager : MonoBehaviour
{
    public static GameManager instancia; // Permite que otros scripts lo encuentren fácil
    public TextMeshProUGUI textoContador;
    private int kills = 0;

    void Awake()
    {
        // Esto hace que solo exista un GameManager en el juego
        if (instancia == null) instancia = this;
    }

    public void SumarKill()
    {
        kills++;
        textoContador.text = "Zombies: " + kills;
        Debug.Log("Zombies eliminados: " + kills);
    }
}