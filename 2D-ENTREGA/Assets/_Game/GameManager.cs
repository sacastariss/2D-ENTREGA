using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;
    public TextMeshProUGUI textoContador;
    private int kills = 0;
    
    private int jeringas = 0;
    private TextMeshProUGUI textoJeringas;
    
    private bool juegoTerminado = false;
    private GameObject canvasGameOver;
    private TextMeshProUGUI textoSalud;

    void Awake()
    {
        if (instancia == null) instancia = this;
    }

    void Start()
    {
        // Crear el Canvas de Game Over dinámicamente
        CrearCanvasGameOver();
        // Crear la barra de salud
        CrearBarraSalud();
        // Crear el contador de jeringas
        CrearContadorJeringas();
        // Mostrar salud inicial
        ActualizarVisualizacionSalud(3);
    }

    void Update()
    {
        // Si el juego terminó, presionar R para reiniciar
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            Reiniciar();
        }
    }

    public void SumarKill()
    {
        if (juegoTerminado) return; // No contar kills después de morir
        
        kills++;
        textoContador.text = "Zombies: " + kills;
        Debug.Log("Zombies eliminados: " + kills);
    }

    public void SumarJeringa()
    {
        if (juegoTerminado) return;
        
        jeringas++;
        if (textoJeringas != null)
        {
            textoJeringas.text = "Jeringas: " + jeringas;
        }
        Debug.Log("Jeringas recolectadas: " + jeringas);
    }

    public int ObtenerJeringas()
    {
        return jeringas;
    }

    public void ActualizarVisualizacionSalud(int saludActual)
    {
        if (textoSalud == null) return;
        
        string barrita = "";
        for (int i = 0; i < saludActual; i++)
        {
            barrita += "♥ ";
        }
        
        textoSalud.text = "Salud: " + barrita;
    }

    public void MostrarGameOver()
    {
        juegoTerminado = true;
        Time.timeScale = 0; // Pausar el juego
        
        if (canvasGameOver != null)
        {
            canvasGameOver.SetActive(true);
        }
        
        Debug.Log("¡GAME OVER! Presiona R para reiniciar");
    }

    void CrearBarraSalud()
    {
        // Buscar Canvas existente en la escena
        Canvas canvasExistente = FindObjectOfType<Canvas>();
        
        if (canvasExistente == null)
        {
            Debug.LogError("No hay Canvas en la escena");
            return;
        }
        
        // Crear texto de salud dentro del Canvas existente
        GameObject textoObj = new GameObject("TextoSalud");
        textoObj.transform.SetParent(canvasExistente.transform, false);
        textoSalud = textoObj.AddComponent<TextMeshProUGUI>();
        textoSalud.text = "Salud: ♥ ♥ ♥";
        textoSalud.fontSize = 32;
        textoSalud.alignment = TextAlignmentOptions.TopLeft;
        textoSalud.color = Color.white;
        
        RectTransform textoRect = textoObj.GetComponent<RectTransform>();
        
        // Desactivar Layout Elements que pudieran interferir
        LayoutElement layoutElement = textoObj.GetComponent<LayoutElement>();
        if (layoutElement != null) layoutElement.enabled = false;
        
        textoRect.anchoredPosition = new Vector2(400, -180);
        textoRect.sizeDelta = new Vector2(300, 80);
    }

    void CrearContadorJeringas()
    {
        // Buscar Canvas existente en la escena
        Canvas canvasExistente = FindObjectOfType<Canvas>();
        
        if (canvasExistente == null)
        {
            Debug.LogError("No hay Canvas en la escena");
            return;
        }
        
        // Crear texto de jeringas dentro del Canvas existente
        GameObject textoObj = new GameObject("TextoJeringas");
        textoObj.transform.SetParent(canvasExistente.transform, false);
        textoJeringas = textoObj.AddComponent<TextMeshProUGUI>();
        textoJeringas.text = "Jeringas: 0";
        textoJeringas.fontSize = 32;
        textoJeringas.alignment = TextAlignmentOptions.TopRight;
        textoJeringas.color = Color.white;
        
        RectTransform textoRect = textoObj.GetComponent<RectTransform>();
        
        // Desactivar Layout Elements que pudieran interferir
        LayoutElement layoutElement = textoObj.GetComponent<LayoutElement>();
        if (layoutElement != null) layoutElement.enabled = false;
        
        textoRect.anchoredPosition = new Vector2(490, -350);
        textoRect.sizeDelta = new Vector2(300, 80);
    }

    void CrearCanvasGameOver()
    {
        // Crear Canvas
        GameObject canvasObj = new GameObject("CanvasGameOver");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Crear Panel de fondo
        GameObject panelObj = new GameObject("Panel");
        panelObj.transform.SetParent(canvasObj.transform, false);
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        
        // Crear texto "GAME OVER"
        GameObject textoGameOverObj = new GameObject("TextoGameOver");
        textoGameOverObj.transform.SetParent(panelObj.transform, false);
        TextMeshProUGUI textoGameOver = textoGameOverObj.AddComponent<TextMeshProUGUI>();
        textoGameOver.text = "GAME OVER\n\nPresiona R para reiniciar";
        textoGameOver.fontSize = 80;
        textoGameOver.alignment = TextAlignmentOptions.Center;
        textoGameOver.color = Color.red;
        
        RectTransform textoRect = textoGameOverObj.GetComponent<RectTransform>();
        textoRect.sizeDelta = new Vector2(800, 300);
        textoRect.anchoredPosition = Vector2.zero;
        
        canvasGameOver = canvasObj;
        canvasGameOver.SetActive(false);
    }

    void Reiniciar()
    {
        Time.timeScale = 1; // Reanudar tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}