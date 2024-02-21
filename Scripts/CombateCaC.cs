using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateCaC : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;

    [SerializeField] private float radioGolpe;

    [SerializeField] private float danoGolpe;

    private Animator animator;

    private AudioSource audioSource;

    public AudioClip sonidoEspada;

    private void Start()
    {
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Golpe();
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("Ataque");

        // Reproducir el sonido del ataque
        if (sonidoEspada != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoEspada);
        }

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                colisionador.transform.GetComponent<Enemigo>().TomarDano(danoGolpe);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
