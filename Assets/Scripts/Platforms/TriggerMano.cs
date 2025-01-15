using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerMano : MonoBehaviour
{
    public PlayableDirector animacionMano;
    public PlayerController playerController;

    private bool eventoActivado = false;

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
        playerController.enabled = false;
        Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }

        animacionMano.Play();
        Debug.Log("Iniciando animación de la mano...");

        yield return new WaitUntil(() => animacionMano.state == PlayState.Paused);

        playerController.enabled = true;

        eventoActivado = false;
    }
}
