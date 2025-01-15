using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D collider2D;

    [Header("Colliders para cada animación")]
    [SerializeField] private Vector2 colliderIdleSize = new Vector2(1, 2);
    [SerializeField] private Vector2 colliderWalkSize = new Vector2(1.2f, 2);
    [SerializeField] private Vector2 colliderJumpSize = new Vector2(1, 1.8f);
    [SerializeField] private Vector2 colliderFallSize = new Vector2(1, 1.5f);
    [SerializeField] private Vector2 colliderGlidingSize = new Vector2(1, 1.2f);

    private BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = collider2D as BoxCollider2D;

        if (boxCollider == null)
        {
            Debug.LogError("El collider no es un BoxCollider2D. Asegúrate de asignar uno.");
        }
    }

    private void Update()
    {
        if (boxCollider == null) return;

        UpdateColliderSize();
    }

    private void UpdateColliderSize()
    {
        if (animator.GetBool("IsGliding"))
        {
            boxCollider.size = colliderGlidingSize;
        }
        else if (animator.GetBool("IsJumping"))
        {
            boxCollider.size = colliderJumpSize;
        }
        else if (animator.GetBool("IsFalling"))
        {
            boxCollider.size = colliderFallSize;
        }
        else if (animator.GetBool("IsWalking"))
        {
            boxCollider.size = colliderWalkSize;
        }
        else
        {
            boxCollider.size = colliderIdleSize;
        }
    }
}
