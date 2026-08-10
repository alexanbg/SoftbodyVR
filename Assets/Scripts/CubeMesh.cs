using System.Collections.Generic;
using UnityEngine;

public class CubeMesh : MonoBehaviour
{
    [SerializeField]
    private List<SoftPoint> points = new List<SoftPoint>();

    private Mesh mesh;
    private Vector3[] vertices;
    int[] triangles;


    void Start()
    {
        mesh = new Mesh();
        vertices = new Vector3[points.Count];
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();
    }
    void Update()
    {
        UpdateMesh();
    }
    private void CreateMesh()
    {
        

        triangles = new int[]
        {
            0,2,1,
            4,2,0,
            0,1,4,
            1,2,4,
            1,2,3,
            3,7,1,
            7,3,2,
            1,7,2,
            1,7,5,
            7,4,5,
            1,5,4,
            1,4,7,
            6,7,2,
            6,2,4,
            7,6,4,
            2,4,7

        };

    }



    private void UpdateMesh()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = points[i].transform.position - transform.position;
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
