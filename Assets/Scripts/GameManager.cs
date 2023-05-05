using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] cats;
    public int prepared = 0;
    public int finished = 0;
    public GameObject catsFight;
    public GameObject stars;
    public GameObject winPanel;
    public GameObject losePanel;
    public AudioSource music;
    private bool raseStarted = false;

    private void Awake()
    {
        if (!DataHolder.musicOn) music.enabled = false;
    }

    private void Update()
    {
        if (!raseStarted && Input.GetMouseButtonDown(0))
        {
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if (hit.collider != null && hit.transform.gameObject.CompareTag("Cat"))
            {
                hit.transform.parent.GetComponent<LineRenderer>().positionCount = 0;
                hit.transform.parent.GetComponent<DrawTrack>().trackDistance = 0;
                hit.transform.parent.GetComponent<DrawTrack>().enabled = true;
                if (hit.transform.parent.GetComponent<DrawTrack>().prepared)
                {
                    hit.transform.parent.GetComponent<DrawTrack>().prepared = false;
                    prepared--;
                }
            }
        }
    }

    public void RaceStart()
    {
        if (prepared == cats.Length)
        {
            raseStarted = true;
            float minDistance = cats.Select(c => c.transform.parent.GetComponent<DrawTrack>().trackDistance).Min();
            foreach (var cat in cats)
            {
                cat.GetComponent<CatMovement>().speed *= cat.transform.parent.GetComponent<DrawTrack>().trackDistance / minDistance;
                cat.GetComponent<CatMovement>().fullDistance = cat.transform.parent.GetComponent<DrawTrack>().trackDistance;
                cat.GetComponent<CatMovement>().enabled = true;
            }
        } 
    }

    public void CatsFight(GameObject cat1, GameObject cat2)
    {
        Vector2 position = new Vector2((cat1.transform.position.x + cat2.transform.position.x)/2, (cat1.transform.position.y + cat2.transform.position.y) / 2);
        var fight = Instantiate(catsFight, position, Quaternion.identity);
        if (!DataHolder.soundsOn) fight.GetComponent<AudioSource>().enabled = false;
        Destroy(fight, 10);
        StopCats();
        StartCoroutine(Lose());
    }

    public void Crash(GameObject cat)
    {
        Vector2 position = cat.transform.position;
        position.y += 0.3f;
        Instantiate(stars, position, Quaternion.identity);
        StopCats();
        StartCoroutine(Lose());
    }

    private void StopCats()
    {
        foreach (var cat in cats)
        {
            if (cat == null) continue;
            cat.GetComponent<Animator>().SetBool("Run", false);
            cat.GetComponent<CatMovement>().enabled = false;
        }
    }

    public void RaceFinish()
    {
        if (finished == cats.Length)
        {
            DataHolder.passedLevel = int.Parse(SceneManager.GetActiveScene().name.Split(' ')[1]);
            StartCoroutine(Win());
        }
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        winPanel.SetActive(true);
    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(1);
        losePanel.SetActive(true);
    }

    public void NextLevel()
    {
        if (DataHolder.soundsOn) gameObject.GetComponent<AudioSource>().Play();
        string nextLevel = "Level " + (int.Parse(SceneManager.GetActiveScene().name.Split(' ')[1]) + 1);
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath(nextLevel);
        if (sceneIndex < 0) ReturnToMenu();
        else SceneManager.LoadScene(nextLevel);
    }

    public void Replay()
    {
        if (DataHolder.soundsOn) gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        if (DataHolder.soundsOn) gameObject.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("Main Menu");
    }
}
