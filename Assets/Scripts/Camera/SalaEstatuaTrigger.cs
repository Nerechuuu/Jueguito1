using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class SalaEstatuaTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera camaraJugador;
    public CinemachineVirtualCamera camaraEstatua;
    public PlayerController playerController;
    public PlayableDirector directorCinematica;

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

        // Bloquear control del jugador
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

        // Cambiar a cámara de la estatua
        camaraJugador.Priority = 0;
        camaraEstatua.Priority = 10;

        // Esperar 1 segundo antes de empezar la cinemática
        yield return new WaitForSeconds(2.5f);
        
        // Reproducir la cinemática
        if (directorCinematica != null)
        {
            directorCinematica.Play();

            // Esperar hasta que termine la cinemática
            float tiempoEspera = 0f;
            float tiempoMaximoEspera = 4f; // Evitar que se quede atrapado en el ciclo
            while (directorCinematica.state == PlayState.Playing && tiempoEspera < tiempoMaximoEspera)
            {
                tiempoEspera += Time.deltaTime;
                yield return null;
            }
        }

        // Restaurar cámara y control del jugador
        camaraEstatua.Priority = 0;
        camaraJugador.Priority = 10;

        if (playerController != null)
        {
            playerController.enabled = true;
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsWalking", false);
            }
        }
    }
}
