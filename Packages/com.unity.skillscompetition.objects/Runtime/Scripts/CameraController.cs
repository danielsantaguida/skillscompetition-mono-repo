using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    private float _mSensitivity;
    private float _mXRotation;
    private float _mYRotation;
    private bool _mCameraEnabled;

    public static Action<bool, bool, CursorLockMode> SetCursorLock;
    public static Action<InputAction.CallbackContext> Look;

    void Start()
    {
        _mCameraEnabled = true;
        _mSensitivity = PlayerPrefs.GetInt(OptionsPage.PlayerPrefsSensitivityKey) / 100f;
        SetCursorLock += OnCursorLockChange;
        Look += OnLook;
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        if (!_mCameraEnabled)
            return;
        
        var rotation = context.ReadValue<Vector2>();
        
        //TODO: Figure out why this needs to be flipped
        _mXRotation += rotation.x * _mSensitivity;
        _mYRotation -= rotation.y * _mSensitivity;

        _mXRotation = Mathf.Clamp(_mXRotation, -75f, 75f);
        _mYRotation = Mathf.Clamp(_mYRotation, -75f, 45f);
        
        transform.localRotation = Quaternion.Euler(_mYRotation, _mXRotation, 0);
    }
    
    private void OnCursorLockChange(bool cameraEnabled, bool visible, CursorLockMode cursorLockMode)
    {
        _mCameraEnabled = cameraEnabled;
        Cursor.lockState = cursorLockMode;
        Cursor.visible = visible;
    }

    void OnDestroy()
    {
        SetCursorLock -= OnCursorLockChange;
        Look -= OnLook;
    }
}
