using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

    }

    // Update is called once per frame
    public IEnumerator ShakeCam(float intenstity, float duration)
    {
        shakeTimer = 0f;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        while (shakeTimer < duration)
        {
            shakeTimer += Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intenstity;
            yield return null;
        }
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }
}
