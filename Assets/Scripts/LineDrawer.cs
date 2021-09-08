using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public LayerMask cantDrawOver;

    [Space(30f)]
    public Gradient lineColor;
    public float linePointsMinDistance;
    public float lineWidth;

    Line currentLine;
    Camera cam;

    private void Start() {
        cam = Camera.main;
        Application.targetFrameRate = 60;
    }

    private void FixedUpdate() {
        if (Input.GetMouseButtonDown(0))
            StartDraw();
        if (currentLine != null)
            Draw();
        if (Input.GetMouseButtonUp(0))
            EndDraw();
    }

    private void StartDraw() {
        currentLine = Instantiate(linePrefab, transform).GetComponent<Line>();
        currentLine.SetLineColor(lineColor);
        currentLine.SetPointsMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);
    }

    private void Draw() {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.CircleCast(mousePos, 0.05f, Vector2.zero, 0f, cantDrawOver);

        if (hit)
        Debug.Log(hit.collider + "  |||  " + currentLine.lastPointCollider);


        if (hit && hit.collider != currentLine.lastPointCollider && currentLine.pointsCount > 2)
            EndDraw();
        else
            currentLine.AddPoint(mousePos);

    }

    private void EndDraw() {
        if (currentLine != null) {
            foreach (Transform line in transform)
                Destroy(line.gameObject);
        }
    }
}
