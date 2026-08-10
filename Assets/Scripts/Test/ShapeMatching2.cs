using UnityEngine;

public class ShapeMatching2 : MonoBehaviour
{
    [SerializeField]
    private Transform[] currentPoints;

    [SerializeField]
    private Transform[] targetPoints;

    private Vector3[] restOffsets;
    private Quaternion baseRotation;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Compute rest center of mass

        //Store rest offsets q[i]
        restOffsets = new Vector3[currentPoints.Length];

        // Store the ORIGINAL local offsets
        for (int i = 0; i < currentPoints.Length; i++)
        {
            restOffsets[i] = currentPoints[i].position - transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
