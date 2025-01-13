using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerMano : MonoBehaviour
{
    public PlayableDirector animacionMano; // Animación que mueve la mano y la devuelve
    public PlayerController playerController; // Referencia al controlador del jugador
    public Rigidbody2D playerRigidbody; // Referencia al Rigidbody del jugador (si no está en PlayerController)
    private Animator playerAnimator; // Animator del jugador

    private bool eventoActivado = false; // Evita que el trigger se active más de una vez

    private void Start()
    {
        if (playerController != null)
        {
            playerAnimator = playerController.GetComponent<Animator>();
            playerRigidbody = playerController.GetComponent<Rigidbody2D>();
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

        // 1. Bloquear completamente el control del jugador
        if (playerController != null)
        {
            playerController.enabled = false; // Desactiva el script del controlador
            if (playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero; // Detén el movimiento
                playerRigidbody.isKinematic = true; // Evita que la física afecte al jugador durante la animación
            }

            if (playerAnimator != null)
            {
                // Apaga cualquier animación o estado actual
                playerAnimator.SetBool("IsWalking", false);
                playerAnimator.SetBool("IsJumping", false);
                playerAnimator.SetBool("IsFalling", false);
                playerAnimator.SetBool("IsGliding", false);
            }
        }

        // 2. Iniciar la animación del Timeline
        if (animacionMano != null)
        {
            animacionMano.Play();
            Debug.Log("Iniciando animación de la mano...");
        }

        // 3. Esperar a que termine la animación
        yield return new WaitForSeconds((float)animacionMano.duration);

        // 4. Restaurar el control del jugador
        if (playerController != null)
        {
            playerController.enabled = true; // Reactiva el controlador
            if (playerRigidbody != null)
            {
                playerRigidbody.isKinematic = false; // Permite que la física vuelva a funcionar
            }
        }

        Debug.Log("Control del jugador restaurado.");
        eventoActivado = false; // Permite que el trigger pueda reutilizarse
    }
}
