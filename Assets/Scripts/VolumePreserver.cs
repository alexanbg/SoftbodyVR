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
    private float restVolume;

    [SerializeField]
    private float volumeStiffness = 10f;

    private List<Spring> springs = new List<Spring>();

    private void Start()
    {

        //Get all springs in this object
        springs = new List<Spring>(GetComponentsInChildren<Spring>());

        SetSpringStrength(baseSpringStrength);

        //Store the initial area of the triangle
        restVolume = CalculateVolume();
    }

    private void FixedUpdate()
    {
        // Calculate the current area of the triangle
        float currentVolume = CalculateVolume();
        // Check the error between the current area and the rest area, you can use this to apply forces to the points to maintain the triangle's shape
        float volumeError = currentVolume - restVolume;

        
            Vector3 x0 = points[0].transform.position;
            Vector3 x1 = points[1].transform.position;
            Vector3 x2 = points[2].transform.position;
            Vector3 x3 = points[3].transform.position;

            Vector3 grad0 =
        -(
            Vector3.Cross(x2 - x0, x3 - x0) +
            Vector3.Cross(x3 - x0, x1 - x0) +
            Vector3.Cross(x1 - x0, x2 - x0)
         ) / 6f;

            Vector3 grad1 =
                Vector3.Cross(x2 - x0, x3 - x0) / 6f;

            Vector3 grad2 =
                Vector3.Cross(x3 - x0, x1 - x0) / 6f;

            Vector3 grad3 =
                Vector3.Cross(x1 - x0, x2 - x0) / 6f;

            points[0].ApplyForce(-grad0 * volumeError * volumeStiffness);
            points[1].ApplyForce(-grad1 * volumeError * volumeStiffness);
            points[2].ApplyForce(-grad2 * volumeError * volumeStiffness);
            points[3].ApplyForce(-grad3 * volumeError * volumeStiffness);

            
        
        Debug.Log($"Tetrahedron with points {string.Join(' ', points.Select(x => x.gameObject.name))} - Current Area: {currentVolume}, Rest Area: {restVolume}, Area Error: {volumeError}");

        /*
        if(currentVolume < 0)
        {
            SetSpringStrength(0);
        }
        else
        {
            SetSpringStrength(baseSpringStrength);
        }*/


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

    private void SetSpringStrength(float strength)
    {
        foreach (Spring spring in springs)
        {
            spring.currentSpringStrength = strength;
            
        }
    }
}
