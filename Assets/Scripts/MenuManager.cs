using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public AudioSource music;
    public GameObject settingsPanel;
    public Image soundsButton;
    public Image musicButton;
    public Sprite soundOn;
    public Sprite soundOff;

    private void Awake()
    {
        if (!DataHolder.musicOn) music.enabled = false;
    }

    public void Play()
    {
        if (DataHolder.soundsOn) gameObject.GetComponent<AudioSource>().Play();
        string nextLevel = "Level " + (DataHolder.passedLevel + 1);
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath(nextLevel);
        if (sceneIndex < 0) SceneManager.LoadScene("Level " + DataHolder.passedLevel);
        else SceneManager.LoadScene(nextLevel);
    }

    public void OpenSettings()
    {
        if (DataHolder.soundsOn) gameObject.GetComponent<AudioSource>().Play();
        else soundsButton.sprite = soundOff;
        if (!DataHolder.musicOn) musicButton.sprite = soundOff;
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (DataHolder.soundsOn) gameObject.GetComponent<AudioSource>().Play();
        settingsPanel.SetActive(false);
    }

    public void SoundsControl()
    {
        DataHolder.soundsOn = !DataHolder.soundsOn;
        if (DataHolder.soundsOn)
        {
            gameObject.GetComponent<AudioSource>().Play();
            soundsButton.sprite = soundOn;
        }
        else soundsButton.sprite = soundOff;
    }

    public void MusicControl()
    {
        if (DataHolder.soundsOn) gameObject.GetComponent<AudioSource>().Play();
        DataHolder.musicOn = !DataHolder.musicOn;
        if (DataHolder.musicOn)
        {
            musicButton.sprite = soundOn;
            music.enabled = true;
        }
        else
        {
            musicButton.sprite = soundOff;
            music.enabled = false;
        }
    }
}
