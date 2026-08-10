using UnityEngine;
using static UnityEngine.ParticleSystem;

public class ShapeMatching3 : MonoBehaviour
{
    [SerializeField]
    private Transform[] currentPoints;

    [SerializeField]
    private Transform[] targetPoints;

    [SerializeField]
    private float springStrength = 10f;
    [SerializeField]
    private float damping = 0.1f;

    private Vector3[] restOffset;
    private Vector3 curretCenterOfMass;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restOffset = new Vector3[currentPoints.Length];
        for (int i = 0; i < currentPoints.Length; i++)
        {
            restOffset[i] = currentPoints[i].position - transform.position;
            targetPoints[i].position = currentPoints[i].position;

            Spring currentSpring = currentPoints[i].gameObject.AddComponent<Spring>();

            currentSpring.attachedPoint = targetPoints[i].GetComponent<SoftPoint>();
            currentSpring.isOneWaySpring = true;
            currentSpring.currentSpringStrength = springStrength;
            currentSpring.damping = damping;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Matrix3x3 A = Matrix3x3.Zero;
        curretCenterOfMass = ComputeCenterOfMass(currentPoints);

        for (int i = 0; i < currentPoints.Length; i++)
        {
            Vector3 p = currentPoints[i].position - curretCenterOfMass;
            Vector3 q = restOffset[i];

            A += Matrix3x3.OuterProduct(p, q);
        }

        Matrix3x3 R = PolarDecomposition(A);
        
        Quaternion rotation = MatrixToQuaternion(R);

        for (int i = 0; i < currentPoints.Length; i++)
        {
            Vector3 goal =
                curretCenterOfMass +
                rotation * restOffset[i];

            targetPoints[i].position = goal;
        }
    }

    private Vector3 ComputeCenterOfMass(Transform[] points)
    {
        Vector3 centerOfMass = Vector3.zero;
        foreach (var point in points)
        {
            centerOfMass += point.position;
        }
        return centerOfMass / points.Length;
    }

    public static Matrix3x3 PolarDecomposition(Matrix3x3 A)
    {
        Matrix3x3 R = A;

        const int iterations = 10;

        for (int i = 0; i < iterations; i++)
        {
            Matrix3x3 invT = R.Inverse().Transpose();

            R = (R + invT) * 0.5f;
        }

        return R;
    }

    public static Quaternion MatrixToQuaternion(Matrix3x3 m)
    {
        float trace = m.m00 + m.m11 + m.m22;

        if (trace > 0)
        {
            float s = Mathf.Sqrt(trace + 1.0f) * 2.0f;

            return new Quaternion(
                (m.m21 - m.m12) / s,
                (m.m02 - m.m20) / s,
                (m.m10 - m.m01) / s,
                0.25f * s);
        }

        if (m.m00 > m.m11 && m.m00 > m.m22)
        {
            float s = Mathf.Sqrt(1.0f + m.m00 - m.m11 - m.m22) * 2;

            return new Quaternion(
                0.25f * s,
                (m.m01 + m.m10) / s,
                (m.m02 + m.m20) / s,
                (m.m21 - m.m12) / s);
        }

        if (m.m11 > m.m22)
        {
            float s = Mathf.Sqrt(1.0f + m.m11 - m.m00 - m.m22) * 2;

            return new Quaternion(
                (m.m01 + m.m10) / s,
                0.25f * s,
                (m.m12 + m.m21) / s,
                (m.m02 - m.m20) / s);
        }

        {
            float s = Mathf.Sqrt(1.0f + m.m22 - m.m00 - m.m11) * 2;

            return new Quaternion(
                (m.m02 + m.m20) / s,
                (m.m12 + m.m21) / s,
                0.25f * s,
                (m.m10 - m.m01) / s);
        }
    }
}
