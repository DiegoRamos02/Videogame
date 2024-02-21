using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // P U B L I C
    public static GameManager Instance { get; private set; }
    public int PuntosTotales {  get;  set; }
    public int EnemigosEliminados { get; set; }  // Contador específico para enemigos
    public HUD hud;

    // P R I V A T E

    private int vidas = 3;

    ///000000000vol
    void Start()
    {
        ReiniciarPreferencias();
    }

    private void ReiniciarPreferencias()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    ///000000000

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            PuntosTotales = PlayerPrefs.GetInt("Puntaje", 0);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SumarPuntos(int puntosASumar)
    {
        PuntosTotales += puntosASumar;

        // Intentamos encontrar el HUD si aún no se ha asignado
        if (hud == null)
        {
            hud = GameObject.FindWithTag("HUD")?.GetComponent<HUD>();
        }

        if (hud != null)
        {
            hud.ActualizarPuntos(PuntosTotales);
            hud.ActualizarEnemigos(EnemigosEliminados);
        }
        else
        {
            Debug.LogError("Error: hud es null en SumarPuntos");
        }
    }

    public void Perdervidas()
    {
        vidas -= 1;
        if (vidas == 0)
        {
            // Guardar puntaje antes de reiniciar el nivel
            PlayerPrefs.SetInt("Puntaje", PuntosTotales);
            PlayerPrefs.Save();

            // Reiniciamos el nivel
            ReiniciarNivel();
            SceneManager.LoadScene(0);
        }
        else
        {
            int indiceVidaADesactivar = vidas - 1; // Ajustar el índice al valor correcto
            if (indiceVidaADesactivar >= 0 && hud.vidas != null && indiceVidaADesactivar < hud.vidas.Length)
            {
                hud.DesactivarVida(indiceVidaADesactivar);
            }
            else
            {
                Debug.LogError("Error: Índice de vida fuera de los límites o el array 'vidas' es nulo.");
            }
        }

        // Llamamos al método para reiniciar los enemigos eliminados cuando se queda sin vidas
        //ReiniciarEnemigosEliminados();
    }

    public void IncrementarEnemigosEliminados()
    {
        EnemigosEliminados++;
        hud.ActualizarEnemigos(EnemigosEliminados);
    }
    
    // Método para reiniciar el contador específico para enemigos
    public void ReiniciarEnemigosEliminados()
    {
        EnemigosEliminados = 0;
        hud.ActualizarEnemigos(EnemigosEliminados);
    }
    
    public bool RecuperarVidas()
    {
        if(vidas == 3)
        {
            return false;
        }
        hud.ActivarVida(vidas);
        vidas += 1;
        return true;
    }

    // Método para reiniciar el nivel
   private void ReiniciarNivel()
    {

        // Restablecer las vidas a su valor inicial
        vidas = 3;

        // Restablecer los puntos a cero antes de reiniciar el nivel
        PuntosTotales = 0;
        EnemigosEliminados = 0;

        PlayerPrefs.SetInt("Puntaje", PuntosTotales);
        PlayerPrefs.Save();

        // Debug para verificar que se está llamando a ReiniciarNivel
        Debug.Log("Reiniciando nivel. PuntosTotales: " + PuntosTotales);

        SceneManager.LoadScene(1);
    }

    public int ObtenerEnemigosEliminados()
    {
        return EnemigosEliminados;
    }

    public int ObtenerVidas()
    {
        return vidas;
    }
}
