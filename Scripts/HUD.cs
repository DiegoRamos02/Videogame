using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{

    // P U B L I C
    public TextMeshProUGUI puntos;
    public TextMeshProUGUI enemigosEliminadosText;  // Texto para mostrar enemigos eliminados
    public GameObject[] vidas;
    public GameObject contenedorMoneda;

    private GameManager gameManager;

    private void Awake()
    {
        // Encuentra el GameManager por etiqueta (asegúrate de etiquetar correctamente el objeto GameManager)
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        puntos.text = GameManager.Instance.PuntosTotales.ToString();

        enemigosEliminadosText.text = GameManager.Instance.ObtenerEnemigosEliminados().ToString();

        // Actualiza la visualización de las vidas
        ActualizarVidas();
    }

    void ActualizarVidas()
    {
        int vidasRestantes = GameManager.Instance.ObtenerVidas();

        // Desactiva todos los corazones
        for (int i = 0; i < vidas.Length; i++)
        {
            vidas[i].SetActive(false);
        }

        // Activa solo la cantidad de corazones correspondientes a las vidas restantes
        for (int i = 0; i < vidasRestantes; i++)
        {
            vidas[i].SetActive(true);
        }
    }

    public void ActualizarPuntos(int puntosTotales)
    {
        puntos.text = puntosTotales.ToString();
    }

    public void ActualizarEnemigos(int puntosEnemigos)
    {
        enemigosEliminadosText.text = puntosEnemigos.ToString();
    }

    public void DesactivarVida(int indice)
    {
        if (indice >= 0 && indice < vidas.Length && vidas[indice] != null)
        {
            vidas[indice].SetActive(false);
        }
        else
        {
            Debug.LogError("Error: Índice de vida fuera de los límites o el objeto ya está desactivado.");
        }
    }

    public void ActivarVida(int indice)
    {
        vidas[indice].SetActive(true);
    }
}
