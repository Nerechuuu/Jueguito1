using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System.Collections;

public class CamaraBridgeTrigger : MonoBehaviour
{
    public CinemachineVirtualCamera camaraJugador;
    public CinemachineVirtualCamera camaraBridge;
    public PlayerController playerController;

    private bool eventoActivado = false;
    private Animator playerAnimator;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !eventoActivado)
        {
            StartCoroutine(ActivarEvento());
        }
    }

    private IEnumerator ActivarEvento()
    {
        eventoActivado = true;

        playerController.enabled = false;
        playerController.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsWalking", false);
            playerAnimator.SetBool("IsJumping", false);
            playerAnimator.SetBool("IsFalling", false);
            playerAnimator.SetBool("IsGliding", false);
        }

        camaraJugador.Priority = 0;
        camaraBridge.Priority = 10;

        yield return new WaitForSeconds(4f);

        camaraBridge.Priority = 0;
        camaraJugador.Priority = 10;

        if (playerController != null)
        {
            playerController.enabled = true;
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("IsWalking", true);
            }
        }
    }
}
