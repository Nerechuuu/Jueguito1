using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    private Vector3 startPosition;
    private bool movingRight = true;
    private bool isActivated = false;  // Para saber si la plataforma ya ha sido activada

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (isActivated)
        {
            // Mover la plataforma solo si ha sido activada
            float direction = movingRight ? 1f : -1f;

            transform.position += Vector3.right * direction * moveSpeed * Time.deltaTime;

            if (movingRight && transform.position.x >= startPosition.x + moveDistance)
            {
                movingRight = false;
            }
            else if (!movingRight && transform.position.x <= startPosition.x)
            {
                movingRight = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobar si el jugador ha tocado el trigger
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;  // Activar la plataforma solo la primera vez que el jugador entra en el trigger
        }
    }
}
