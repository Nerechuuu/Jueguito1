using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerMano : MonoBehaviour
{
    public PlayableDirector animacionMano; // Animación que mueve la mano y la devuelve
    private PlayerController playerController; // Referencia al controlador del jugador

    private bool eventoActivado = false; // Evita que el trigger se active más de una vez

    private void Start()
    {
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
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
            playerController.enabled = true;
            Debug.Log("Control del jugador restaurado.");
        }

        // 5. Reiniciar la variable para permitir que el trigger se active de nuevo
        eventoActivado = false;
    }
}
