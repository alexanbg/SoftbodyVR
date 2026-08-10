using UnityEngine;

public class SoftPoint : MonoBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }
}
