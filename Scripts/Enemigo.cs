using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // P U B L I C 
    public float cooldownAtaque;
    public float velocidad;
    public Transform controladorSuelo;
    public float distancia;
    public bool moviendoDerecha;

    // P R I V A T E 
    private bool puedeAtacar = true;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private float vida;
    private bool estaVivo = true;
    private static int enemigosEliminados = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();  
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!estaVivo)
        {
            // Si el enemigo esta muerto, no actualizamos la posicion
            return;
        }

        RaycastHit2D informacionSuelo = Physics2D.Raycast(controladorSuelo.position, Vector2.down, distancia);

        animator.SetBool("IsRunning", true);
        
        rb.velocity = new Vector2(velocidad, rb.velocity.y); ;

        if(informacionSuelo == false)
        {
            // Girar
            Girar();
        }
    }

    private void Girar()
    {
        moviendoDerecha = !moviendoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        velocidad *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorSuelo.transform.position, controladorSuelo.transform.position + Vector3.down * distancia);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Si no puede atacar salimos de la funcion
            if (!puedeAtacar) return;

            // Desactivamos el ataque
            puedeAtacar = false;

            // Cambiamos la opacidad del sprite
            Color color = spriteRenderer.color;
            color.a =0.5f;
            spriteRenderer.color = color;   

            // Perdemos una vida
            GameManager.Instance.Perdervidas();

            // Aplicamos el golpe al personaje
            other.gameObject.GetComponent<MovimientoPersonaje>().AplicarGolpe();

            Invoke("ReactivarAtaque", cooldownAtaque);
        }
    }

    void ReactivarAtaque()
    {
        puedeAtacar = true;

        // Cambiamos la opacidad del sprite.
        Color c = spriteRenderer.color;
        c.a = 1f;
        spriteRenderer.color = c;
    }

    public void TomarDano(float dano) 
    {
        vida -= dano;

        if(vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        // Incrementar el contador de enemigos eliminados
        GameManager.Instance.IncrementarEnemigosEliminados();

        estaVivo = false; // Cambiamos el estado del enemigo a muerto cuando se detecte que le baje la vida
        animator.SetTrigger("Muerte");
        rb.velocity = Vector2.zero; // Detenemos el movimiento del personaje 

        Invoke("Desaparecer", 3f);
    }

    // Método estático para obtener la cantidad de enemigos eliminados
    public static int ObtenerEnemigosEliminados()
    {
        return enemigosEliminados;
    }

    // Método estático para reiniciar el contador de enemigos eliminados
    public static void ReiniciarEnemigosEliminados()
    {
        enemigosEliminados = 0;
    }


    private void Desaparecer()
    {
        // Puedes desactivar el objeto o destruirlo, segun lo que prefieras
        gameObject.SetActive(false);   // Desactiva el objeto

        // Destroy(gameObject); // Destruye el objeto
    }
}
