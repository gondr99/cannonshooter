using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public Camera MainCam
    {
        get
        {
            if(_mainCam == null)
            {
                _mainCam = Camera.main;
            }
            return _mainCam;
        }
    }
    private Camera _mainCam;
    
    [SerializeField] private CinemachineVirtualCamera _cmRigcam;
    [SerializeField] private CinemachineVirtualCamera _cmActioncam;
    [SerializeField] private CinemachineVirtualCamera _cmCannoncam;

    private CinemachineBasicMultiChannelPerlin _bCannonPerlin;
    private CinemachineBasicMultiChannelPerlin _bActionPerlin;

    //활성화된 Vcam과 perinNoise
    private CinemachineVirtualCamera _activeVcam = null;
    private CinemachineBasicMultiChannelPerlin _activePerlin = null;

    private const int backPriority = 10;
    private const int frontPriority = 15;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Multiple CameraManager is running");
        }
        instance = this;

        _bCannonPerlin = _cmCannoncam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _bActionPerlin = _cmActioncam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _mainCam = Camera.main;
    }

    public void ShakeCam(float intensity, float time)
    {
        if (_activeVcam == null || _activePerlin == null) return; 
        StopAllCoroutines();
        StartCoroutine(ShakeCamCoroutine(intensity,time));
    }

    IEnumerator ShakeCamCoroutine(float intensity, float endTime)
    {
        _activePerlin.m_AmplitudeGain = intensity;
        float currentTime = 0f;
        while(currentTime < endTime)
        {
            yield return null;
            _activePerlin.m_AmplitudeGain = Mathf.Lerp(intensity, 0, currentTime / endTime);
            currentTime += Time.deltaTime;
        }
        _activePerlin.m_AmplitudeGain = 0;
    }

    public void SetCannonCamActive()
    {
        _cmCannoncam.Priority = frontPriority;
        _cmActioncam.Priority = backPriority;
        _cmRigcam.Priority = backPriority;

        _activePerlin = _bCannonPerlin;
        _activeVcam = _cmCannoncam;
    }

    public void SetActionCamActive(Transform trm)
    {
        _cmActioncam.m_Follow = trm;
        _cmCannoncam.Priority = backPriority;
        _cmActioncam.Priority = frontPriority;
        _cmRigcam.Priority = backPriority;

        _activePerlin = _bActionPerlin;
        _activeVcam = _cmActioncam;
    }

    public void SetRigCamActive()
    {
        _cmCannoncam.Priority = backPriority;
        _cmActioncam.Priority = backPriority;
        _cmRigcam.Priority = frontPriority;

        _activePerlin = null;
        _activeVcam = null;
    }

}
