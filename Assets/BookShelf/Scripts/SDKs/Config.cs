using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LionStudios.Suite.Debugging;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-5)]
public class Config : MonoBehaviour
{

    private void Awake()
    {
        MaxSdk.SetSdkKey("_U0VESiTlK8AbZn8Fgr57W7HHGZwqN8EeRhAAF0RJCZZ0K4FqmlzpgwQ-SewrfVrorzR_XiM0kd704zRH0we1m");
        MaxSdk.SetUserId(SystemInfo.deviceUniqueIdentifier);
        MaxSdk.SetVerboseLogging(true);
        MaxSdk.InitializeSdk();
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // Show Mediation Debugger
            MaxSdk.ShowMediationDebugger();
        };
    }
    // Start is called before the first frame update
    void Start()
    {
        LionDebugger.Hide();
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
