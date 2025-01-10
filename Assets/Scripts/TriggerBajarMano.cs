using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerBajarMano : MonoBehaviour
{
    public CinemachineVirtualCamera camaraJugador; // Cámara principal que sigue al jugador
    public CinemachineVirtualCamera camaraFija; // Cámara fija para la animación
    public PlayableDirector directorBajarMano; // Timeline de la animación de bajar la mano
    public PlayerController playerController; // Controlador del jugador

    private bool eventoActivado = false; // Evita que el trigger se active más de una vez
    private Animator playerAnimator; // Animator del jugador

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

        // 1. Bloquear el control del jugador
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

        // 2. Cambiar a la cámara fija
        camaraJugador.Priority = 0; // Bajar la prioridad de la cámara del jugador
        camaraFija.Priority = 10; // Subir la prioridad de la cámara fija

        // 3. Esperar 2 segundos antes de iniciar la animación
        yield return new WaitForSeconds(2f);

        // 4. Iniciar la animación del Timeline
        if (directorBajarMano != null)
        {
            directorBajarMano.Play();
            Debug.Log("Iniciando animación de bajar la mano...");
        }

        // 5. Esperar 5 segundos (duración de la animación)
        yield return new WaitForSeconds(5f);

        // 6. Restaurar la cámara del jugador
        camaraFija.Priority = 0; // Bajar la prioridad de la cámara fija
        camaraJugador.Priority = 10; // Subir la prioridad de la cámara del jugador

        // 7. Restaurar el control del jugador
        if (playerController != null)
        {
            playerController.enabled = true;
            Debug.Log("Control del jugador restaurado.");
        }
    }
}
