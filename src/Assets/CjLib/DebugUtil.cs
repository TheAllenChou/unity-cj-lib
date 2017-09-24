/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace CjLib
{
  public class DebugUtil
  {
    // box
    // ------------------------------------------------------------------------

    private static Mesh s_boxWireframeMesh;
    private static Material s_boxWireframeMaterial;

    public static void DrawBox(Vector3 center, Vector3 dimensions, Quaternion rotation, Color color)
    {
      if (s_boxWireframeMesh == null)
      {
        s_boxWireframeMesh = new Mesh();

        Vector3[] aVert =
        {
          new Vector3(-0.5f, -0.5f, -0.5f),
          new Vector3(-0.5f,  0.5f, -0.5f),
          new Vector3( 0.5f,  0.5f, -0.5f),
          new Vector3( 0.5f, -0.5f, -0.5f),
          new Vector3(-0.5f, -0.5f,  0.5f),
          new Vector3(-0.5f,  0.5f,  0.5f),
          new Vector3( 0.5f,  0.5f,  0.5f),
          new Vector3( 0.5f, -0.5f,  0.5f),
        };

        int[] aIndex =
        {
          0, 1,
          1, 2,
          2, 3,
          3, 0,
          2, 6,
          6, 7,
          7, 3,
          7, 4,
          4, 5,
          5, 6,
          5, 1,
          1, 0,
          0, 4,
        };

        s_boxWireframeMesh.vertices = aVert;
        s_boxWireframeMesh.SetIndices(aIndex, MeshTopology.Lines, 0);
      }

      if (s_boxWireframeMaterial == null)
        s_boxWireframeMaterial = new Material(Shader.Find("CjLib/BoxWireframe"));

      MaterialPropertyBlock properties = new MaterialPropertyBlock();
      properties.SetColor("_Color", color);
      properties.SetVector("_Dimensions", new Vector4(dimensions.x, dimensions.y, dimensions.z, 0.0f));

      Graphics.DrawMesh(s_boxWireframeMesh, center, rotation, s_boxWireframeMaterial, 0, null, 0, properties);
    }

    // ------------------------------------------------------------------------
    // end: box


    // rect
    // ------------------------------------------------------------------------

    private static Mesh s_rectWireframeMesh;
    private static Material s_rectWireframeMaterial;

    // draw a rectangle on the XZ plane centered at origin in object space, dimensions = (X dimension, Z dimension)
    public static void DrawRect(Vector3 center, Vector2 dimensions, Quaternion rotation, Color color)
    {
      if (s_rectWireframeMesh == null)
      {
        s_rectWireframeMesh = new Mesh();

        Vector3[] aVert =
        {
          new Vector3(-0.5f, 0.0f, -0.5f),
          new Vector3(-0.5f, 0.0f,  0.5f),
          new Vector3( 0.5f, 0.0f,  0.5f),
          new Vector3( 0.5f, 0.0f, -0.5f),
        };

        int[] aIndex =
        {
          0, 1,
          1, 2,
          2, 3,
          3, 0,
        };

        s_rectWireframeMesh.vertices = aVert;
        s_rectWireframeMesh.SetIndices(aIndex, MeshTopology.Lines, 0);
      }

      if (s_rectWireframeMaterial == null)
        s_rectWireframeMaterial = new Material(Shader.Find("CjLib/RectWireframe"));

      MaterialPropertyBlock properties = new MaterialPropertyBlock();
      properties.SetColor("_Color", color);
      properties.SetVector("_Dimensions", new Vector4(dimensions.x, 0.0f, dimensions.y, 0.0f));

      Graphics.DrawMesh(s_rectWireframeMesh, center, rotation, s_rectWireframeMaterial, 0, null, 0, properties);
    }

    public static void DrawRect2D(Vector3 center, Vector2 dimensions, float rotationDeg, Color color)
    {
      Quaternion rotation = Quaternion.AngleAxis(rotationDeg, Vector3.forward) * Quaternion.AngleAxis(90.0f, Vector3.right);
      DrawRect(center, dimensions, rotation, color);
    }

    // ------------------------------------------------------------------------
    // end: rect


    // circle
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_circleWireframeMeshPool;
    private static Material s_circleWireframeMaterial;

    // draw a circle on the XZ plane centered at origin in object space
    public static void DrawCircle(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color)
    {
      if (numSegments <= 1)
        return;

      if (s_circleWireframeMeshPool == null)
        s_circleWireframeMeshPool = new Dictionary<int, Mesh>();

      Mesh mesh;
      if (!s_circleWireframeMeshPool.TryGetValue(numSegments, out mesh))
      {
        mesh = new Mesh();

        Vector3[] aVert = new Vector3[numSegments];
        int[] aIndex = new int[numSegments + 1];

        float angleIncrement = 2.0f * Mathf.PI / numSegments;
        float angle = 0.0f;
        for (int i = 0; i < numSegments; ++i)
        {
          aVert[i] = Mathf.Cos(angle) * Vector3.right + Mathf.Sin(angle) * Vector3.forward;
          aIndex[i] = i;
          angle += angleIncrement;
        }
        aIndex[numSegments] = 0;

        mesh.vertices = aVert;
        mesh.SetIndices(aIndex, MeshTopology.LineStrip, 0);

        s_circleWireframeMeshPool.Add(numSegments, mesh);
      }

      if (s_circleWireframeMaterial == null)
        s_circleWireframeMaterial = new Material(Shader.Find("CjLib/CircleWireframe"));

      MaterialPropertyBlock properties = new MaterialPropertyBlock();
      properties.SetColor("_Color", color);
      properties.SetFloat("_Radius", radius);

      Graphics.DrawMesh(mesh, center, rotation, s_circleWireframeMaterial, 0, null, 0, properties);
    }

    public static void DrawCircle(Vector3 center, Vector3 normal, float radius, int numSegments, Color color)
    {
      Vector3 normalCrosser = Vector3.Dot(normal, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(normalCrosser, normal));
      Quaternion rotation = Quaternion.LookRotation(tangent, normal);
      DrawCircle(center, rotation, radius, numSegments, color);
    }

    public static void DrawCircle2D(Vector3 center, float radius, int numSegments, Color color)
    {
      DrawCircle(center, Vector3.forward, radius, numSegments, color);
    }

    // ------------------------------------------------------------------------
    // end: circle


    // cylinder
    // ------------------------------------------------------------------------

    public static void DrawCylinder(Vector3 point0, Vector3 point1, float radius, int numSegments, Color color)
    {
      if (numSegments <= 1)
        return;

      Vector3 axisY = point1 - point0;
      float axisYSelfDot = Vector3.Dot(axisY, axisY);
      if (axisYSelfDot < MathUtil.kEpsilon)
        return;

      axisY = Vector3.Normalize(axisY);
      Vector3 axisYPerp = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 axisX = Vector3.Normalize(Vector3.Cross(axisY, axisYPerp));
      Vector3 axisZ = Vector3.Cross(axisY, axisX);
      Vector3 baseX = radius * axisX;
      Vector3 baseZ = radius * axisZ;

      float angleIncrement = 2.0f * Mathf.PI / numSegments;
      float angle = 0.0f;
      Vector3 prevPos0 = point0 + baseX;
      Vector3 prevPos1 = point1 + baseX;
      for (int i = 0; i < numSegments; ++i)
      {
        angle += angleIncrement;
        Vector3 offset = Mathf.Cos(angle) * baseX + Mathf.Sin(angle) * baseZ;
        Vector3 currPos0 = point0 + offset;
        Vector3 currPos1 = point1 + offset;
        Debug.DrawLine(currPos0, currPos1, color, 0.0f);
        Debug.DrawLine(currPos0, prevPos0, color, 0.0f);
        Debug.DrawLine(currPos1, prevPos1, color, 0.0f);
        prevPos0 = currPos0;
        prevPos1 = currPos1;
      }
    }

    // ------------------------------------------------------------------------
    // cylinder


    // sphere
    // ------------------------------------------------------------------------

    public static void DrawSphereTripleCircles(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color)
    {
      Vector3 axisX = rotation * Vector3.right;
      Vector3 axisY = rotation * Vector3.up;
      Vector3 axisZ = rotation * Vector3.forward;
      DrawCircle(center, axisX, radius, numSegments, color);
      DrawCircle(center, axisY, radius, numSegments, color);
      DrawCircle(center, axisZ, radius, numSegments, color);
    }

    // identity rotation
    public static void DrawSphereTripleCircles(Vector3 center, float radius, int numSegments, Color color)
    {
      DrawSphereTripleCircles(center, Quaternion.identity, radius, numSegments, color);
    }

    public static void DrawSphere(Vector3 center, Quaternion rotation, float radius, int latSegments, int longSegments, Color color)
    {
      if (latSegments <= 0 || longSegments <= 1)
        return;

      Vector3 axisX = rotation * Vector3.right;
      Vector3 axisY = rotation * Vector3.up;
      Vector3 axisZ = rotation * Vector3.forward;

      Vector3 top = center + radius * axisY;
      Vector3 bottom = center - radius * axisY;

      float[] aLatSin = new float[latSegments];
      float[] aLatCos = new float[latSegments];
      {
        float latAngleIncrement = Mathf.PI / (latSegments + 1);
        float latAngle = 0.0f;
        for (int iLat = 0; iLat < latSegments; ++iLat)
        {
          latAngle += latAngleIncrement;
          aLatSin[iLat] = Mathf.Sin(latAngle);
          aLatCos[iLat] = Mathf.Cos(latAngle);
        }
      }

      float[] aLongSin = new float[longSegments];
      float[] aLongCos = new float[longSegments];
      {
        float longAngleIncrement = 2.0f * Mathf.PI / longSegments;
        float longAngle = 0.0f;
        for (int iLong = 0; iLong < longSegments; ++iLong)
        {
          longAngle += longAngleIncrement;
          aLongSin[iLong] = Mathf.Sin(longAngle);
          aLongCos[iLong] = Mathf.Cos(longAngle);
        }
      }

      Vector3[] aPrevLongSample = new Vector3[latSegments];
      for (int iLat = 0; iLat < latSegments; ++iLat)
      {
        float latSin = aLatSin[iLat];
        float latCos = aLatCos[iLat];
        aPrevLongSample[iLat] = center + radius * (axisX * latSin + axisY * latCos);
      }

      Vector3[] aCurrLongSample = new Vector3[latSegments];
      for (int iLong = 0; iLong < longSegments; ++iLong)
      {
        float longSin = aLongSin[iLong];
        float longCos = aLongCos[iLong];

        for (int iLat = 0; iLat < latSegments; ++iLat)
        {
          float latSin = aLatSin[iLat];
          float latCos = aLatCos[iLat];
          Vector3 vecXz = axisX * longCos + axisZ * longSin;
          aCurrLongSample[iLat] = center + radius * (vecXz * latSin + axisY * latCos);
        }

        Debug.DrawLine(top, aCurrLongSample[0], color, 0.0f);
        for (int iLat = 0; iLat < latSegments; ++iLat)
        {
          if (iLat < latSegments - 1)
          {
            Debug.DrawLine(aCurrLongSample[iLat], aCurrLongSample[iLat + 1], color, 0.0f);
          }
          Debug.DrawLine(aCurrLongSample[iLat], aPrevLongSample[iLat], color, 0.0f);
        }
        Debug.DrawLine(aCurrLongSample[latSegments - 1], bottom);

        Vector3[] aLongTempSample = aPrevLongSample;
        aPrevLongSample = aCurrLongSample;
        aCurrLongSample = aLongTempSample;
      }
    }

    // identity rotation
    public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color)
    {
      DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color);
    }

    // ------------------------------------------------------------------------
    // end: sphere


    // capsule
    // ------------------------------------------------------------------------

    public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color)
    {
      if (latSegmentsPerCap <= 0 || longSegmentsPerCap <= 1)
        return;

      Vector3 axisY = point1 - point0;
      float axisYSelfDot = Vector3.Dot(axisY, axisY);
      if (axisYSelfDot < MathUtil.kEpsilon)
        return;

      axisY = Vector3.Normalize(axisY);
      Vector3 axisYPerp = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 axisX = Vector3.Normalize(Vector3.Cross(axisY, axisYPerp));
      Vector3 axisZ = Vector3.Cross(axisY, axisX);

      Vector3 end0 = point0 - radius * axisY;
      Vector3 end1 = point1 + radius * axisY;

      latSegmentsPerCap = latSegmentsPerCap * 2 - 1;
      float[] aLatSin = new float[latSegmentsPerCap];
      float[] aLatCos = new float[latSegmentsPerCap];
      {
        float latAngleIncrement = Mathf.PI / (latSegmentsPerCap + 1);
        float latAngle = 0.0f;
        for (int iLat = 0; iLat < latSegmentsPerCap; ++iLat)
        {
          latAngle += latAngleIncrement;
          aLatSin[iLat] = Mathf.Sin(latAngle);
          aLatCos[iLat] = Mathf.Cos(latAngle);
        }
      }

      float[] aLongSin = new float[longSegmentsPerCap];
      float[] aLongCos = new float[longSegmentsPerCap];
      {
        float longAngleIncrement = 2.0f * Mathf.PI / longSegmentsPerCap;
        float longAngle = 0.0f;
        for (int iLong = 0; iLong < longSegmentsPerCap; ++iLong)
        {
          longAngle += longAngleIncrement;
          aLongSin[iLong] = Mathf.Sin(longAngle);
          aLongCos[iLong] = Mathf.Cos(longAngle);
        }
      }

      Vector3[] aPrevLongOffset = new Vector3[latSegmentsPerCap];
      for (int iLat = 0; iLat < latSegmentsPerCap; ++iLat)
      {
        float latSin = aLatSin[iLat];
        float latCos = aLatCos[iLat];
        aPrevLongOffset[iLat] = radius * (axisX * latSin + axisY * latCos);
      }

      Vector3[] aCurrLongOffset = new Vector3[latSegmentsPerCap];
      for (int iLong = 0; iLong < longSegmentsPerCap; ++iLong)
      {
        float longSin = aLongSin[iLong];
        float longCos = aLongCos[iLong];

        for (int iLat = 0; iLat < latSegmentsPerCap; ++iLat)
        {
          float latSin = aLatSin[iLat];
          float latCos = aLatCos[iLat];
          Vector3 vecXz = axisX * longCos + axisZ * longSin;
          aCurrLongOffset[iLat] = radius * (vecXz * latSin + axisY * latCos);
        }

        Debug.DrawLine(end1, point1 + aCurrLongOffset[0], color, 0.0f);
        for (int iLat = 0; iLat <= latSegmentsPerCap / 2; ++iLat)
        {
          if (iLat < latSegmentsPerCap / 2)
          {
            Debug.DrawLine(point1 + aCurrLongOffset[iLat], point1 + aCurrLongOffset[iLat + 1], color, 0.0f);
          }
          Debug.DrawLine(point1 + aCurrLongOffset[iLat], point1 + aPrevLongOffset[iLat], color, 0.0f);
        }
        Debug.DrawLine(point1 + aCurrLongOffset[latSegmentsPerCap / 2], point0 + aCurrLongOffset[latSegmentsPerCap / 2], color, 0.0f);
        for (int iLat = latSegmentsPerCap / 2; iLat < latSegmentsPerCap; ++iLat)
        {
          if (iLat < latSegmentsPerCap - 1)
          {
            Debug.DrawLine(point0 + aCurrLongOffset[iLat], point0 + aCurrLongOffset[iLat + 1], color, 0.0f);
          }
          Debug.DrawLine(point0 + aCurrLongOffset[iLat], point0 + aPrevLongOffset[iLat], color, 0.0f);
        }
        Debug.DrawLine(point0 + aCurrLongOffset[latSegmentsPerCap - 1], end0);

        Vector3[] aLongTempOffset = aPrevLongOffset;
        aPrevLongOffset = aCurrLongOffset;
        aCurrLongOffset = aLongTempOffset;
      }
    }

    public static void DrawCapsule2D(Vector3 point0, Vector3 point1, float radius, int capSegments, Color color, bool depthTest = false)
    {
      Vector3 axisY = point1 - point0;
      float axisYSelfDot = Vector3.Dot(axisY, axisY);
      if (axisYSelfDot < MathUtil.kEpsilon)
        return;

      axisY = Vector3.Normalize(axisY);
      Vector3 axisX = VectorUtil.Rotate2D(axisY, 0.5f * Mathf.PI);

      Debug.DrawLine(point0 - radius * axisX, point1 - radius * axisX, color, 0.0f);
      Debug.DrawLine(point0 + radius * axisX, point1 + radius * axisX, color, 0.0f);

      if (capSegments <= 1)
        return;

      float angleIncrement = Mathf.PI / capSegments;
      float angle = 0.0f;
      Vector3 prevCapX = radius * axisX;
      Vector3 prevCapY = Vector3.zero;
      for (int i = 0; i < capSegments; ++i)
      {
        angle += angleIncrement;
        Vector3 currCapX = radius * Mathf.Cos(angle) * axisX;
        Vector3 currCapY = radius * Mathf.Sin(angle) * axisY;

        Debug.DrawLine(point0 + prevCapX - prevCapY, point0 + currCapX - currCapY, color, 0.0f);
        Debug.DrawLine(point1 + prevCapX + prevCapY, point1 + currCapX + currCapY, color, 0.0f);

        prevCapX = currCapX;
        prevCapY = currCapY;
      }
    }

    // ------------------------------------------------------------------------
    // end: capsule
  }
}
