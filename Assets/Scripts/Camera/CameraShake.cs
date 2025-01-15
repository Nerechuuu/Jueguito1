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
    private PlayerController _playerController;

    void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _playerController = FindObjectOfType<PlayerController>();
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
