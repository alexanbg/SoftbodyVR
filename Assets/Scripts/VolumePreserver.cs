using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class VolumePreserver : MonoBehaviour
{
    [SerializeField]
    private List<SoftPoint> points = new List<SoftPoint>();

    [SerializeField]
    private float baseSpringStrength = 1f;
    [SerializeField]
    private float baseSpringDamping = 1f;

    [SerializeField]
    private float restVolume;

    [SerializeField]
    private float volumeStiffness = 10f;

    private List<Spring> springs = new List<Spring>();

    private void Start()
    {
        // Get all the springs in the children of this object   
        springs = new List<Spring>(GetComponentsInChildren<Spring>());
        // Set the spring strength and damping for all springs
        SetSpring(baseSpringStrength, baseSpringDamping);
        // Calculate the rest area of the triangle formed by the three points
        restVolume = CalculateVolume();
    }

    private void FixedUpdate()
    {
        // Calculate the current area of the triangle formed by the three points
        float currentVolume = CalculateVolume();
        // Calculate the difference between the current area and the rest area
        float volumeError = currentVolume - restVolume;

        // Calculate the gradient of the volume with respect to the positions of the points
        Vector3 x0 = points[0].transform.position;
        Vector3 x1 = points[1].transform.position;
        Vector3 x2 = points[2].transform.position;
        Vector3 x3 = points[3].transform.position;
        Vector3 grad0 = Vector3.Cross(x3 - x1, x2 - x1) / 6f;
        Vector3 grad1 = Vector3.Cross(x2 - x0, x3 - x0) / 6f;
        Vector3 grad2 = Vector3.Cross(x3 - x0, x1 - x0) / 6f;
        Vector3 grad3 = Vector3.Cross(x1 - x0, x2 - x0) / 6f;

        // Apply forces to the points based on the volume error and the gradients
        points[0].ApplyForce(-grad0 * volumeError * volumeStiffness);
        points[1].ApplyForce(-grad1 * volumeError * volumeStiffness);
        points[2].ApplyForce(-grad2 * volumeError * volumeStiffness);
        points[3].ApplyForce(-grad3 * volumeError * volumeStiffness);
    }

    private float CalculateVolume()
    {
        Vector3 p1 = points[0].transform.position;
        Vector3 p2 = points[1].transform.position;
        Vector3 p3 = points[2].transform.position;
        Vector3 p4 = points[3].transform.position;

        Vector3 u = p2 - p1;
        Vector3 v = p3 - p1;
        Vector3 w = p4 - p1;

        return Vector3.Dot(u, Vector3.Cross(v, w)) / 6f;
    }

    private void SetSpring(float strength, float damping)
    {
        foreach (Spring spring in springs)
        {
            spring.currentSpringStrength = strength;
            spring.damping = damping;
        }
    }
}
