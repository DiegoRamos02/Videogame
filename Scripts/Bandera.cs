using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bandera : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Obtén la instancia del GameManager
            GameManager gameManager = GameManager.Instance;

            // Verifica si se han eliminado los tres enemigos
            if (gameManager != null && gameManager.ObtenerEnemigosEliminados() >= 3)
            {
                
                Debug.Log("¡Puedes pasar al segundo nivel!");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                //  Mostrar mensaje si no se cumplen las condiciones
                Debug.Log("¡Aún no has eliminado a los tres enemigos!");
            }
        }
    }
}
