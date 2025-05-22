using Google.XR.Cardboard;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class VrModeController : MonoBehaviour
{
    //ссылка на компонент для переключения камер в зависимости от режима игры
    [SerializeField] CameraController _cameraController;

    /// <summary>
    /// Gets a value indicating whether the screen has been touched this frame.
    /// </summary>
    private bool IsScreenTouched
    {
        get
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the VR mode is enabled.
    /// </summary>
    private bool IsVrModeEnabled
    {
        get
        {
            return XRGeneralSettings.Instance.Manager.isInitializationComplete;
        }
    }

    void Start()
    {
        // Configures the app to not shut down the screen and sets the brightness to maximum.
        // Brightness control is expected to work only in iOS, see:
        // https://docs.unity3d.com/ScriptReference/Screen-brightness.html.
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.brightness = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsVrModeEnabled)
        {
            if (Api.IsCloseButtonPressed)
            {
                ExitVR();
            }

            Api.UpdateScreenParams();
        }
        //else
        //{
        // TODO(b/171727815): Add a button to switch to VR mode.
        //if (IsScreenTouched)
        //{
        //    EnterVR();
        //}
        //}
    }

    //метод для включения/выключения VR-режима через внешний интерфейс
    //например, через кнопку
    public void ActivateVR(bool activate)
    {
        if (activate)
            EnterVR();
        else
            ExitVR();
    }

    /// <summary>
    /// Enters VR mode.
    /// </summary>
    private void EnterVR()
    {
        StartCoroutine(StartXR());
    }

    /// <summary>
    /// Exits VR mode.
    /// </summary>
    private void ExitVR()
    {
        StopXR();
    }

    /// <summary>
    /// Initializes and starts the Cardboard XR plugin.
    /// See https://docs.unity3d.com/Packages/com.unity.xr.management@3.2/manual/index.html.
    /// </summary>
    ///
    /// <returns>
    /// Returns result value of <c>InitializeLoader</c> method from the XR General Settings Manager.
    /// </returns>
    private IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed.");
        }
        else
        {
            Debug.Log("XR initialized.");

            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR started.");

            //включение VR-камеры
            _cameraController.SetVRCameraActive(true);
        }
    }

    /// <summary>
    /// Stops and deinitializes the Cardboard XR plugin.
    /// See https://docs.unity3d.com/Packages/com.unity.xr.management@3.2/manual/index.html.
    /// </summary>
    private void StopXR()
    {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Debug.Log("XR stopped.");

        Debug.Log("Deinitializing XR...");
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR deinitialized.");

        //выключение VR-камеры, включение 3D-камеры
        _cameraController.SetVRCameraActive(false);
    }

    //метод для сброса центра в VR-режиме
    public void RecenterView()
    {
        StartCoroutine(Recenter());
    }

    private IEnumerator Recenter()
    {
        //сброс центра через 3 секунды задержки
        yield return new WaitForSeconds(3f);
        Api.Recenter();
    }
}