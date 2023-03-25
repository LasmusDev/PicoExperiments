using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Main : DSingleton<Main>
{
    private Dictionary<int, string> avatar_id_dic = new Dictionary<int, string>();
    private bool menuIsDone;
    public static bool XrKeydownIndexBool = true;
    public List<Vector3> EyeTrackingDirectionAdjustments = new();

    private GameObject leftHandController;
    private GameObject rightHandController;

    float delaytime;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
    }
    void Update()
    {
        //Long Press Menu Button
        if (((Input.GetKey(KeyCode.Escape) || (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.menuButton, out menuIsDone) && menuIsDone) || (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.menuButton, out menuIsDone) && menuIsDone))) && SceneManager.GetSceneByBuildIndex(0) != SceneManager.GetActiveScene())
        {
            delaytime += Time.deltaTime;
            if (delaytime >= 2)
            {
                delaytime = 0;
                if (SceneManager.GetActiveScene().name == "00_Menu")
                    SceneManager.LoadScene("Calibration");
                else
                    SceneManager.LoadScene(0);
            }
        }
    }

    public void openPackage(string pkgName)
    {
        using (AndroidJavaClass jcPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject joActivity = jcPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaObject joPackageManager = joActivity.Call<AndroidJavaObject>("getPackageManager"))
                {
                    using (AndroidJavaObject joIntent = joPackageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", pkgName))
                    {
                        if (null != joIntent)
                        {
                            joActivity.Call("startActivity", joIntent);
                        }
                    }
                }
            }
        }
    }
    public bool XR_GetKeyDown(XRNode node, InputFeatureUsage<bool> usage)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);
        bool isDone;
        if (device.TryGetFeatureValue(usage, out isDone) && isDone)
        {
            if (XrKeydownIndexBool)
            {
                XrKeydownIndexBool = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            XrKeydownIndexBool = true;
            return false;
        }
    }
    public static void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
