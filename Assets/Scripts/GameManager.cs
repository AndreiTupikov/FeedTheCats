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
    public GameObject winPanel;
    public GameObject losePanel;
    private bool raseStarted = false;

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
        Destroy(fight, 5);
        foreach (var cat in cats)
        {
            if (cat == null || cat == cat1 || cat == cat2) continue;
            cat.GetComponent<Animator>().SetBool("Run", false);
            cat.GetComponent<CatMovement>().enabled = false;
        }
        losePanel.SetActive(true);
    }

    public void RaceFinish()
    {
        if (finished == cats.Length)
        {
            winPanel.SetActive(true);
        }
    }

    public void NextLevel()
    {
        string nextLevel = "Level " + (int.Parse(SceneManager.GetActiveScene().name.Split(' ')[1]) + 1);
        int sceneIndex = SceneUtility.GetBuildIndexByScenePath(nextLevel);
        if (sceneIndex < 0) ReturnToMenu();
        else SceneManager.LoadScene(nextLevel);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
