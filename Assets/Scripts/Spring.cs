using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private SoftPoint currentPoint;
    public SoftPoint attachedPoint;

    
    public float currentSpringStrength = 10f;

    [SerializeField]
    private float restLength = 1f;
    
    public float damping = 0.1f;

    public bool isOneWaySpring = false;

    public bool setLengthToZero = false;




    private Rigidbody rb;
    private Rigidbody rbAttached;

    private void Start()
    {
        currentPoint = GetComponent<SoftPoint>();
        rb = GetComponent<Rigidbody>();
        rbAttached = attachedPoint.GetComponent<Rigidbody>();
        restLength = Vector3.Distance(transform.position, attachedPoint.transform.position);
        if (setLengthToZero)
        {
            restLength = 0f;
        }
    }

    private void FixedUpdate()
    {
        // If there is no attached point, do nothing
        if (attachedPoint == null)
            return;

        // Calculate the direction and distance between the two points
        Vector3 direction =
            attachedPoint.transform.position -
            transform.position;

        float distance = direction.magnitude;

        if (distance < 1e-6f)
            return;

        
        Vector3 directionNormalized =
            direction / distance;

        // Calculate the spring force based on Hooke's law
        float springMagnitude =
            (distance - restLength) *
            currentSpringStrength;

        // Calculate the relative velocity between the two points
        Vector3 relativeVelocity =
            rb.linearVelocity -
            rbAttached.linearVelocity;

        // Calculate the velocity along the spring direction
        float velocityAlongSpring =
            Vector3.Dot(
                relativeVelocity,
                directionNormalized);

        // Calculate the damping force based on the relative velocity
        float dampingMagnitude =
            -damping *
            velocityAlongSpring;

        // Calculate the total force to be applied to the current point
        Vector3 force =
            directionNormalized *
            (springMagnitude + dampingMagnitude);

        rb.AddForce(force);

        if (!isOneWaySpring)
        {
            rbAttached.AddForce(-force);
        }
    }

    /*
     * OLD CODE, NOT USED ANYMORE, BUT KEPT FOR REFERENCE
    private void Update()
    {
        if (attachedPoint != null)
        {
            // Calculte the length the point must move
            Vector3 direction = attachedPoint.transform.position - transform.position;
            float distance = Vector3.Distance(transform.position, attachedPoint.transform.position);

            // Calculate the spring force
            float springForce = (restLength - distance) * currentSpringStrength;

            // Get the difference between the velocity of the two points and its dot value
            Vector3 relativeVelocity = rb.linearVelocity - rbAttached.linearVelocity;
            float dotRelativeVelocity = Vector3.Dot(relativeVelocity, direction.normalized);

            // Calculate the damping force
            float dampingForce = dotRelativeVelocity * damping;

            // Get the total force, clamp it and apply it to both points
            float totalForce = springForce + dampingForce;

            rb.AddForce(direction.normalized * -totalForce);

            if (!isOneWaySpring)
                rbAttached.AddForce(direction.normalized * totalForce);
        }
    }*/

    public void SetSpringLength(float length)
    {
        restLength = length;
    }
}
