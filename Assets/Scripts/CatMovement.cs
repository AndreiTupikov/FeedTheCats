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
        gameObject.GetComponent<Animator>().SetBool("Run", true);
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
                    speedCorrection = 2f;
                    break;
                }
            }
            transform.position = Vector2.Lerp(transform.position, track.GetPosition(currentPosition), Time.deltaTime * speed * speedCorrection);
            speedCorrection = 1;
            if (Vector2.Distance(transform.position, track.GetPosition(currentPosition)) < 0.1f)
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
        }
    }
}
