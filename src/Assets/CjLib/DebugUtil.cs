using UnityEngine;

namespace CjLib
{
  public class DebugUtil
  {

    public static void DrawCircle(Vector3 center, float radius, Vector3 normal, uint numSegments, Color color, float duration, bool depthTest = true)
    {
      if (numSegments <= 1)
        return;

      Vector3 binormal = Vector3.Dot(normal, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 axisX = Vector3.Normalize(Vector3.Cross(normal, binormal));
      Vector3 axisY = Vector3.Cross(normal, axisX);
      Vector3 baseX = radius * axisX;
      Vector3 baseB = radius * axisY;

      float angleIncrement = 2.0f * Mathf.PI / numSegments;
      float angle = 0.0f;
      Vector3 prevPos = baseX;
      for (uint i = 0; i < numSegments; ++i)
      {
        angle += angleIncrement;
        Vector3 currPos = Mathf.Cos(angle) * baseX + Mathf.Sin(angle) * baseB;
        Debug.DrawLine(prevPos, currPos, color, duration, depthTest);
        prevPos = currPos;
      }
    }

    public static void DrawSphereTripleCircles(Vector3 center, float radius, Quaternion rotation, uint numSegments, Color color, float duration = 0.0f, bool depthTest = true)
    {
      Vector3 axisX = rotation * Vector3.right;
      Vector3 axisY = rotation * Vector3.up;
      Vector3 axisZ = rotation * Vector3.forward;
      DrawCircle(center, radius, axisX, numSegments, color, duration, depthTest);
      DrawCircle(center, radius, axisY, numSegments, color, duration, depthTest);
      DrawCircle(center, radius, axisZ, numSegments, color, duration, depthTest);
    }

    // identity rotation
    public static void DrawSphereTripleCircles(Vector3 center, float radius, uint numSegments, Color color, float duration = 0.0f, bool depthTest = true)
    {
      DrawSphereTripleCircles(center, radius, Quaternion.identity, numSegments, color, duration, depthTest);
    }

    public static void DrawSphere(Vector3 center, float radius, Quaternion rotation, uint latSegments, uint longSegments, Color color, float duration = 0.0f, bool depthTest = true)
    {
      if (latSegments <= 1 || longSegments <= 1)
        return;

      Vector3 axisX = rotation * Vector3.right;
      Vector3 axisY = rotation * Vector3.up;
      Vector3 axisZ = rotation * Vector3.forward;

      Vector3 top = radius * axisY;
      Vector3 bottom = -radius * axisY;

      float latAngleIncrement = 2.0f * Mathf.PI / latSegments;
      float longAngleIncrement = Mathf.PI / (longSegments + 1);
      float latAngle = 0.0f;
      float longAngle = 0.0f;

      Vector3[] aPrevLong = new Vector3[longSegments];
      for (uint iLong = 0; iLong < longSegments; ++iLong)
      {
        longAngle += longAngleIncrement;
        aPrevLong[iLong] = radius * (axisX * Mathf.Sin(longAngle) + axisY * Mathf.Cos(longAngle));
      }

      Vector3[] aCurrLong = new Vector3[longSegments];
      for (uint iLat = 0; iLat < latSegments; ++iLat)
      {
        latAngle += latAngleIncrement;

        longAngle = 0.0f;
        for (uint iLong = 0; iLong < longSegments; ++iLong)
        {
          longAngle += longAngleIncrement;
          Vector3 vecXz = axisX * Mathf.Cos(latAngle) + axisZ * Mathf.Sin(latAngle);
          aCurrLong[iLong] = radius * (vecXz * Mathf.Sin(longAngle) + axisY * Mathf.Cos(longAngle));
        }

        Debug.DrawLine(top, aCurrLong[0], color, duration, depthTest);
        for (uint iLong = 0; iLong < longSegments - 1; ++iLong)
        {
          Debug.DrawLine(aCurrLong[iLong], aCurrLong[iLong + 1], color, duration, depthTest);
          Debug.DrawLine(aCurrLong[iLong], aPrevLong[iLong], color, duration, depthTest);
        }
        Debug.DrawLine(aCurrLong[longSegments - 1], bottom);

        Vector3[] aLongTemp = aPrevLong;
        aPrevLong = aCurrLong;
        aCurrLong = aLongTemp;
      }
    }

    // identity rotation
    public static void DrawSphere(Vector3 center, float radius, uint latSegments, uint longSegments, Color color, float duration = 0.0f, bool depthTest = true)
    {
      DrawSphere(center, radius, Quaternion.identity, latSegments, longSegments, color, duration, depthTest);
    }

  }
}
