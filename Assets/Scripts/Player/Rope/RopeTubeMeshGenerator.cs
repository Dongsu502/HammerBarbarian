using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RopeTubeMeshGenerator : MonoBehaviour
{
    [Header("Tube Settings")]
    public float radius = 0.05f;
    public int circleResolution = 8;

    [Header("Data Source")]
    public List<Vector3> ropePoints = new List<Vector3>(); 

    private Mesh ropeMesh;
    public Renderer ropeRenderer;

    void Awake()
    {
        ropeMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = ropeMesh;
        ropeRenderer = GetComponent<Renderer>();
    }

    void LateUpdate()
    {
        if (ropePoints == null || ropePoints.Count < 2) return;
        GenerateTube(ropePoints);
    }

    public void SetPoints(Vector3[] points)
    {
        ropePoints.Clear();
        ropePoints.AddRange(points);
    }

    void GenerateTube(List<Vector3> path)
    {
        int vertCount = path.Count * circleResolution;
        Vector3[] verts = new Vector3[vertCount];
        Vector2[] uvs = new Vector2[vertCount];
        int[] tris = new int[(path.Count - 1) * circleResolution * 6];

        int v = 0, t = 0;

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 forward = (i < path.Count - 1)
                ? (path[i + 1] - path[i]).normalized
                : (path[i] - path[i - 1]).normalized;

            if (forward.sqrMagnitude < 0.0001f)
                forward = Vector3.forward;

            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);

            for (int j = 0; j < circleResolution; j++)
            {
                float angle = j / (float)circleResolution * Mathf.PI * 2f;
                Vector3 offset = rotation * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * radius;

                verts[v] = path[i] + offset;
                uvs[v] = new Vector2((float)j / circleResolution, (float)i / path.Count);
                v++;
            }
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            for (int j = 0; j < circleResolution; j++)
            {
                int current = i * circleResolution + j;
                int next = current + circleResolution;
                int nextJ = (j + 1) % circleResolution;

                tris[t++] = current;
                tris[t++] = next;
                tris[t++] = i * circleResolution + nextJ;

                tris[t++] = next;
                tris[t++] = (i + 1) * circleResolution + nextJ;
                tris[t++] = i * circleResolution + nextJ;
            }
        }

        ropeMesh.Clear();
        ropeMesh.vertices = verts;
        ropeMesh.triangles = tris;
        ropeMesh.uv = uvs;
        ropeMesh.RecalculateNormals();
        ropeMesh.RecalculateTangents();
    }
}
