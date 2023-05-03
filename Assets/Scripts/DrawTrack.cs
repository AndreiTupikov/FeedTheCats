using UnityEngine;

public class DrawTrack : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public SphereCollider finish;
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit) || hit.transform.GetComponent<SphereCollider>() != finish)
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
