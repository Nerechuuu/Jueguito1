using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer))]
public class Boton : MonoBehaviour
{
    public enum TipoBoton { Crecer, Crecer2, Restablecer, RestablecerDesdeCrecido2, Reducir1, Reducir2, Reducir3 }
    [SerializeField] private TipoBoton tipoBoton;

    [Header("Colores")]
    [SerializeField] private Color colorInactivo = Color.white;
    [SerializeField] private Color colorActivo = Color.green;
    [SerializeField] private float duracionTransicionColor = 0.2f;

    [Header("Efectos Visuales")]
    [SerializeField] private List<EstadoParticulas> particulasPorEstado; // Lista de estados y partículas
    [SerializeField] private Transform posicionParticulas; // Posición para las partículas (opcional)

    private SpriteRenderer spriteRenderer;
    private Coroutine transicionCoroutine;
    private bool botonActivado;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetColor();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController jugador = other.GetComponent<PlayerController>();
        if (jugador == null) return;

        // Evitar interacción si el jugador está en Crecido2, excepto con RestablecerDesdeCrecido2
        if (jugador.EstaEnNivelDeReduccion(5) && tipoBoton != TipoBoton.RestablecerDesdeCrecido2) return;

        // Verificar si el estado ya está activo
        if (EstadoYaActivo(jugador)) return;

        // Aplicar la acción del botón
        switch (tipoBoton)
        {
            case TipoBoton.Crecer:
                jugador.Crecer();
                break;
            case TipoBoton.Crecer2:
                jugador.CrecerANivel2();
                break;
            case TipoBoton.Reducir1:
                jugador.ReducirANivel1();
                break;
            case TipoBoton.Reducir2:
                jugador.ReducirANivel2();
                break;
            case TipoBoton.Reducir3:
                jugador.ReducirANivel3();
                break;
            case TipoBoton.Restablecer:
                jugador.RestablecerEstado();
                break;
            case TipoBoton.RestablecerDesdeCrecido2:
                jugador.RestablecerDesdeCrecido2();
                break;
        }

        // Instanciar partículas específicas del estado
        InstanciarParticulas();

        ActivarBoton();
    }

    private bool EstadoYaActivo(PlayerController jugador)
    {
        switch (tipoBoton)
        {
            case TipoBoton.Crecer:
                return jugador.EstaEnNivelDeReduccion(4);
            case TipoBoton.Crecer2:
                return false; // Crecer2 siempre puede ejecutarse
            case TipoBoton.Reducir1:
                return jugador.EstaEnNivelDeReduccion(1);
            case TipoBoton.Reducir2:
                return jugador.EstaEnNivelDeReduccion(2);
            case TipoBoton.Reducir3:
                return jugador.EstaEnNivelDeReduccion(3);
            case TipoBoton.Restablecer:
                return jugador.EstaEnNivelDeReduccion(0);
            case TipoBoton.RestablecerDesdeCrecido2:
                return !jugador.EstaEnNivelDeReduccion(5);
            default:
                return false;
        }
    }

    private void InstanciarParticulas()
    {
        // Buscar el prefab correspondiente al estado actual del botón
        GameObject prefabParticulas = ObtenerPrefabDeParticulas(tipoBoton);

        if (prefabParticulas == null) return;

        // Usar la posición especificada o la posición del botón como fallback
        Vector3 posicion = posicionParticulas != null ? posicionParticulas.position : transform.position;

        // Instanciar el sistema de partículas
        Instantiate(prefabParticulas, posicion, Quaternion.identity);
    }

    private GameObject ObtenerPrefabDeParticulas(TipoBoton tipo)
    {
        foreach (var estadoParticulas in particulasPorEstado)
        {
            if (estadoParticulas.tipoBoton == tipo)
                return estadoParticulas.prefabParticulas;
        }
        return null; // No se encontró un prefab para este tipo de botón
    }

    private void ActivarBoton()
    {
        if (botonActivado) return;

        botonActivado = true;
        if (transicionCoroutine != null) StopCoroutine(transicionCoroutine);
        transicionCoroutine = StartCoroutine(TransicionColor(colorActivo));

        Invoke(nameof(ResetColor), 2f);
    }

    private void ResetColor()
    {
        botonActivado = false;
        if (transicionCoroutine != null) StopCoroutine(transicionCoroutine);
        transicionCoroutine = StartCoroutine(TransicionColor(colorInactivo));
    }

    private System.Collections.IEnumerator TransicionColor(Color targetColor)
    {
        Color inicio = spriteRenderer.color;
        float tiempo = 0f;

        while (tiempo < duracionTransicionColor)
        {
            spriteRenderer.color = Color.Lerp(inicio, targetColor, tiempo / duracionTransicionColor);
            tiempo += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = targetColor;
    }

    [System.Serializable]
    public class EstadoParticulas
    {
        public TipoBoton tipoBoton;
        public GameObject prefabParticulas;
    }
}
