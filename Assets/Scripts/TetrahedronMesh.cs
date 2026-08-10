using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TetrahedronMesh : MonoBehaviour
{
    [SerializeField]
    private List<SoftPoint> points = new List<SoftPoint>();

    private Mesh mesh;
    private Vector3[] vertices;
    int[] triangles;


    


    
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateMesh();

        
        //Set the parameters
        
    }
    void Update()
    {
        UpdateMesh();
    }
    private void CreateMesh()
    {
        vertices = points.ConvertAll(point => point.transform.position - transform.position).ToArray();


        





        triangles = new int[]
        {
            2,1,0,
            1,2,3,
            2,0,3,
            0,1,3

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
