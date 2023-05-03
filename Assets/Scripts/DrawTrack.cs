using UnityEngine;

public class DrawTrack : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public CircleCollider2D finish;
    public float trackDistance;
    public bool prepared;
    private GameObject manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager");
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            if (lineRenderer.positionCount > 1) trackDistance += Vector2.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), currentPosition);           
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(point, Vector2.zero);
            if (hit.collider == null || hit.transform.GetComponent<CircleCollider2D>() != finish)
            {
                lineRenderer.positionCount = 0;
                trackDistance = 0;
            }
            else
            {
                prepared = true;
                manager.GetComponent<GameManager>().prepared++;
                manager.GetComponent<GameManager>().RaceStart();
            }
            gameObject.GetComponent<DrawTrack>().enabled = false;
        }
    }
}
