using UnityEngine;

public class PlayerSlopeStepHandler : MonoBehaviour
{
    [Header("Step Offset")]
    [SerializeField] private float stepHeight = 0.3f; // Altura máxima del escalón
    [SerializeField] private float stepCheckDistance = 0.5f; // Distancia para detectar el escalón
    [SerializeField] private LayerMask stepLayer; // Capas que se consideran escalones

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle = 45f; // Ángulo máximo de pendiente manejable
    [SerializeField] private float slopeSpeedReduction = 0.5f; // Reducción de velocidad en pendientes
    [SerializeField] private LayerMask groundLayer; // Capas que se consideran suelo

    [Header("References")]
    [SerializeField] private Transform groundDetector; // Objeto vacío que detecta el suelo

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>(); // Referencia automática al colisionador
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            HandleStepOffset();
            HandleSlopes();
        }
    }

    private void HandleStepOffset()
    {
        // Detectar si hay un obstáculo bajo en frente
        Vector2 direction = new Vector2(transform.localScale.x, 0).normalized;
        RaycastHit2D hitLow = Physics2D.Raycast(groundDetector.position, direction, stepCheckDistance, stepLayer);

        if (hitLow && hitLow.distance < stepCheckDistance)
        {
            // Detectar si hay espacio libre arriba para subir el escalón
            Vector2 highCheckPosition = (Vector2)groundDetector.position + Vector2.up * stepHeight;
            RaycastHit2D hitHigh = Physics2D.Raycast(highCheckPosition, direction, stepCheckDistance, stepLayer);

            if (!hitHigh)
            {
                // Subir el escalón solo si el obstáculo tiene suficiente altura para ser considerado un escalón
                float obstacleHeight = Mathf.Abs(hitLow.point.y - groundDetector.position.y);

                if (obstacleHeight > 0.1f && obstacleHeight <= stepHeight) // Ajuste de umbral mínimo
                {
                    // Usar interpolación para suavizar el ajuste
                    Vector2 targetPosition = rb.position + Vector2.up * obstacleHeight;
                    rb.position = Vector2.Lerp(rb.position, targetPosition, Time.fixedDeltaTime * 10f);
                }
            }
        }
    }


    private void HandleSlopes()
    {
        // Detectar pendiente bajo el jugador
        Vector2 origin = (Vector2)groundDetector.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, groundLayer);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle > maxSlopeAngle)
            {
                // Evitar que el jugador se deslice en pendientes pronunciadas
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (slopeAngle > 0)
            {
                // Reducir velocidad en pendientes
                float speedFactor = 1 - (slopeAngle / maxSlopeAngle) * slopeSpeedReduction;
                rb.velocity = new Vector2(rb.velocity.x * speedFactor, rb.velocity.y);
            }
        }
    }

    private bool IsGrounded()
    {
        // Verificar si el jugador está tocando el suelo
        Vector2 origin = groundDetector.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, groundLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (groundDetector == null) return;

        // Visualizar los rayos para depuración
        Vector2 detectorPosition = groundDetector.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(detectorPosition, detectorPosition + Vector2.right * stepCheckDistance);
        Gizmos.DrawLine(detectorPosition + Vector2.up * stepHeight, detectorPosition + Vector2.up * stepHeight + Vector2.right * stepCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(detectorPosition, detectorPosition + Vector2.down * 0.1f);
    }
}
