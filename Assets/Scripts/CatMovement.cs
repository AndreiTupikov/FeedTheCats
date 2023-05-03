using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public LineRenderer track;
    public float speed;
    public float fullDistance;
    public float donePart = 0;
    public GameObject[] others;
    private float speedCorrection = 1;
    private int currentPosition = 0;
    private GameObject manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager");
        transform.position = track.GetPosition(currentPosition);
        currentPosition++;
    }

    private void Update()
    {
        if (currentPosition < track.positionCount)
        {
            foreach (var other in others)
            {
                if (donePart + 0.05f < other.GetComponent<CatMovement>().donePart)
                {
                    Debug.Log(transform.parent.name);
                    speedCorrection = 2f;
                    break;
                }
            }
            transform.position = Vector2.Lerp(transform.position, track.GetPosition(currentPosition), Time.deltaTime * speed * speedCorrection);
            //transform.Translate((track.GetPosition(currentPosition) - transform.position) * Time.deltaTime * speed * speedCorrection); 
            speedCorrection = 1;
            if (Vector2.Distance(transform.position, track.GetPosition(currentPosition)) < 0.1f)
            {
                donePart += Vector2.Distance(track.GetPosition(currentPosition - 1), track.GetPosition(currentPosition)) / fullDistance;
                currentPosition++;
            }
        }
        else
        {
            manager.GetComponent<GameManager>().finished++;
            manager.GetComponent<GameManager>().RaceFinish();
            gameObject.GetComponent<CatMovement>().enabled = false;
        }
    }
}
