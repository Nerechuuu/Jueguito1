using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
    public Cinemachine.CinemachineImpulseSource impulseSource;

    // Método para activar el Impulse
    public void TriggerShake()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse(); // Esto generará el impulso en la cámara
            Debug.Log("Shake activado");
        }
    }
}
