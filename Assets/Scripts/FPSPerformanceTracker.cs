using UnityEngine;

public class FPSPerformanceTracker : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField]
    private float testDuration = 30f;

    [SerializeField]
    private bool startAutomatically = true;

    [Header("Results")]
    [SerializeField]
    private float averageFPS;

    [SerializeField]
    private float minimumFPS;

    [SerializeField]
    private float maximumFPS;

    [SerializeField]
    private int frameCount;

    private float elapsedTime;
    private float totalFPS;

    private bool isTracking;

    private void Start()
    {
        if (startAutomatically)
        {
            StartTracking();
        }
    }

    private void Update()
    {
        if (!isTracking)
            return;

        float currentFPS = 1f / Time.unscaledDeltaTime;

        totalFPS += currentFPS;
        frameCount++;

        minimumFPS = Mathf.Min(
            minimumFPS,
            currentFPS
        );

        maximumFPS = Mathf.Max(
            maximumFPS,
            currentFPS
        );

        elapsedTime += Time.unscaledDeltaTime;

        if (elapsedTime >= testDuration)
        {
            StopTracking();
        }
    }

    public void StartTracking()
    {
        elapsedTime = 0f;
        totalFPS = 0f;
        frameCount = 0;

        averageFPS = 0f;
        minimumFPS = float.MaxValue;
        maximumFPS = 0f;

        isTracking = true;

        Debug.Log("FPS performance test started.");
    }

    public void StopTracking()
    {
        isTracking = false;

        if (frameCount > 0)
        {
            averageFPS = totalFPS / frameCount;
        }

        Debug.Log(
            $"FPS Performance Test Finished\n" +
            $"Average FPS: {averageFPS:F2}\n" +
            $"Minimum FPS: {minimumFPS:F2}\n" +
            $"Maximum FPS: {maximumFPS:F2}\n" +
            $"Frames: {frameCount}\n" +
            $"Duration: {elapsedTime:F2}s"
        );
    }
}
