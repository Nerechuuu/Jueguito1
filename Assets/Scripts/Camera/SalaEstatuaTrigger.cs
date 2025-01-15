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

    private float intensidadShake = 0.8f;
    private float duracionShake = 0.8f;

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

        playerController.enabled = false;
        playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsWalking", false);
            playerAnimator.SetBool("IsJumping", false);
            playerAnimator.SetBool("IsFalling", false);
            playerAnimator.SetBool("IsGliding", false);
        }

        camaraJugador.Priority = 0;
        camaraEstatua.Priority = 10;

        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(HacerShake());
        
        directorCinematica.Play();

        float tiempoEspera = 0f;
        float tiempoMaximoEspera = 4f; 

        while (directorCinematica.state == PlayState.Playing && tiempoEspera < tiempoMaximoEspera)
        {
            tiempoEspera += Time.deltaTime;
           yield return null;
        }

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

     private IEnumerator HacerShake()
    {
        Vector3 posicionInicial = camaraEstatua.transform.localPosition;

        float tiempo = 0;
        while (tiempo < duracionShake)
        {

            float offsetX = Random.Range(-1f, 1f) * intensidadShake;
            float offsetY = Random.Range(-1f, 1f) * intensidadShake;

            camaraEstatua.transform.localPosition = new Vector3(posicionInicial.x + offsetX, posicionInicial.y + offsetY, posicionInicial.z);

            tiempo += Time.deltaTime;
            yield return null;
        }

        camaraEstatua.transform.localPosition = posicionInicial;
    }

}
