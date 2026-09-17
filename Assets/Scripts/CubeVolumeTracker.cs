using System.Collections.Generic;
using UnityEngine;

public class CubeVolumeTracker : MonoBehaviour
{
    [SerializeField]
    private List<VolumePreserver> tetrahedrons =
        new List<VolumePreserver>();

    [Header("Volume")]
    [SerializeField]
    private float restVolume;

    [SerializeField]
    private float currentVolume;

    [SerializeField]
    private float maxVolumeError;

    [SerializeField]
    private float maxVolumeErrorPercentage;

    [Header("Restoration")]
    [SerializeField]
    private float restorationTolerance = 0.05f;

    [SerializeField]
    private float currentRestorationTime;

    [SerializeField]
    private float maxRestorationTime;

    [SerializeField]
    private float averageRestorationTime;

    private float totalRestorationTime;
    private int restorationCount;

    private bool isRestoring;
    private float restorationStartTime;

    private void Start()
    {
        tetrahedrons = new List<VolumePreserver>(
            GetComponentsInChildren<VolumePreserver>()
        );

        restVolume = CalculateTotalVolume();

        currentVolume = restVolume;

        maxVolumeError = 0f;
        maxVolumeErrorPercentage = 0f;

        currentRestorationTime = 0f;
        maxRestorationTime = 0f;
        averageRestorationTime = 0f;

        totalRestorationTime = 0f;
        restorationCount = 0;

        isRestoring = false;
    }

    private void FixedUpdate()
    {
        currentVolume = CalculateTotalVolume();

        float volumeError =
            currentVolume - restVolume;

        float absoluteVolumeError =
            Mathf.Abs(volumeError);

        // Track maximum volume error
        maxVolumeError = Mathf.Max(
            maxVolumeError,
            absoluteVolumeError
        );

        // Calculate percentage error
        float percentageError = 0f;

        if (Mathf.Abs(restVolume) > Mathf.Epsilon)
        {
            percentageError =
                absoluteVolumeError /
                Mathf.Abs(restVolume) *
                100f;

            maxVolumeErrorPercentage = Mathf.Max(
                maxVolumeErrorPercentage,
                percentageError
            );
        }

        // Convert tolerance from percentage to decimal
        float tolerance =
            Mathf.Abs(restVolume) *
            restorationTolerance;

        // Cube is sufficiently deformed
        if (!isRestoring &&
            absoluteVolumeError > tolerance)
        {
            isRestoring = true;

            restorationStartTime =
                Time.time;

            currentRestorationTime = 0f;
        }

        // Cube has returned to its rest volume
        if (isRestoring &&
            absoluteVolumeError <= tolerance)
        {
            currentRestorationTime =
                Time.time -
                restorationStartTime;

            isRestoring = false;

            // Store restoration statistics
            restorationCount++;

            totalRestorationTime +=
                currentRestorationTime;

            maxRestorationTime = Mathf.Max(
                maxRestorationTime,
                currentRestorationTime
            );

            averageRestorationTime =
                totalRestorationTime /
                restorationCount;

            Debug.Log(
                $"Cube restored after " +
                $"{currentRestorationTime:F3} seconds."
            );
        }
    }

    private float CalculateTotalVolume()
    {
        float totalVolume = 0f;

        foreach (VolumePreserver tetrahedron in tetrahedrons)
        {
            totalVolume +=
                tetrahedron.CalculateVolume();
        }

        return totalVolume;
    }

    private void OnDisable()
    {
        Debug.Log(
            $"Maximum Volume Error: " +
            $"{maxVolumeError:F4}"
        );

        Debug.Log(
            $"Maximum Volume Error: " +
            $"{maxVolumeErrorPercentage:F2}%"
        );

        Debug.Log(
            $"Maximum Restoration Time: " +
            $"{maxRestorationTime:F3} seconds"
        );

        Debug.Log(
            $"Average Restoration Time: " +
            $"{averageRestorationTime:F3} seconds"
        );

        Debug.Log(
            $"Number of Restorations: " +
            $"{restorationCount}"
        );
    }
}
