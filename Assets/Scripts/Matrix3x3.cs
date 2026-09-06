using UnityEngine;

public struct Matrix3x3
{
    public float m00, m01, m02;
    public float m10, m11, m12;
    public float m20, m21, m22;

    public Vector3 Column0 => new Vector3(m00, m10, m20);
    public Vector3 Column1 => new Vector3(m01, m11, m21);
    public Vector3 Column2 => new Vector3(m02, m12, m22);

    public Vector3 Row0 => new Vector3(m00, m01, m02);
    public Vector3 Row1 => new Vector3(m10, m11, m12);
    public Vector3 Row2 => new Vector3(m20, m21, m22);


    public Matrix3x3(
        float m00, float m01, float m02,
        float m10, float m11, float m12,
        float m20, float m21, float m22)
    {
        this.m00 = m00; this.m01 = m01; this.m02 = m02;
        this.m10 = m10; this.m11 = m11; this.m12 = m12;
        this.m20 = m20; this.m21 = m21; this.m22 = m22;
    }
    public static Matrix3x3 Identity =>
        new Matrix3x3(1f, 0f, 0f,
                      0f, 1f, 0f,
                      0f, 0f, 1f);
    public static Matrix3x3 Zero =>
        new Matrix3x3(0f, 0f, 0f,
                      0f, 0f, 0f,
                      0f, 0f, 0f);

    public static Matrix3x3 operator +(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(
            a.m00 + b.m00, a.m01 + b.m01, a.m02 + b.m02,
            a.m10 + b.m10, a.m11 + b.m11, a.m12 + b.m12,
            a.m20 + b.m20, a.m21 + b.m21, a.m22 + b.m22);
    }
    public static Matrix3x3 operator *(Matrix3x3 a, float s)
    {
        return new Matrix3x3(
            a.m00 * s, a.m01 * s, a.m02 * s,
            a.m10 * s, a.m11 * s, a.m12 * s,
            a.m20 * s, a.m21 * s, a.m22 * s);
    }
    public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
    {
        return new Matrix3x3(

            a.m00 * b.m00 + a.m01 * b.m10 + a.m02 * b.m20,
            a.m00 * b.m01 + a.m01 * b.m11 + a.m02 * b.m21,
            a.m00 * b.m02 + a.m01 * b.m12 + a.m02 * b.m22,

            a.m10 * b.m00 + a.m11 * b.m10 + a.m12 * b.m20,
            a.m10 * b.m01 + a.m11 * b.m11 + a.m12 * b.m21,
            a.m10 * b.m02 + a.m11 * b.m12 + a.m12 * b.m22,

            a.m20 * b.m00 + a.m21 * b.m10 + a.m22 * b.m20,
            a.m20 * b.m01 + a.m21 * b.m11 + a.m22 * b.m21,
            a.m20 * b.m02 + a.m21 * b.m12 + a.m22 * b.m22
        );
    }
    public static Vector3 operator *(Matrix3x3 m, Vector3 v)
    {
        return new Vector3(
            m.m00 * v.x + m.m01 * v.y + m.m02 * v.z,
            m.m10 * v.x + m.m11 * v.y + m.m12 * v.z,
            m.m20 * v.x + m.m21 * v.y + m.m22 * v.z
        );
    }
    public Matrix3x3 Transpose()
    {
        return new Matrix3x3(
            m00, m10, m20,
            m01, m11, m21,
            m02, m12, m22);
    }
    public float Determinant()
    {
        return
            m00 * (m11 * m22 - m12 * m21)
          - m01 * (m10 * m22 - m12 * m20)
          + m02 * (m10 * m21 - m11 * m20);
    }
    public Matrix3x3 Inverse()
    {
        float det = Determinant();

        if (Mathf.Abs(det) < 1e-8f)
            return Identity;

        float inv = 1.0f / det;

        return new Matrix3x3(

            (m11 * m22 - m12 * m21) * inv,
            (m02 * m21 - m01 * m22) * inv,
            (m01 * m12 - m02 * m11) * inv,

            (m12 * m20 - m10 * m22) * inv,
            (m00 * m22 - m02 * m20) * inv,
            (m02 * m10 - m00 * m12) * inv,

            (m10 * m21 - m11 * m20) * inv,
            (m01 * m20 - m00 * m21) * inv,
            (m00 * m11 - m01 * m10) * inv
        );
    }
    public static Matrix3x3 OuterProduct(Vector3 a, Vector3 b)
    {
        return new Matrix3x3(

            a.x * b.x,
            a.x * b.y,
            a.x * b.z,

            a.y * b.x,
            a.y * b.y,
            a.y * b.z,

            a.z * b.x,
            a.z * b.y,
            a.z * b.z
        );
    }

}
