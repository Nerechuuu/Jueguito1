using UnityEngine;

public class PlayerSlopeStepHandler : MonoBehaviour
{
    [Header("Step Offset")]
    [SerializeField] private float stepHeight = 0.3f; 
    [SerializeField] private float stepCheckDistance = 0.5f; 
    [SerializeField] private LayerMask stepLayer; 

    [Header("Slope Handling")]
    [SerializeField] private float maxSlopeAngle = 45f; 
    [SerializeField] private float slopeSpeedReduction = 0.5f; 
    [SerializeField] private LayerMask groundLayer; 

    [Header("References")]
    [SerializeField] private Transform groundDetector; 

    private Rigidbody2D rb;
    private Collider2D playerCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>(); 
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
        Vector2 direction = new Vector2(transform.localScale.x, 0).normalized;
        RaycastHit2D hitLow = Physics2D.Raycast(groundDetector.position, direction, stepCheckDistance, stepLayer);

        if (hitLow && hitLow.distance < stepCheckDistance)
        {
            Vector2 highCheckPosition = (Vector2)groundDetector.position + Vector2.up * stepHeight;
            RaycastHit2D hitHigh = Physics2D.Raycast(highCheckPosition, direction, stepCheckDistance, stepLayer);

            if (!hitHigh)
            {
                float obstacleHeight = Mathf.Abs(hitLow.point.y - groundDetector.position.y);

                if (obstacleHeight > 0.1f && obstacleHeight <= stepHeight)
                {
                    Vector2 targetPosition = rb.position + Vector2.up * obstacleHeight;
                    rb.position = Vector2.Lerp(rb.position, targetPosition, Time.fixedDeltaTime * 10f);
                }
            }
        }
    }


    private void HandleSlopes()
    {
        Vector2 origin = (Vector2)groundDetector.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, groundLayer);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle > maxSlopeAngle)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (slopeAngle > 0)
            {
                float speedFactor = 1 - (slopeAngle / maxSlopeAngle) * slopeSpeedReduction;
                rb.velocity = new Vector2(rb.velocity.x * speedFactor, rb.velocity.y);
            }
        }
    }

    private bool IsGrounded()
    {
        Vector2 origin = groundDetector.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, groundLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (groundDetector == null) return;

        Vector2 detectorPosition = groundDetector.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(detectorPosition, detectorPosition + Vector2.right * stepCheckDistance);
        Gizmos.DrawLine(detectorPosition + Vector2.up * stepHeight, detectorPosition + Vector2.up * stepHeight + Vector2.right * stepCheckDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(detectorPosition, detectorPosition + Vector2.down * 0.1f);
    }
}
