using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private float ShakeIntensity = 1f;
    private float ShakeTime = 0.2f;

    private float timer;
    private CinemachineBasicMultiChannelPerlin _cbmp;
    private PlayerController _playerController; // Referencia al controlador del jugador

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _playerController = FindObjectOfType<PlayerController>(); // Encuentra al jugador en la escena
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        _cbmp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmp.m_AmplitudeGain = ShakeIntensity;
        timer = ShakeTime;
    }

    void StopShake()
    {
        CinemachineBasicMultiChannelPerlin _cbmp = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmp.m_AmplitudeGain = 0f;
    }

    void Update()
    {
        // Si el jugador está en modo grande y presiona el botón de salto (aquí se usa Space como ejemplo)
        if (_playerController.EsGrande() && Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera();
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                StopShake();
            }
        }
    }
}
