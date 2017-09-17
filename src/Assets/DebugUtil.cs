using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtil
{
  public static void DrawCircle(Vector3 center, float radius, Vector3 normal, uint numSegments, Color color)
  {
    if (numSegments == 0)
      return;

    Vector3 binormal = Vector3.Dot(normal, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
    Vector3 axisA = Vector3.Normalize(Vector3.Cross(normal, binormal));
    Vector3 axisB = Vector3.Cross(normal, axisA);
    Vector3 baseA = radius * axisA;
    Vector3 baseB = radius * axisB;

    float angleIncrement = 2.0f * Mathf.PI / numSegments;
    float angle = 0.0f;
    Vector3 prevPos = baseA;
    for (uint i = 0; i < numSegments; ++i)
    {
      angle += angleIncrement;
      Vector3 currPos = Mathf.Cos(angle) * baseA + Mathf.Sin(angle) * baseB;
      Debug.DrawLine(prevPos, currPos, color);
      prevPos = currPos;
    }
  }

  public static void DrawSphereCircles(Vector3 center, float radius, uint numSegments, Color color)
  {
    DrawSphereCircles(center, radius, numSegments, color, Quaternion.identity);
  }

  public static void DrawSphereCircles(Vector3 center, float radius, uint numSegments, Color color, Quaternion rotation)
  {
    Vector3 right = rotation * Vector3.right;
    Vector3 up = rotation * Vector3.up;
    Vector3 forward = rotation * Vector3.forward;
    DrawCircle(center, radius, right, numSegments, color);
    DrawCircle(center, radius, up, numSegments, color);
    DrawCircle(center, radius, forward, numSegments, color);
  }
}
