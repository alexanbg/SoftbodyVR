using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private SoftPoint currentPoint;
    public SoftPoint attachedPoint;

    
    public float currentSpringStrength = 10f;


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
        /*
        LineRenderer line = this.AddComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, attachedPoint.transform.position);
        line.startWidth = 0.1f;
        */
    }
    private void Update()
    {
        if (attachedPoint != null)
        {
            // Calculte the length the point must move
            Vector3 direction = attachedPoint.transform.position - transform.position;
            float distance = direction.magnitude;

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
    }

    public void SetSpringLength(float length)
    {
        restLength = length;
    }
}
