using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

public class TriggerMano : MonoBehaviour
{
    public PlayableDirector animacionMoverMano; // Animación que mueve la mano al otro lado
    public PlayableDirector animacionVolverMano; // Animación que devuelve la mano a su posición inicial
    public TriggerMano triggerBajarMano; // Referencia al script del primer trigger para resetearlo

    private bool jugadorSobreMano = false; // Para saber si el jugador está sobre la mano

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !jugadorSobreMano)
        {
            jugadorSobreMano = true;
            StartCoroutine(MoverMano());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && jugadorSobreMano)
        {
            jugadorSobreMano = false;
            StartCoroutine(VolverMano());
        }
    }

    private IEnumerator MoverMano()
    {
        // Espera 2 segundos antes de iniciar la animación
        yield return new WaitForSeconds(2f);

        if (animacionMoverMano != null)
        {
            animacionMoverMano.Play();
            Debug.Log("Animación de mover mano iniciada.");
        }

        yield return new WaitForSeconds(9f);

        if (animacionMoverMano != null)
        {
            animacionMoverMano.Stop();
            Debug.Log("Animación de mover mano detenida.");
        }
    }

    private IEnumerator VolverMano()
    {
        if (animacionVolverMano != null)
        {
            animacionVolverMano.Play();
            Debug.Log("Animación de volver mano iniciada.");
        }

        // Espera 5 segundos para que la animación termine
        yield return new WaitForSeconds(12f);

        if (animacionVolverMano != null)
        {
            animacionVolverMano.Stop();
            Debug.Log("Animación de volver mano detenida.");
        }
    }
}
