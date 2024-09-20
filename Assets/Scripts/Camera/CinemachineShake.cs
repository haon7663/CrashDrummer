using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    public float shakeTimer;

    private Camera MainCamera;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        MainCamera = Camera.main;
    }

    public void ShakeCamera(float intensity, float time)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }
        if (shakeTimer <= 0f)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
