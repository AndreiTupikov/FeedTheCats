using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] cats;
    public int prepared = 0;
    public int finished = 0;
    private bool raseStarted = false;

    private void Update()
    {
        if (!raseStarted && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.CompareTag("Cat"))
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

    public void RaceFinish()
    {
        if (finished == cats.Length)
        {
            Debug.Log("Finish");
        }
    }

    public void ReturnToMenu()
    {
        Debug.Log("Return To Menu");
    }
}
