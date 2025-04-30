using UnityEngine;

public static class Utils
{
    public static Vector3 Hermite(Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        float h00 = 2 * t3 - 3 * t2 + 1;
        float h10 = t3 - 2 * t2 + t;
        float h01 = -2 * t3 + 3 * t2;
        float h11 = t3 - t2;

        return h00 * p0 + h10 * m0 + h01 * p1 + h11 * m1;
    }

    public static float EstimateHermiteLength(Vector3 p0, Vector3 m0, Vector3 p1, Vector3 m1, int steps = 100)
    {
        float length = 0f;
        Vector3 prev = p0;

        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 point = Hermite(p0, m0, p1, m1, t);
            length += Vector3.Distance(prev, point);
            prev = point;
        }

        return length;
    }
}
