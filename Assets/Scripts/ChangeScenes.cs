using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenes : MonoBehaviour
{
    public void ChangeScenceToNamedInstance(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void ChangeScenceToMirror()
    {
        Main.Instance.GoMirror();
    }

    public void ChangeScenceToGame()
    {
        Main.Instance.GoET_lighting();
    }

    public void ChangeScenceToCalibration()
    {
        Main.Instance.GoCalibration();
    }

    public void ChangeScenceToCoordinates()
    {
        Main.Instance.GoCoordinates();
    }
}
