using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerMano : MonoBehaviour
{
    public PlayableDirector animacionMano; // Controlador de la animación
    public PlayerController playerController; // Controlador del jugador

    private bool eventoActivado = false; // Control para evitar múltiples activaciones

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !eventoActivado)
        {
            eventoActivado = true;
            StartCoroutine(ActivarEvento());
        }
    }

    private IEnumerator ActivarEvento()
    {
        // Deshabilitar control del jugador
        playerController.enabled = false;

        // Obtener y ajustar Rigidbody2D
        Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Detener cualquier movimiento
            rb.isKinematic = true; // Evitar que las físicas lo afecten durante la animación
        }

        // Ajustar manualmente la posición del jugador si es necesario
        playerController.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            playerController.transform.position.z
        );

        // Iniciar la animación
        animacionMano.Play();
        Debug.Log("Iniciando animación de la mano...");

        // Esperar a que la animación termine
        yield return new WaitUntil(() => animacionMano.state == PlayState.Paused);

        // Restablecer el estado del Rigidbody2D
        if (rb != null)
        {
            rb.isKinematic = false; // Reactivar físicas
        }

        // Asegurarse de que el jugador recupere el control correctamente
        yield return new WaitForEndOfFrame(); // Esperar al final del frame para evitar conflictos
        playerController.enabled = true; // Habilitar el controlador del jugador

        // Resetear el estado del evento
        eventoActivado = false;

        Debug.Log("Control del jugador restaurado.");
    }
}
