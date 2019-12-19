using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject drawPrefab;

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

    private bool CanDrawAt(Vector2 point)
    {
        foreach (var zone in noDrawZones)
        {
            if (zone.GetComponent<BoxCollider2D>().OverlapPoint(point))
            {
                return false;
            }
        }
        return true;
    }
    private void AddLine(GameObject line)
    {
        //Debug.Log("AddLine:" + lineCount);
        lines.Add(line);
        lineCount++;
    }
    public void RemoveLast()
    {
        //Debug.Log("RemoveLast:" + (lineCount-1));

        if(lineCount > 0)
        {
            lineCount--;
            Destroy(lines[lineCount]);
            lines.RemoveAt(lineCount);
        }
    }

    public void RemoveAll()
    {
        //Debug.Log("RemovedAll:" + lineCount);

        foreach (var line in lines)
        {
            Destroy(line);
            lineCount = 0;
        }
        lines.Clear();
    }
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

    private void CreateNewLine()
    {
        activeLine = Instantiate(drawPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        var lineRenderer = activeLine.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }

    private void AddPoint(GameObject line, Vector3 newPoint)
    {
        var absX = Math.Abs(newPoint.x - lastPoint.x);
        var absY = Math.Abs(newPoint.y - lastPoint.y);

        if ( absX > 1 || absY > 1)
        {
            var lineRenderer = line.GetComponent<LineRenderer>();
            newPoint.z = 0;
            lineRenderer.SetPosition(lineRenderer.positionCount++, newPoint);
            lastPoint = newPoint;
        }
    }
    private bool CreateCollider(GameObject line)
    {
        var lineRenderer = line.GetComponent<LineRenderer>();
        var collider = line.GetComponent<PolygonCollider2D>();

        // get 3d mesh of drawn line
        if (lineRenderer.positionCount > 1)
        {
            var mesh = new Mesh();
            lineRenderer.BakeMesh(mesh);
            SetPolygonCollider2DFromMesh(collider, mesh);
            AddLine(activeLine);
            return true;
        }
        return false;
    }

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
