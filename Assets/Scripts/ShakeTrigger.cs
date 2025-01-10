using UnityEngine;
using Cinemachine;

public class ShakeTrigger : MonoBehaviour
{
    public Cinemachine.CinemachineImpulseSource impulseSource;

    public void TriggerShake()
{
    // Código para activar el shake de la cámara.
    CinemachineImpulseSource impulseSource = GetComponent<CinemachineImpulseSource>();
    impulseSource.GenerateImpulse();
}
}
