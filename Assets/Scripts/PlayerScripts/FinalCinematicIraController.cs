using Cinemachine;
using UnityEngine;
using System.Collections;

public class FinalCinematicIraController : MonoBehaviour
{
    [Header("Sistema de Partículas")]
    [SerializeField] private ParticleSystem particulasFinales;

    [Header("Player")] 
    [SerializeField] private PlayerController player;

    [Header("Shake de Cámara")]
    [SerializeField] private float intensidadShake = 1.0f;
    [SerializeField] private float duracionShake = 0.5f;

    [Header("Cámaras")]
    [SerializeField] private CinemachineVirtualCamera camaraPrincipal; 
    [SerializeField] private CinemachineVirtualCamera camaraCinematica; 

    [Header("GameObject a activar")]
    [SerializeField] private GameObject AnimacionAActivar;
    [SerializeField] private GameObject AnimacionAActivar2;
    [SerializeField] private GameObject BotonRestablecerAActivar;
    [SerializeField] private GameObject ImagenAnimacionAActivar;
    [SerializeField] private GameObject ImagenAnimacionAActivar2;


    private bool cinematicaEnProgreso = false;

    public void IniciarCinematica()
    {
        if (cinematicaEnProgreso) return;
        StartCoroutine(CinematicaFinal());
    }

    private IEnumerator CinematicaFinal()
    {
        cinematicaEnProgreso = true;

        AnimacionAActivar.SetActive(false);
        BotonRestablecerAActivar.SetActive(false);
        AnimacionAActivar2.SetActive(false);
        ImagenAnimacionAActivar.SetActive(false);
        ImagenAnimacionAActivar2.SetActive(false);

        player.enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsGliding", false);

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(HacerShake());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(HacerShake());

        camaraPrincipal.Priority = 0; 
        camaraCinematica.Priority = 10;

        yield return new WaitForSeconds(2f);
        ImagenAnimacionAActivar2.SetActive(true);
        AnimacionAActivar.SetActive(true);
        BotonRestablecerAActivar.SetActive(true);

        yield return new WaitForSeconds(2f);
        AnimacionAActivar.SetActive(false);

        yield return new WaitForSeconds(1f);
        AnimacionAActivar2.SetActive(true);

        yield return new WaitForSeconds(3f);
        ImagenAnimacionAActivar.SetActive(true);
        AnimacionAActivar2.SetActive(false);

        camaraPrincipal.Priority = 10;
        camaraCinematica.Priority = 0;
        
        yield return new WaitForSeconds(2f);

        particulasFinales.Play();

        player.CrecerANivel2Si();

        player.enabled = true;

        cinematicaEnProgreso = false;
    }

    private IEnumerator HacerShake()
    {

    Transform objetoSeguidoOriginal = camaraPrincipal.Follow;


    camaraPrincipal.Follow = null;


    Vector3 posicionInicial = camaraPrincipal.transform.localPosition;

    float tiempo = 0;
    while (tiempo < duracionShake)
    {
        float offsetX = Random.Range(-1f, 1f) * intensidadShake;
        float offsetY = Random.Range(-1f, 1f) * intensidadShake;

        camaraPrincipal.transform.localPosition = new Vector3(posicionInicial.x + offsetX, posicionInicial.y + offsetY, posicionInicial.z);

        tiempo += Time.deltaTime;
        yield return null;
    }

    camaraPrincipal.transform.localPosition = posicionInicial;

    camaraPrincipal.Follow = objetoSeguidoOriginal;
    }

}
