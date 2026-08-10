using System.Drawing;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private SoftPoint currentPoint;
    public SoftPoint attachedPoint;

    
    public float currentSpringStrength = 10f;
    
    private float restLength = 1f;
    
    public float damping = 0.1f;

    public bool isOneWaySpring = false;


    private Rigidbody rb;
    private Rigidbody rbAttached;

    private void Start()
    {
        currentPoint = GetComponent<SoftPoint>();
        rb = GetComponent<Rigidbody>();
        rbAttached = attachedPoint.GetComponent<Rigidbody>();
        restLength = Vector2.Distance(transform.position, attachedPoint.transform.position);
    }

    private void Update()
    {
        if (attachedPoint != null)
        {
            // Calculte the length the point must move
            Vector2 direction = attachedPoint.transform.position - transform.position;
            float distance = direction.magnitude;

            // Calculate the spring force
            float springForce = (restLength - distance) * currentSpringStrength;

            // Get the difference between the velocity of the two points and its dot value
            Vector2 relativeVelocity = rb.linearVelocity - rbAttached.linearVelocity;
            float dotRelativeVelocity = Vector2.Dot(relativeVelocity, direction.normalized);

            // Calculate the damping force
            float dampingForce = dotRelativeVelocity * damping;

            // Get the total force, clamp it and apply it to both points
            float totalForce = springForce + dampingForce;

            rb.AddForce(direction.normalized * -totalForce);

            if (!isOneWaySpring)
                rbAttached.AddForce(direction.normalized * totalForce);
        }
    }
}
