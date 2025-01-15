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
    [SerializeField] private CinemachineVirtualCamera camaraPrincipal;  // Asignada desde el Inspector
    [SerializeField] private CinemachineVirtualCamera camaraCinematica; // Asignada desde el Inspector

    [Header("GameObject a activar")]
    [SerializeField] private GameObject objetoAActivar;
    [SerializeField] private GameObject objetoAActivar2;

    private bool cinematicaEnProgreso = false;

    public void IniciarCinematica()
    {
        if (cinematicaEnProgreso) return;
        StartCoroutine(CinematicaFinal());
    }

    private IEnumerator CinematicaFinal()
    {
        cinematicaEnProgreso = true;

        // Desactivar el objeto al principio de la cinemática
            objetoAActivar.SetActive(false);
            objetoAActivar2.SetActive(false);

        // Bloquear control del jugador y animaciones
        player.enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsGliding", false);

        yield return new WaitForSeconds(1f);

        // Realizar dos shakes de cámara con 2 segundos entre ellos
        yield return StartCoroutine(HacerShake());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(HacerShake());

        // Activar el objeto después del shake (o en el momento que prefieras)
            objetoAActivar.SetActive(true);
            objetoAActivar2.SetActive(true);

        // Cambiar a la cámara cinemática usando prioridad
        camaraPrincipal.Priority = 0; // Baja la prioridad de la cámara principal
        camaraCinematica.Priority = 10; // Aumenta la prioridad de la cámara cinemática

        // Esperar 8 segundos para mostrar la cinemática
        yield return new WaitForSeconds(8f);

        // Volver a la cámara principal
        camaraPrincipal.Priority = 10; // Vuelve a dar prioridad a la cámara principal
        camaraCinematica.Priority = 0; // Baja la prioridad de la cámara cinemática
        
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);

        particulasFinales.Play();

        // Aplicar el efecto del botón al jugador
        player.CrecerANivel2Si();

        // Desbloquear el control del jugador
        player.enabled = true;

        cinematicaEnProgreso = false;
    }

    private IEnumerator HacerShake()
    {
    // Guardamos el objeto que la cámara sigue actualmente
    Transform objetoSeguidoOriginal = camaraPrincipal.Follow;

    // Desactivar el Follow de la cámara
    camaraPrincipal.Follow = null;

    // Guardamos la posición inicial de la cámara
    Vector3 posicionInicial = camaraPrincipal.transform.localPosition;

    float tiempo = 0;
    while (tiempo < duracionShake)
    {
        // Generar un movimiento aleatorio para el "shake"
        float offsetX = Random.Range(-1f, 1f) * intensidadShake;
        float offsetY = Random.Range(-1f, 1f) * intensidadShake;

        // Aplicar el movimiento a la cámara
        camaraPrincipal.transform.localPosition = new Vector3(posicionInicial.x + offsetX, posicionInicial.y + offsetY, posicionInicial.z);

        tiempo += Time.deltaTime;
        yield return null;
    }

    // Restaurar la posición original de la cámara
    camaraPrincipal.transform.localPosition = posicionInicial;

    // Restaurar el Follow a su valor original
    camaraPrincipal.Follow = objetoSeguidoOriginal;
    }

}
