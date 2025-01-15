using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float moveDistance = 5f;

    private Vector3 startPosition;
    private bool movingUp = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float direction = movingUp ? 1f : -1f;

        transform.position += Vector3.up * direction * moveSpeed * Time.deltaTime;

        if (movingUp && transform.position.y >= startPosition.y + moveDistance)
        {
            movingUp = false;
        }
        else if (!movingUp && transform.position.y <= startPosition.y)
        {
            movingUp = true;
        }
    }
}
