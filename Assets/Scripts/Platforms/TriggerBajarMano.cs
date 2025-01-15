using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerBajarMano : MonoBehaviour
{
    public CinemachineVirtualCamera camaraJugador; 
    public CinemachineVirtualCamera camaraFija; 
    public PlayableDirector directorBajarMano;
    public PlayerController playerController;

    private bool eventoActivado = false;
    private Animator playerAnimator;

    private void Start()
    {
        if (playerController != null)
        {
            playerAnimator = playerController.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !eventoActivado)
        {
            StartCoroutine(ActivarEvento());
        }
    }

    private IEnumerator ActivarEvento()
    {
        eventoActivado = true;

        if (playerController != null)
        {
            playerController.enabled = false;
            playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsWalking", false);
                playerAnimator.SetBool("IsJumping", false);
                playerAnimator.SetBool("IsFalling", false);
                playerAnimator.SetBool("IsGliding", false);
            }
        }

        camaraJugador.Priority = 0; 
        camaraFija.Priority = 10; 

        yield return new WaitForSeconds(2f);

        if (directorBajarMano != null)
        {
            directorBajarMano.Play();
            Debug.Log("Iniciando animación de bajar la mano...");
        }

        yield return new WaitForSeconds(5f);

        camaraFija.Priority = 0; 
        camaraJugador.Priority = 10;

        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("Control del jugador restaurado.");
        }
    }
}
