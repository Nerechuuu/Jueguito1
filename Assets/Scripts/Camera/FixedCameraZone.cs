using UnityEngine;
using Cinemachine;

public class FixedCameraZone : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera fixedCamera;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fixedCamera.Priority = 10;
            mainCamera.Priority = 0;

            fixedCamera.transform.position = new Vector3(
                fixedCamera.transform.position.x,
                fixedCamera.transform.position.y,
                mainCamera.transform.position.z
            );
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            mainCamera.Priority = 10;
            fixedCamera.Priority = 0;
        }
    }
}
