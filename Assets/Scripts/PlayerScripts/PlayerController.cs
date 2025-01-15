using UnityEngine;
using System.Collections;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadNormal = 5f;
    [SerializeField] private float velocidadReducido1 = 6f;
    [SerializeField] private float velocidadReducido2 = 7f;
    [SerializeField] private float velocidadReducido3 = 8f;
    [SerializeField] private float velocidadCrecido = 4f;
    [SerializeField] private float velocidadCrecido2 = 3f;

    [Header("Salto")]
    [SerializeField] private float alturaSaltoConstante = 3f;
    [SerializeField] private float gravedadNormal = 2.5f;
    [SerializeField] private float gravedadPlaneo = 0.8f;
    [SerializeField] private float gravedadCrecido = 5f;
    [SerializeField] private float gravedadCrecido2 = 6f;

    [Header("Tamaño del Personaje")]
    [SerializeField] private Vector3 escalaNormal = new Vector3(1, 1, 1);
    [SerializeField] private Vector3 escalaReducido1 = new Vector3(0.75f, 0.75f, 1);
    [SerializeField] private Vector3 escalaReducido2 = new Vector3(0.5f, 0.5f, 1);
    [SerializeField] private Vector3 escalaReducido3 = new Vector3(0.25f, 0.25f, 1);
    [SerializeField] private Vector3 escalaCrecido = new Vector3(1.5f, 1.5f, 1);
    [SerializeField] private Vector3 escalaCrecido2 = new Vector3(2, 2, 1);
    [SerializeField] private AnimationCurve curvaDeAnimacion;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform puntoSuelo;
    [SerializeField] private float radioDeteccion = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Partículas")]
    [SerializeField] private ParticleSystem particulasSalto;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    [Header("Coyote Time")]
    [SerializeField] private float tiempoCoyote = 0.1f; // Tiempo permitido después de salir del suelo
    private float tiempoDesdeQueSalioDelSuelo; // Temporizador para Coyote Time

    [Header("Cámaras")]
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera secondaryCamera1;
    public CinemachineVirtualCamera secondaryCamera2;
    public CinemachineVirtualCamera secondaryCamera3;

    [Header("Shake")]
    [SerializeField] private float shakeIntensity = 3f;
    [SerializeField] private float shakeDuration = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool puedeSaltar;
    private bool estaEnSuelo;
    private int nivelEncogimiento = 0;
    private bool bloqueadoPorAnimacion = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (bloqueadoPorAnimacion)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * ObtenerVelocidad(), rb.velocity.y);

        if (inputHorizontal != 0)
        {
            spriteRenderer.flipX = inputHorizontal < 0;
        }

        // Salto con teclas W, Espacio, Flecha Arriba y botón X del mando
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetButtonDown("Jump")) && PuedeUsarCoyoteTime())
        {
            Saltar();
        }

        AjustarGravedad();
        VerificarSuelo();
        ActualizarAnimaciones(inputHorizontal);
    }

    private void VerificarSuelo()
    {
        bool estabaEnSuelo = estaEnSuelo;
        estaEnSuelo = Physics2D.OverlapCircle(puntoSuelo.position, radioDeteccion, capaSuelo);

        if (estaEnSuelo)
        {
            tiempoDesdeQueSalioDelSuelo = 0; // Reiniciar el temporizador si estamos en el suelo
            if (!estabaEnSuelo) // Si el jugador acaba de aterrizar
            {
                StartCoroutine(AplicarShake()); // Aplicar shake
            }
        }
        else if (estabaEnSuelo && !estaEnSuelo)
        {
            tiempoDesdeQueSalioDelSuelo = Time.time; // Registrar el tiempo al salir del suelo
        }

        puedeSaltar = estaEnSuelo; // Esto sigue igual para compatibilidad con otras partes del script
    }

    private bool PuedeUsarCoyoteTime()
    {
        // Permitir el salto si está en el suelo o si estamos dentro del tiempo Coyote
        return estaEnSuelo || Time.time - tiempoDesdeQueSalioDelSuelo <= tiempoCoyote;
    }

    private void Saltar()
    {
        if (particulasSalto != null)
        {
            particulasSalto.Play();
        }

        float alturaDesdePies = alturaSaltoConstante + transform.localScale.y / 2;
        float fuerzaSalto = Mathf.Sqrt(2 * alturaDesdePies * gravedadNormal);

        rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);

        PlataformaToggle.AlternarGrupos();
    }

    private IEnumerator AplicarShake()
    {
        if (nivelEncogimiento == 4 || nivelEncogimiento == 5)
        {
            Transform objetoSeguidoOriginal = mainCamera.Follow;
            Transform objetoSeguidoOriginal2 = secondaryCamera3.Follow;
            mainCamera.Follow = null;
            secondaryCamera3.Follow = null;

            Vector3 posicionInicial1 = secondaryCamera1.transform.localPosition;
            Vector3 posicionInicial2 = secondaryCamera2.transform.localPosition;
            Vector3 posicionInicialMC = mainCamera.transform.localPosition;
            Vector3 posicionInicial3 = secondaryCamera3.transform.localPosition;

            float tiempo = 0;
            while (tiempo < shakeDuration)
            {
                // Generar un movimiento aleatorio para el "shake"
                float offsetX = Random.Range(-1f, 1f) * shakeIntensity;
                float offsetY = Random.Range(-1f, 1f) * shakeIntensity;

                // Aplicar el movimiento a la cámara
                secondaryCamera1.transform.localPosition = new Vector3(posicionInicial1.x + offsetX, posicionInicial1.y + offsetY, posicionInicial1.z);
                secondaryCamera2.transform.localPosition = new Vector3(posicionInicial2.x + offsetX, posicionInicial2.y + offsetY, posicionInicial2.z);
                mainCamera.transform.localPosition = new Vector3(posicionInicialMC.x + offsetX, posicionInicialMC.y + offsetY, posicionInicialMC.z);
                secondaryCamera3.transform.localPosition = new Vector3(posicionInicial3.x + offsetX, posicionInicial3.y + offsetY, posicionInicial3.z);

                tiempo += Time.deltaTime;
                yield return null;
            }

            // Restaurar la posición original de la cámara
            secondaryCamera1.transform.localPosition = posicionInicial1;
            secondaryCamera2.transform.localPosition = posicionInicial2;
            mainCamera.transform.localPosition = posicionInicialMC;
            secondaryCamera3.transform.localPosition = posicionInicial3;

            mainCamera.Follow = objetoSeguidoOriginal;
            secondaryCamera3.Follow = objetoSeguidoOriginal2;
        }
    }

    private void AjustarGravedad()
    {
        if (nivelEncogimiento == 5)
        {
            rb.gravityScale = gravedadCrecido2;
        }
        else if (nivelEncogimiento == 4)
        {
            rb.gravityScale = gravedadCrecido;
        }
        else if (!estaEnSuelo && rb.velocity.y < 0)
        {
            rb.gravityScale = gravedadPlaneo;
        }
        else
        {
            rb.gravityScale = gravedadNormal;
        }
    }

    private float ObtenerVelocidad()
    {
        return nivelEncogimiento switch
        {
            1 => velocidadReducido1,
            2 => velocidadReducido2,
            3 => velocidadReducido3,
            4 => velocidadCrecido,
            5 => velocidadCrecido2,
            _ => velocidadNormal,
        };
    }

    private void ActualizarAnimaciones(float inputHorizontal)
    {
        if (estaEnSuelo)
        {
            animator.SetBool("IsFalling", false);
            animator.SetBool("IsGliding", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsWalking", inputHorizontal != 0);
        }
        else
        {
            animator.SetBool("IsWalking", false);

            if (rb.velocity.y > 0)
            {
                animator.SetBool("IsJumping", true);
                animator.SetBool("IsFalling", false);
                animator.SetBool("IsGliding", false);
            }
            else if (rb.velocity.y < 0)
            {
                animator.SetBool("IsJumping", false);

                if (nivelEncogimiento == 4 || nivelEncogimiento == 5)
                {
                    animator.SetBool("IsFalling", true);
                    animator.SetBool("IsGliding", false);
                }
                else
                {
                    animator.SetBool("IsFalling", false);
                    animator.SetBool("IsGliding", true);
                }
            }
        }
    }

    public bool EsGrande()
    {
        return nivelEncogimiento == 4 || nivelEncogimiento == 5;
    }

    public bool EstaEnNivelDeReduccion(int nivel)
    {
        return nivelEncogimiento == nivel;
    }

    public void ReducirANivel1()
    {
        if (nivelEncogimiento == 0 || nivelEncogimiento == 2)
        {
            nivelEncogimiento = 1;
            CambiarEscala(escalaReducido1);
        }
    }

    public void ReducirANivel2()
    {
        if (nivelEncogimiento == 1)
        {
            nivelEncogimiento = 2;
            CambiarEscala(escalaReducido2);
        }
    }

    public void ReducirANivel3()
    {
        if (nivelEncogimiento == 2)
        {
            nivelEncogimiento = 3;
            CambiarEscala(escalaReducido3);
        }
    }

    public void Crecer()
    {
        nivelEncogimiento = 4;
        CambiarEscala(escalaCrecido);
    }

    public void CrecerANivel2()
    {
        // Iniciar la cinemática
        FindObjectOfType<FinalCinematicIraController>()?.IniciarCinematica();
    }

    public void CrecerANivel2Si()
    {
        nivelEncogimiento = 5;
        CambiarEscala(escalaCrecido2);
    }

    public void RestablecerEstado()
    {
        nivelEncogimiento = 0;
        CambiarEscala(escalaNormal);
    }

    public void RestablecerDesdeCrecido2()
    {

        FindObjectOfType<RestablecerGrandeController>()?.Restablecer();
    }

    public void RestablecerDesdeCrecido2Si()
    {
        if (nivelEncogimiento == 5)
        {
            nivelEncogimiento = 0;
            CambiarEscala(escalaNormal);
        }
    }

    private void CambiarEscala(Vector3 nuevaEscala)
    {
        float duracionAnimacion = 0.5f;
        StartCoroutine(AnimarCambioDeTamaño(nuevaEscala, duracionAnimacion));
    }

    private IEnumerator AnimarCambioDeTamaño(Vector3 escalaObjetivo, float duracion)
    {
        bloqueadoPorAnimacion = true;

        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsGliding", false);

        Vector3 escalaInicial = transform.localScale;
        float tiempo = 0;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float progreso = curvaDeAnimacion.Evaluate(tiempo / duracion);
            transform.localScale = Vector3.Lerp(escalaInicial, escalaObjetivo, progreso);
            yield return null;
        }

        transform.localScale = escalaObjetivo;
        bloqueadoPorAnimacion = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(puntoSuelo.position, radioDeteccion);
    }

    public bool EresMuyGrande()
    {
        if (nivelEncogimiento == 5)
            return true;
        else
            return false;
    }
}
