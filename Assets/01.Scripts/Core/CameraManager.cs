using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    private Camera _mainCam;
    private CinemachineVirtualCamera _cmVcam;
    private CinemachineBasicMultiChannelPerlin _bPerlin;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple CameraManager is running");
        }
        instance = this;

        _cmVcam = Transform.FindObjectOfType<CinemachineVirtualCamera>();
        _bPerlin = _cmVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _mainCam = Camera.main;
    }

    public void ShakeCam(float intensity, float time)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeCamCoroutine(intensity,time));
    }

    IEnumerator ShakeCamCoroutine(float intensity, float endTime)
    {
        _bPerlin.m_AmplitudeGain = intensity;
        float currentTime = 0f;
        while(currentTime < endTime)
        {
            yield return null;
            _bPerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0, currentTime / endTime);
            currentTime += Time.deltaTime;
        }
    }
}
