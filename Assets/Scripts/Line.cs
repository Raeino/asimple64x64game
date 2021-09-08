using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lr;
    public EdgeCollider2D ec;

    [HideInInspector] public List<Vector2> points = new List<Vector2>();
    [HideInInspector] public int pointsCount = 0;

    private float pointsMinDistance = 0.05f;
    private float pointColliderRadius;

    [HideInInspector] public CircleCollider2D lastPointCollider;

    public void AddPoint(Vector2 newPoint) {
        if (pointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
            return;

        // Edge collider
        if (pointsCount > 1)
            ec.points = points.ToArray();

        points.Add(newPoint);
        pointsCount++;
        
        // Line renderer
        lr.positionCount = pointsCount;
        lr.SetPosition(pointsCount - 1, newPoint);



        // Single point
        if (lastPointCollider)
            Destroy(lastPointCollider);
        lastPointCollider = gameObject.AddComponent<CircleCollider2D>();
        lastPointCollider.offset = newPoint;
        lastPointCollider.radius = pointColliderRadius;
        lastPointCollider.isTrigger = true;

    }

    public Vector2 GetLastPoint() {
        return lr.GetPosition(pointsCount - 1);
    }

    public void SetLineColor(Gradient gradient) {
        lr.colorGradient = gradient;
    }

    public void SetPointsMinDistance(float distance) {
        pointsMinDistance = distance;
    }

    public void SetLineWidth(float width) {
        lr.startWidth = width;
        lr.endWidth = width;

        pointColliderRadius = width / 2f;
        ec.edgeRadius = pointColliderRadius;
    }
}
