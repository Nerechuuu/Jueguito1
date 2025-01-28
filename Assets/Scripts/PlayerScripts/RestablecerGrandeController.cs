using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;
using System.Collections;

public class RestablecerGrandeController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Sistema de Partículas y AnimationWolfs")]
    [SerializeField] private ParticleSystem dustWolfs;
    [SerializeField] private GameObject ImagenAnimacionADesactivar;
    [SerializeField] private GameObject ImagenAnimacionADesactivar2;
    [SerializeField] private GameObject AnimacionAActivar3;

    [Header("Timeline")]
    [SerializeField] private PlayableDirector playableDirector;

    [Header("Cámaras")]
    [SerializeField] private CinemachineVirtualCamera camaraPrincipal;
    [SerializeField] private CinemachineVirtualCamera camaraCinematica;

    [Header("Shake de Cámara")]
    [SerializeField] private float intensidadShake = 1.0f;
    [SerializeField] private float duracionShake = 0.5f;

    private bool cinematicaEnProgreso = false;

    public void Restablecer()
    {
        if (cinematicaEnProgreso) return;
        StartCoroutine(RestablecerGrande());
    }

    private IEnumerator RestablecerGrande()
    {
        cinematicaEnProgreso = true;

        ImagenAnimacionADesactivar.SetActive(true);
        ImagenAnimacionADesactivar2.SetActive(true);
        AnimacionAActivar3.SetActive(false);

        player.enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsGliding", false);

        camaraPrincipal.Priority = 0;
        camaraCinematica.Priority = 10;

        yield return new WaitForSeconds(1.5f);
        ImagenAnimacionADesactivar.SetActive(false);
        AnimacionAActivar3.SetActive(true);

        yield return new WaitForSeconds(1f);

        AnimacionAActivar3.SetActive(false);

        //AQUÍ IRÁ LA ANIMACIÓN DE LA CHICA CAMINANDO HACIA EL PERRO

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(HacerShake());

        Debug.Log("Intentando reproducir las partículas.");
        dustWolfs.Play();

        yield return new WaitForSeconds(1f);

        ImagenAnimacionADesactivar2.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        camaraPrincipal.Priority = 10;
        camaraCinematica.Priority = 0;

        yield return new WaitForSeconds(1.5f);

        player.RestablecerDesdeCrecido2Si();

        //Y AHORA CAE AL SUELO DE LA TRISTEZA

        yield return new WaitForSeconds(2f);

            playableDirector.Play();

            animator.SetBool("IsGliding", true);

        yield return new WaitForSeconds(5f);

        cinematicaEnProgreso = false;
    }

    private IEnumerator HacerShake()
    {

        Vector3 posicionInicial = camaraCinematica.transform.localPosition;

        float tiempo = 0;
        while (tiempo < duracionShake)
        {
            float offsetX = Random.Range(-1f, 1f) * intensidadShake;
            float offsetY = Random.Range(-1f, 1f) * intensidadShake;


            camaraCinematica.transform.localPosition = new Vector3(posicionInicial.x + offsetX, posicionInicial.y + offsetY, posicionInicial.z);

            tiempo += Time.deltaTime;
            yield return null;
        }

        camaraCinematica.transform.localPosition = posicionInicial;
    }
}
