using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public LineRenderer track;
    public float speed;
    public float fullDistance;
    public float donePart = 0;
    public GameObject[] others;
    private int currentPosition = 0;
    private GameObject manager;
    private GameObject leader;

    private void Start()
    {
        manager = GameObject.Find("GameManager");
        gameObject.GetComponent<Animator>().SetBool("Run", true);
        transform.position = track.GetPosition(currentPosition);
        currentPosition++;
        foreach (GameObject other in others)
        {
            if (leader == null || leader.GetComponent<CatMovement>().fullDistance < other.GetComponent<CatMovement>().fullDistance) leader = other;
        }
        if (leader.GetComponent<CatMovement>().fullDistance > fullDistance) leader = null; 
    }

    private void Update()
    {
        if (currentPosition < track.positionCount)
        {
            if (leader!= null)
            {
                if (leader.GetComponent<CatMovement>().donePart - donePart > 0.01f)
                {
                    float leaderDonePart = leader.GetComponent<CatMovement>().donePart;
                    while (true)
                    {
                        if (leaderDonePart - donePart < 0.05f || donePart > 1 || currentPosition >= track.positionCount - 2) break;
                        donePart += Vector2.Distance(track.GetPosition(currentPosition - 1), track.GetPosition(currentPosition)) / fullDistance;
                        currentPosition++;
                    }
                    transform.position = track.GetPosition(currentPosition);
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, track.GetPosition(currentPosition), speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, track.GetPosition(currentPosition)) < 0.05f)
            {
                donePart += Vector2.Distance(track.GetPosition(currentPosition - 1), track.GetPosition(currentPosition)) / fullDistance;
                currentPosition++;
                if (currentPosition < track.positionCount)
                {
                    if (track.GetPosition(currentPosition).x >= transform.position.x) gameObject.GetComponent<SpriteRenderer>().flipX = false;
                    else gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
            }
        }
        else
        {
            manager.GetComponent<GameManager>().finished++;
            manager.GetComponent<GameManager>().RaceFinish();
            gameObject.GetComponent<Animator>().SetBool("Run", false);
            gameObject.GetComponent<CatMovement>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat"))
        {
            manager.GetComponent<GameManager>().CatsFight(gameObject, collision.gameObject);
            Destroy(gameObject);
        } else if (collision.CompareTag("Wall"))
        {
            manager.GetComponent<GameManager>().Crash(gameObject);
        }
    }
}
