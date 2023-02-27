using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Main : DSingleton<Main>
{
    private Dictionary<int, string> avatar_id_dic = new Dictionary<int, string>();
    //Scene ID
    private int sceneIndex = 0;
    private bool menuIsDone;
    public static bool indexBool = true;
    public Vector3 EyeTrackingDirectionAdjustment = Vector3.zero;

    private GameObject leftHandController;
    private GameObject rightHandController;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        avatar_id_dic.Add(1, "Avatar_boy");
        avatar_id_dic.Add(2, "Avatar_jackfruit");
        avatar_id_dic.Add(3, "Avatar_watermelon");

    }
    float delaytime;
    bool shouldChange;
    void Update()
    {
        // Disable Controller Line Visual in all Scenes except Menu
        // if (sceneIndex != 0)
        // {
        //     GameObject.Find("LeftHand Controller").GetComponent<XRInteractorLineVisual>().enabled = false;
        //     GameObject.Find("RightHand Controller").GetComponent<XRInteractorLineVisual>().enabled = false;
        // }

        //Long Press Menu Button
        if (sceneIndex <= 1)
        {
            if (Input.GetKey(KeyCode.Escape) || (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.menuButton, out menuIsDone) && menuIsDone) || (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.menuButton, out menuIsDone) && menuIsDone))
            {
                delaytime += Time.deltaTime;
                if (delaytime >= 1)
                {
                    delaytime = 0;
                    GoCalibration();
                }
            }
        }

        //Return Menu
        if (((Input.GetKey(KeyCode.Escape) || (InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.menuButton, out menuIsDone) && menuIsDone) || (InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.menuButton, out menuIsDone) && menuIsDone))) && SceneManager.GetSceneByBuildIndex(0) != SceneManager.GetActiveScene())
        {
            delaytime = 0;
            GoMenu();
        }

        //Change Game Demo
        if (sceneIndex >= 2 && sceneIndex <= 4)
        {
            if (!shouldChange)
            {
                delaytime += Time.deltaTime;
                if (delaytime >= 0.5f)
                {
                    shouldChange = true;
                    delaytime = 0;
                }
            }
            if (shouldChange && (XR_GetKeyDown(XRNode.RightHand, CommonUsages.triggerButton) || XR_GetKeyDown(XRNode.LeftHand, CommonUsages.triggerButton)))
            {
                shouldChange = false;
                switch (sceneIndex)
                {
                    case 2:
                        GoET_moving();
                        break;
                    case 3:
                        GoET_shooting();
                        break;
                    case 4:
                        GoET_lighting();
                        break;
                }
            }
        }

        //Mirror Change Avatar
        if (sceneIndex == 5)
        {
            if (!shouldChange)
            {
                delaytime += Time.deltaTime;
                if (delaytime >= 0.5f)
                {
                    shouldChange = true;
                    delaytime = 0;
                }
            }
            if (shouldChange && (XR_GetKeyDown(XRNode.LeftHand, CommonUsages.triggerButton) || XR_GetKeyDown(XRNode.RightHand, CommonUsages.triggerButton)))
            {
                shouldChange = false;
                GameObject _smoketx = Instantiate(Resources.Load("Bang"), transform.Find("AvatarObject")) as GameObject;
                changeAvatar();
            }
        }
    }

    public void GoET_shooting()
    {
        sceneIndex = 4;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoET_lighting()
    {
        sceneIndex = 2;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoET_moving()
    {
        sceneIndex = 3;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoCoordinates()
    {
        sceneIndex = 1;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoCalibration()
    {
        // openPackage("com.tobii.usercalibration.pico");
        // openPackage("com.tobii.usercalibration.neo3");
        // sceneIndex = 6;
        string sceneName = "Calibration";
        SceneManager.LoadScene(sceneName);
    }
    public void GoMirror()
    {
        sceneIndex = 5;
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoMenu()
    {
        sceneIndex = 0;
        delaytime = 0;
        shouldChange = false;
        SceneManager.LoadScene(sceneIndex);
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
    int _nowAvatarId = 1;
    void changeAvatar()
    {
        if (GameObject.Find("AvatarObject/Avatar") != null)
        {
            Destroy(GameObject.Find("AvatarObject/Avatar").gameObject);
        }
        _nowAvatarId++;
        if (_nowAvatarId > 3)
        {
            _nowAvatarId = 1;
        }
        GameObject _avatar = Instantiate(Resources.Load(avatar_id_dic[_nowAvatarId]), GameObject.Find("AvatarObject").transform) as GameObject;
        _avatar.name = "Avatar";
    }
    public bool XR_GetKeyDown(XRNode node, InputFeatureUsage<bool> usage)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);
        bool isDone;
        if (device.TryGetFeatureValue(usage, out isDone) && isDone)
        {
            if (indexBool)
            {
                indexBool = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            indexBool = true;
            return false;
        }
    }
}
