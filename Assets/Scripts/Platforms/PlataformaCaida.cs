using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaCaida : MonoBehaviour
{
    [Header("Configuraci�n de la Plataforma")]
    [SerializeField] private LayerMask capaJugador;

    private Rigidbody2D rb;
    private bool jugadorCerca;
    private PlayerController jugadorActual;


    private void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    rb.bodyType = RigidbodyType2D.Static;
    rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
}


    private void Update()
    {
        if (jugadorCerca && jugadorActual != null && jugadorActual.EsGrande())
        {
            if (rb.bodyType != RigidbodyType2D.Dynamic)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaJugador) != 0)
        {
            PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugadorCerca = true;
                jugadorActual = jugador;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaJugador) != 0)
        {
            PlayerController jugador = collision.gameObject.GetComponent<PlayerController>();
            if (jugador != null && jugador == jugadorActual)
            {
                jugadorCerca = false;
                jugadorActual = null;
            }
        }
    }
}