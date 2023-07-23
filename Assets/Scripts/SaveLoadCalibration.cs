using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveLoadCalibration : MonoBehaviour
{
    // public GameObject statusGo;
    private Main main;
    private void Start()
    {
        // if (statusGo == null) statusGo = GameObject.Find("Status");
        // statusGo.GetComponent<TextMeshProUGUI>().text = "";
        main = GameObject.Find("Main").GetComponent<Main>();
    }
    public void SaveEyetrackingCalibration(string savefile = "et_calibration_settings.json")
    {
        string configPath = Application.persistentDataPath + "/Config/";
        if (!Directory.Exists(configPath)) Directory.CreateDirectory(configPath);
        string path = configPath + savefile;

        Main.EyeTrackingDirectionAdjustmentsSavedata data = new Main.EyeTrackingDirectionAdjustmentsSavedata();
        data.eyeTrackingDirectionAdjustments = main.EyeTrackingDirectionAdjustments;

        string json = JsonUtility.ToJson(data);

        try
        {
            File.WriteAllText(path, json);
            // writer = new StreamWriter(path);
            // writer.WriteLine(data);
            // writer.Flush();
            // writer.Close();
            ChangeStatusMessage($"Saving calibration data successful");
        }
        catch (System.Exception e)
        {
            ChangeStatusMessage($"Saving calibration data error {e}");
        }

    }

    public void LoadEyetrackingCalibration(string savefile = "et_calibration_settings.json")
    {
        string configPath = Application.persistentDataPath + "/Config/" + savefile;
        try
        {
            string json = File.ReadAllText(configPath);
            Main.EyeTrackingDirectionAdjustmentsSavedata data = new Main.EyeTrackingDirectionAdjustmentsSavedata();
            data = JsonUtility.FromJson<Main.EyeTrackingDirectionAdjustmentsSavedata>(json);
            print(data.eyeTrackingDirectionAdjustments.ToString());

            main.EyeTrackingDirectionAdjustments = data.eyeTrackingDirectionAdjustments;

            ChangeStatusMessage($"Loading calibration data successful");
        }
        catch (System.Exception e)
        {
            ChangeStatusMessage($"Loading calibration data error:\n {e}");
        }
    }
    public void ChangeStatusMessage(string status_msg)
    {
        print(status_msg);
        // statusGo.GetComponent<TextMeshProUGUI>().text = status_msg;
        // Invoke("HideStatus", 5);
    }

    // public void HideStatus() => statusGo.GetComponent<TextMeshProUGUI>().text = "";

    public void TestStatus() => ChangeStatusMessage("Test");
}
