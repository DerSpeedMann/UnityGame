using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{ 

    public GameObject drawPrefab;

    public float minDistance = 1; // the minimal distance between points of a line

    private List<GameObject> lines;
    private GameObject activeLine = null;

    private Vector2 lastPoint;
    private int lineCount = 0;

    private GameObject[] noDrawZones;

    // Start is called before the first frame update
    private void Start()
    {
        lines = new List<GameObject>();
        noDrawZones = GameObject.FindGameObjectsWithTag("NoDraw");
    }

    // checks if the current point is in a no Draw Zone 
    // returns true if not in no draw zone false otherwise
    private bool CanDrawAt(Vector2 point)
    {
        foreach (var zone in noDrawZones)
        {
            if (zone.GetComponent<PolygonCollider2D>().OverlapPoint(point))
            {
                return false;
            }
        }
        return true;
    }

    // add a line to the line List and increase line counter
    private void AddLine(GameObject line)
    {
        //Debug.Log("AddLine:" + lineCount);
        lines.Add(line);
        lineCount++;
    }

    // remove and destroy last line from line list
    public void RemoveLast()
    {
        if(lineCount > 0)
        {
            lineCount--;
            Destroy(lines[lineCount]);
            lines.RemoveAt(lineCount);
        }
    }

    // removes and destroys all lines from line list
    public void RemoveAll()
    {
        foreach (var line in lines)
        {
            Destroy(line);
            lineCount = 0;
        }
        lines.Clear();
    }

    // creates new line or adds point to active line 
    // if point is in a noDrawZone no point is added and stopDrawing is called
    public void Draw(Vector3 startPoint)
    {
        var worldPoint = Camera.main.ScreenToWorldPoint(startPoint);
        if (CanDrawAt(worldPoint))
        {
            if (activeLine == null)
            {
                CreateNewLine();
            }

            AddPoint(activeLine, worldPoint);
        }
        else
        {
            StopDrawing();
        }
       
    }

    // creates collider for active line if possible and sets active line to null
    // if collider cant be created line is destoyed
    public void StopDrawing()
    {
        if (activeLine != null)
        {

            if (!CreateCollider(activeLine))
            { 
               Destroy(activeLine);
            }

            activeLine = null;
        }
    }

    // initializes new active line
    private void CreateNewLine()
    {
        activeLine = Instantiate(drawPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        var lineRenderer = activeLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    // add point to given line if distance between last and new point > minDistance or line has no points
    private void AddPoint(GameObject line, Vector3 newPoint)
    {
        var distance = Vector2.Distance(newPoint, lastPoint);
        var lineRenderer = line.GetComponent<LineRenderer>();

        if ( distance > minDistance || lineRenderer.positionCount <= 0)
        {

            if(lineRenderer.positionCount > 0 && IntersectsNoDraw(lastPoint, newPoint))
            {
                StopDrawing();
                return;
            }
            
            newPoint.z = 0;
            lineRenderer.SetPosition(lineRenderer.positionCount++, newPoint);
            lastPoint = newPoint;
        }
    }

    // creates collider for the given line
    // returnes true if line has more than 1 point false otherwise
    private bool CreateCollider(GameObject line)
    {
        var lineRenderer = line.GetComponent<LineRenderer>();
        var collider = line.GetComponent<PolygonCollider2D>();

        
        if (lineRenderer.positionCount > 1)
        {
            // get 3d mesh of drawn line
            var mesh = new Mesh();
            lineRenderer.BakeMesh(mesh);
            SetPolygonCollider2DFromMesh(collider, mesh);
            AddLine(activeLine);
            return true;
        }
        return false;
    }

    // calculates if a line (point1 to point2) is intersecting with any noDrawZones in the level
    // returnes true if line is intersecting false otherwise
    private bool IntersectsNoDraw(Vector2 p1, Vector2 p2)
    {
        foreach(var zone in noDrawZones)
        {
            var collider = zone.GetComponent<PolygonCollider2D>();
            if(collider.points.Length >= 2)
            {

                var prevPoint = collider.transform.TransformPoint(collider.points[0]);

                for (int i = 1; i <= collider.points.Length; i++)
                {
                    Vector2 point;

                    if(i == collider.points.Length)
                    {
                       point = collider.transform.TransformPoint(collider.points[0]);
                    }
                    else
                    {
                       point = collider.transform.TransformPoint(collider.points[i]);
                    }
                    
                    if (FasterLineSegmentIntersection(prevPoint, point, p1, p2))
                    {
                        return true;
                    }
                    prevPoint = point;
                }
            }
        }
        return false;
    }

    // calculates line intersection
    static bool FasterLineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {

        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = a.y * b.x - a.x * b.y;

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {

            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;

                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect || betaDenominator > 0) {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            } else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        return doIntersect;
    }

    // Creates 2DPolygon collider from Mesh
    private void SetPolygonCollider2DFromMesh(PolygonCollider2D polygonCollider, Mesh mesh)
    {

        // Get triangles and vertices from mesh
        int[] triangles = mesh.triangles;
        Vector3[] vertices = mesh.vertices;

        // Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
        Dictionary<string, KeyValuePair<int, int>> edges = new Dictionary<string, KeyValuePair<int, int>>();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            for (int e = 0; e < 3; e++)
            {
                int vert1 = triangles[i + e];
                int vert2 = triangles[i + e + 1 > i + 2 ? i : i + e + 1];
                string edge = Mathf.Min(vert1, vert2) + ":" + Mathf.Max(vert1, vert2);
                if (edges.ContainsKey(edge))
                {
                    edges.Remove(edge);
                }
                else
                {
                    edges.Add(edge, new KeyValuePair<int, int>(vert1, vert2));
                }
            }
        }

        // Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
        Dictionary<int, int> lookup = new Dictionary<int, int>();
        foreach (KeyValuePair<int, int> edge in edges.Values)
        {
            if (lookup.ContainsKey(edge.Key) == false)
            {
                lookup.Add(edge.Key, edge.Value);
            }
        }

        // Loop through edge vertices in order
        polygonCollider.pathCount = 0;
        int startVert = 0;
        int nextVert = startVert;
        int highestVert = startVert;
        List<Vector2> colliderPath = new List<Vector2>();
        while (true)
        {

            // Add vertex to collider path
            colliderPath.Add(vertices[nextVert]);

            // Get next vertex
            nextVert = lookup[nextVert];

            // Store highest vertex (to know what shape to move to next)
            if (nextVert > highestVert)
            {
                highestVert = nextVert;
            }

            // Shape complete
            if (nextVert == startVert)
            {

                // Add path to polygon collider
                polygonCollider.pathCount++;
                polygonCollider.SetPath(polygonCollider.pathCount - 1, colliderPath.ToArray());
                colliderPath.Clear();

                // Go to next shape if one exists
                if (lookup.ContainsKey(highestVert + 1))
                {

                    // Set starting and next vertices
                    startVert = highestVert + 1;
                    nextVert = startVert;

                    // Continue to next loop
                    continue;
                }

                // No more verts
                break;
            }
        }
    }
}
