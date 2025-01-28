using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TransitionCredits : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Cámaras")]
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera transitionCamera;
    [SerializeField] private CinemachineVirtualCamera transitionCamera2;

    [Header("Animación a activar")]
    [SerializeField] private GameObject AnimacionAActivar;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Iniciando transición...");
            StartCoroutine(TransitionSequence());
        }
    }

    private IEnumerator TransitionSequence()
    {
        AnimacionAActivar.SetActive(false);

        player.enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Animator animator = player.GetComponent<Animator>();
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        animator.SetBool("IsGliding", false);

        //Que se quede quito (el pj se mueve solo)

        yield return new WaitForSeconds(2f);

        mainCamera.Priority = 0;
        transitionCamera.Priority = 10;

        yield return new WaitForSeconds(3f);

        transitionCamera.Priority = 0;
        transitionCamera2.Priority = 10;

        yield return new WaitForSeconds(3f);

        AnimacionAActivar.SetActive(true);

        yield return new WaitForSeconds(7f);

        SceneManager.LoadScene("FinalCredits");
    }
}
