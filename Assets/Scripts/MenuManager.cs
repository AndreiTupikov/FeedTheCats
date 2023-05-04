using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        string nextLevel = "Level " + (DataHolder.passedLevel + 1);
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath(nextLevel);
        if (sceneIndex < 0) SceneManager.LoadScene("Level " + DataHolder.passedLevel);
        else SceneManager.LoadScene(nextLevel);
    }

    public void Settings()
    {
        Debug.Log("Settings");
    }
}
