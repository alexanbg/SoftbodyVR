using System;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class VolumePreserver : MonoBehaviour
{
    [SerializeField]
    private List<SoftPoint> points = new List<SoftPoint>();

    [SerializeField]
    private float baseSpringStrength = 1f;
    [SerializeField]
    private float springDamping = 0.1f;

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

    private void Update()
    {
        // Calculate the current area of the triangle
        float currentVolume = CalculateVolume();
        // Check the error between the current area and the rest area, you can use this to apply forces to the points to maintain the triangle's shape
        float volumeError = currentVolume - restVolume;

        //Vector3 currentCrossProduct = Vector3.Cross(points[1].transform.position - points[0].transform.position, points[2].transform.position - points[0].transform.position);

        // Find the direction to push the points to maintain the triangle's shape
        Vector3 p1 = Vector3.Cross(
            (points[1].transform.position - points[3].transform.position),
            (points[2].transform.position - points[3].transform.position)) / 6f;
        Vector3 p2 = Vector3.Cross(
            (points[2].transform.position - points[0].transform.position),
            (points[3].transform.position - points[0].transform.position)) / 6f;
        Vector3 p3 = Vector3.Cross(
            (points[3].transform.position - points[0].transform.position),
            (points[1].transform.position - points[0].transform.position)) / 6f;
        Vector3 p4 = Vector3.Cross(
            (points[1].transform.position - points[0].transform.position),
            (points[2].transform.position - points[0].transform.position)) / 6f;
        // Apply forces to the points based on the area error and the direction
        points[0].ApplyForce(p1 * volumeError * volumeStiffness);
        points[1].ApplyForce(p2 * volumeError * volumeStiffness);
        points[2].ApplyForce(p3 * volumeError * volumeStiffness);

        /*
        if(currentVolume < 0)
        {
            SetSpringStrength(0);
        }
        else
        {
            SetSpringStrength(baseSpringStrength);
        }*/

        Debug.Log($"Current Area: {currentVolume}, Rest Area: {restVolume}, Area Error: {volumeError}");
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
