using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] public CameraState cameraState;
    [SerializeField] private CinemachineVirtualCamera _fpsCamera;
    [SerializeField] private CinemachineFreeLook _tpsCamera;
    [SerializeField] private InputManager _inputManager;

    public Action onChangePerspective;

    public void Start()
    {
        _inputManager.onChangePOV += SwitchCamera;
    }

    public void OnDestroy()
    {
        _inputManager.onChangePOV -= SwitchCamera;
    }


    public void SetFPSClampedCamera(bool isClamped, Vector3 playerRotation)
    {
        CinemachinePOV pov = _fpsCamera.GetCinemachineComponent<CinemachinePOV>();
        if (isClamped)
        {
            pov.m_HorizontalAxis.m_Wrap = false;
            pov.m_HorizontalAxis.m_MinValue = playerRotation.y - 45;
            pov.m_HorizontalAxis.m_MaxValue = playerRotation.y + 45;
        }
        else
        {
            pov.m_HorizontalAxis.m_MinValue = -180;
            pov.m_HorizontalAxis.m_MaxValue = 180;
            pov.m_HorizontalAxis.m_Wrap = true;
        }
    }

    private void SwitchCamera()
    {
        if (cameraState == CameraState.ThirdPerson)
        {
            cameraState = CameraState.FirstPerson;
            _tpsCamera.gameObject.SetActive(false);
            //_tpsCamera.Priority = 10;
            _fpsCamera.gameObject.SetActive(true);
            //_fpsCamera.Priority = 20;
        }
        else
        {
            cameraState = CameraState.ThirdPerson;
            _tpsCamera.gameObject.SetActive(true);
            //_tpsCamera.Priority = 20;
            _fpsCamera.gameObject.SetActive(false);
            //_fpsCamera.Priority = 10;

        }
        onChangePerspective?.Invoke();
    }

    public void SetTPSFieldOfView(float fieldOfView)
    {
        _tpsCamera.m_Lens.FieldOfView = fieldOfView;
    }


}
