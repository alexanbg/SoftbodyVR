
using System.Collections.Generic;
using UnityEngine;

public class ShapeMatching : MonoBehaviour
{
    [SerializeField]
    private Transform[] currentPoints;

    [SerializeField]
    private Transform[] targetPoints;

    private Vector3[] restOffsets;
    private Quaternion baseRotation;

    void Start()
    {
        restOffsets = new Vector3[currentPoints.Length];

        // Store the ORIGINAL local offsets
        for (int i = 0; i < currentPoints.Length; i++)
        {
            restOffsets[i] = currentPoints[i].position - transform.position;
        }

        baseRotation = GetAverageAngle();
    }

    void Update()
    {
        Quaternion currentRotation = GetAverageAngle();

        // Rotation from rest orientation to current orientation
        Quaternion deltaRotation =
            currentRotation * Quaternion.Inverse(baseRotation);

        Debug.Log($"Delta Rotation: {deltaRotation.eulerAngles}");

        for (int i = 0; i < targetPoints.Length; i++)
        {
            targetPoints[i].position =
                transform.position +
                deltaRotation * restOffsets[i];
        }
    }

    private Quaternion GetAverageAngle()
    {
        Vector3 averageForward = Vector3.zero;
        Vector3 averageUp = Vector3.zero;

        for (int i = 0; i < currentPoints.Length; i++)
        {
            Vector3 dir =
                (currentPoints[i].position - transform.position).normalized;

            Quaternion rot =
                Quaternion.LookRotation(dir, Vector3.forward);

            averageForward += rot * Vector3.forward;
            averageUp += rot * Vector3.up;
        }

        averageForward.Normalize();
        averageUp.Normalize();

        return Quaternion.LookRotation(averageForward, averageUp);
    }
}
