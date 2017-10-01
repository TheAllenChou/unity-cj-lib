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
    private static MaterialPropertyBlock s_boxWireframeMaterialProperties;

    public static void DrawBox(Vector3 center, Quaternion rotation, Vector3 dimensions, Color color)
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
        s_boxWireframeMaterial = new Material(Shader.Find("CjLib/PrimitiveWireframeShader"));

      if (s_boxWireframeMaterialProperties == null)
        s_boxWireframeMaterialProperties = new MaterialPropertyBlock();
      
      s_boxWireframeMaterialProperties.SetColor("_Color", color);
      s_boxWireframeMaterialProperties.SetVector("_Dimensions", new Vector4(dimensions.x, dimensions.y, dimensions.z, 0.0f));

      Graphics.DrawMesh(s_boxWireframeMesh, center, rotation, s_boxWireframeMaterial, 0, null, 0, s_boxWireframeMaterialProperties, false, false, false);
    }

    // ------------------------------------------------------------------------
    // end: box


    // rect
    // ------------------------------------------------------------------------

    private static Mesh s_rectWireframeMesh;
    private static Material s_rectWireframeMaterial;
    private static MaterialPropertyBlock s_rectWireframeMaterialProperties;

    // draw a rectangle on the XZ plane centered at origin in object space, dimensions = (X dimension, Z dimension)
    public static void DrawRect(Vector3 center, Quaternion rotation, Vector2 dimensions, Color color)
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
        s_rectWireframeMaterial = new Material(Shader.Find("CjLib/PrimitiveWireframeShader"));

      if (s_rectWireframeMaterialProperties == null)
        s_rectWireframeMaterialProperties = new MaterialPropertyBlock();

      s_rectWireframeMaterialProperties.SetColor("_Color", color);
      s_rectWireframeMaterialProperties.SetVector("_Dimensions", new Vector4(dimensions.x, 0.0f, dimensions.y, 0.0f));

      Graphics.DrawMesh(s_rectWireframeMesh, center, rotation, s_rectWireframeMaterial, 0, null, 0, s_rectWireframeMaterialProperties, false, false, false);
    }

    public static void DrawRect2D(Vector3 center, float rotationDeg, Vector2 dimensions, Color color)
    {
      Quaternion rotation = Quaternion.AngleAxis(rotationDeg, Vector3.forward) * Quaternion.AngleAxis(90.0f, Vector3.right);

      DrawRect(center, rotation, dimensions, color);
    }

    // ------------------------------------------------------------------------
    // end: rect


    // circle
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_circleWireframeMeshPool;
    private static Material s_circleWireframeMaterial;
    private static MaterialPropertyBlock s_circleWireframeMaterialProperties;

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
        s_circleWireframeMaterial = new Material(Shader.Find("CjLib/PrimitiveWireframeShader"));

      if (s_circleWireframeMaterialProperties == null)
        s_circleWireframeMaterialProperties = new MaterialPropertyBlock();

      s_circleWireframeMaterialProperties.SetColor("_Color", color);
      s_circleWireframeMaterialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0.0f));

      Graphics.DrawMesh(mesh, center, rotation, s_circleWireframeMaterial, 0, null, 0, s_circleWireframeMaterialProperties, false, false, false);
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

    private static Dictionary<int, Mesh> s_cylinderWireframeMeshPool;
    private static Material s_cylinderWireframeMaterial;
    private static MaterialPropertyBlock s_cylinderWireframeMaterialProperties;

    public static void DrawCylinder(Vector3 center, Quaternion rotation, float height, float radius, int numSegments, Color color)
    {
      if (numSegments <= 1)
        return;

      if (s_cylinderWireframeMeshPool == null)
        s_cylinderWireframeMeshPool = new Dictionary<int, Mesh>();

      Mesh mesh;
      if (!s_cylinderWireframeMeshPool.TryGetValue(numSegments, out mesh))
      {
        mesh = new Mesh();

        Vector3[] aVert = new Vector3[numSegments * 2];
        int[] aIndex = new int[numSegments * 6];

        Vector3 bottom = new Vector3(0.0f, -0.5f, 0.0f);
        Vector3 top = new Vector3(0.0f, 0.5f, 0.0f);

        int iIndex = 0;
        float angleIncrement = 2.0f * Mathf.PI / numSegments;
        float angle = 0.0f;
        for (int i = 0; i < numSegments; ++i)
        {
          Vector3 offset = Mathf.Cos(angle) * Vector3.right + Mathf.Sin(angle) * Vector3.forward;
          aVert[i] = bottom + offset;
          aVert[numSegments + i] = top + offset;

          aIndex[iIndex++] = i;
          aIndex[iIndex++] = ((i + 1) % numSegments);

          aIndex[iIndex++] = i;
          aIndex[iIndex++] = numSegments + i;

          aIndex[iIndex++] = numSegments + i;
          aIndex[iIndex++] = numSegments + ((i + 1) % numSegments);

          angle += angleIncrement;
        }

        mesh.vertices = aVert;
        mesh.SetIndices(aIndex, MeshTopology.Lines, 0);

        s_cylinderWireframeMeshPool.Add(numSegments, mesh);
      }

      if (s_cylinderWireframeMaterial == null)
        s_cylinderWireframeMaterial = new Material(Shader.Find("CjLib/PrimitiveWireframeShader"));

      if (s_cylinderWireframeMaterialProperties == null)
        s_cylinderWireframeMaterialProperties = new MaterialPropertyBlock();

      s_cylinderWireframeMaterialProperties.SetColor("_Color", color);
      s_cylinderWireframeMaterialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));

      Graphics.DrawMesh(mesh, center, rotation, s_cylinderWireframeMaterial, 0, null, 0, s_cylinderWireframeMaterialProperties, false, false, false);
    }

    public static void DrawCylinder(Vector3 point0, Vector3 point1, float radius, int numSegments, Color color)
    {
      Vector3 axisY = point1 - point0;
      float height = axisY.magnitude;
      if (height < MathUtil.kEpsilon)
        return;

      Vector3 center = 0.5f * (point0 + point1);

      Vector3 axisYCrosser = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      DrawCylinder(center, rotation, height, radius, numSegments, color);
    }

    // ------------------------------------------------------------------------
    // cylinder


    // sphere
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_sphereWireframeMeshPool;
    private static Material s_sphereWireframeMaterial;
    private static MaterialPropertyBlock s_sphereWireframeMaterialProperties;

    public static void DrawSphere(Vector3 center, Quaternion rotation, float radius, int latSegments, int longSegments, Color color)
    {
      if (latSegments <= 0 || longSegments <= 1)
        return;

      if (s_sphereWireframeMeshPool == null)
        s_sphereWireframeMeshPool = new Dictionary<int, Mesh>();

      int meshKey = (latSegments << 16 ^ longSegments);
      Mesh mesh;
      if (!s_sphereWireframeMeshPool.TryGetValue(meshKey, out mesh))
      {
        mesh = new Mesh();

        Vector3[] aVert = new Vector3[longSegments * (latSegments - 1) + 2];
        int[] aIndex = new int[(longSegments * (latSegments * 2 - 1)) * 2];

        Vector3 top = new Vector3(0.0f, 1.0f, 0.0f);
        Vector3 bottom = new Vector3(0.0f, -1.0f, 0.0f);
        int iTop = aVert.Length - 2;
        int iBottom = aVert.Length - 1;
        aVert[iTop] = top;
        aVert[iBottom] = bottom;

        float[] aLatSin = new float[latSegments];
        float[] aLatCos = new float[latSegments];
        {
          float latAngleIncrement = Mathf.PI / latSegments;
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

        int iVert = 0;
        int iIndex = 0;
        for (int iLong = 0; iLong < longSegments; ++iLong)
        {
          float longSin = aLongSin[iLong];
          float longCos = aLongCos[iLong];

          for (int iLat = 0; iLat < latSegments - 1; ++iLat)
          {
            float latSin = aLatSin[iLat];
            float latCos = aLatCos[iLat];

            aVert[iVert] = new Vector3(longCos * latSin, latCos, longSin * latSin);

            if (iLat == 0)
            {
              aIndex[iIndex++] = iTop;
              aIndex[iIndex++] = iVert;
            }
            else
            {
              aIndex[iIndex++] = iVert - 1;
              aIndex[iIndex++] = iVert;
            }

            if (iLat < latSegments - 1)
            {
              aIndex[iIndex++] = iVert;
              aIndex[iIndex++] = (iVert + latSegments - 1) % (longSegments * (latSegments - 1));
            }
            
            if (iLat == latSegments - 2)
            {
              aIndex[iIndex++] = iVert;
              aIndex[iIndex++] = iBottom;
            }

            ++iVert;
          }
        }

        mesh.vertices = aVert;
        mesh.SetIndices(aIndex, MeshTopology.Lines, 0);

        s_sphereWireframeMeshPool.Add(meshKey, mesh);
      }

      if (s_sphereWireframeMaterial == null)
        s_sphereWireframeMaterial = new Material(Shader.Find("CjLib/PrimitiveWireframeShader"));

      if (s_sphereWireframeMaterialProperties == null)
        s_sphereWireframeMaterialProperties = new MaterialPropertyBlock();

      s_sphereWireframeMaterialProperties.SetColor("_Color", color);
      s_sphereWireframeMaterialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0.0f));

      Graphics.DrawMesh(mesh, center, rotation, s_sphereWireframeMaterial, 0, null, 0, s_sphereWireframeMaterialProperties, false, false, false);
    }

    // identity rotation
    public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color)
    {
      DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color);
    }

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

    // ------------------------------------------------------------------------
    // end: sphere


    // capsule
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_capsuleWireframeMeshPool;
    private static Material s_capsuleWireframeMaterial;
    private static MaterialPropertyBlock s_capsuleWireframeMaterialProperties;

    public static void DrawCapsule(Vector3 center, Quaternion rotation, float height, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color)
    {
      if (latSegmentsPerCap <= 0 || longSegmentsPerCap <= 1)
        return;

      if (s_capsuleWireframeMeshPool == null)
        s_capsuleWireframeMeshPool = new Dictionary<int, Mesh>();

      int meshKey = (latSegmentsPerCap << 16 ^ longSegmentsPerCap);
      Mesh mesh;
      if (!s_capsuleWireframeMeshPool.TryGetValue(meshKey, out mesh))
      {
        mesh = new Mesh();

        Vector3[] aVert = new Vector3[longSegmentsPerCap * latSegmentsPerCap * 2 + 2];
        int[] aIndex = new int[longSegmentsPerCap * (latSegmentsPerCap * 4 + 1) * 2];

        Vector3 top = new Vector3(0.0f, 1.5f, 0.0f);
        Vector3 bottom = new Vector3(0.0f, -1.5f, 0.0f);
        int iTop = aVert.Length - 2;
        int iBottom = aVert.Length - 1;
        aVert[iTop] = top;
        aVert[iBottom] = bottom;

        float[] aLatSin = new float[latSegmentsPerCap];
        float[] aLatCos = new float[latSegmentsPerCap];
        {
          float latAngleIncrement = 0.5f * Mathf.PI / latSegmentsPerCap;
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

        int iVert = 0;
        int iIndex = 0;
        for (int iLong = 0; iLong < longSegmentsPerCap; ++iLong)
        {
          float longSin = aLongSin[iLong];
          float longCos = aLongCos[iLong];

          for (int iLat = 0; iLat < latSegmentsPerCap; ++iLat)
          {
            float latSin = aLatSin[iLat];
            float latCos = aLatCos[iLat];

            aVert[iVert    ] = new Vector3(longCos * latSin,  latCos + 0.5f, longSin * latSin);
            aVert[iVert + 1] = new Vector3(longCos * latSin, -latCos - 0.5f, longSin * latSin);

            if (iLat == 0)
            {
              aIndex[iIndex++] = iTop;
              aIndex[iIndex++] = iVert;
              aIndex[iIndex++] = iBottom;
              aIndex[iIndex++] = iVert + 1;
            }
            else
            {
              aIndex[iIndex++] = iVert - 2;
              aIndex[iIndex++] = iVert;
              aIndex[iIndex++] = iVert - 1;
              aIndex[iIndex++] = iVert + 1;
            }

            aIndex[iIndex++] = iVert;
            aIndex[iIndex++] = (iVert + latSegmentsPerCap * 2) % (longSegmentsPerCap * latSegmentsPerCap * 2);
            aIndex[iIndex++] = iVert + 1;
            aIndex[iIndex++] = (iVert + 1 + latSegmentsPerCap * 2) % (longSegmentsPerCap * latSegmentsPerCap * 2);

            if (iLat == latSegmentsPerCap - 1)
            {
              aIndex[iIndex++] = iVert;
              aIndex[iIndex++] = iVert + 1;
            }

            iVert += 2;
          }
        }

        mesh.vertices = aVert;
        mesh.SetIndices(aIndex, MeshTopology.Lines, 0);

        s_capsuleWireframeMeshPool.Add(meshKey, mesh);
      }

      if (s_capsuleWireframeMaterial == null)
        s_capsuleWireframeMaterial = new Material(Shader.Find("CjLib/PrimitiveWireframeShader"));

      if (s_capsuleWireframeMaterialProperties == null)
        s_capsuleWireframeMaterialProperties = new MaterialPropertyBlock();

      s_capsuleWireframeMaterialProperties.SetColor("_Color", color);
      s_capsuleWireframeMaterialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));

      Graphics.DrawMesh(mesh, center, rotation, s_capsuleWireframeMaterial, 0, null, 0, s_capsuleWireframeMaterialProperties, false, false, false);
    }

    public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color)
    {
      if (latSegmentsPerCap <= 0 || longSegmentsPerCap <= 1)
        return;

      Vector3 axisY = point1 - point0;
      float height = axisY.magnitude;
      if (height < MathUtil.kEpsilon)
        return;

      Vector3 center = 0.5f * (point0 + point1);

      Vector3 axisYCrosser = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      DrawCapsule(center, rotation, height, radius, latSegmentsPerCap, longSegmentsPerCap, color);
    }

    private static Dictionary<int, Mesh> s_capsule2dWireframeMeshPool;
    private static Material s_capsule2dWireframeMaterial;
    private static MaterialPropertyBlock s_capsule2dWireframeMaterialProperties;

    public static void DrawCapsule2D(Vector3 center, float rotationDeg, float height, float radius, int capSegments, Color color)
    {
      if (capSegments <= 0)
        return;

      if (s_capsule2dWireframeMeshPool == null)
        s_capsule2dWireframeMeshPool = new Dictionary<int, Mesh>();

      Mesh mesh;
      if (!s_capsule2dWireframeMeshPool.TryGetValue(capSegments, out mesh))
      {
        mesh = new Mesh();

        Vector3[] aVert = new Vector3[(capSegments + 1) * 2];
        int[] aIndex = new int[(capSegments + 1) * 4];

        int iVert = 0;
        int iIndex = 0;
        float angleIncrement = Mathf.PI / capSegments;
        float angle = 0.0f;
        for (int i = 0; i < capSegments; ++i)
        {
          aVert[iVert++] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) + 0.5f, 0.0f);
          angle += angleIncrement;
        }
        aVert[iVert++] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) + 0.5f, 0.0f);
        for (int i = 0; i < capSegments; ++i)
        {
          aVert[iVert++] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) - 0.5f, 0.0f);
          angle += angleIncrement;
        }
        aVert[iVert++] = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle) - 0.5f, 0.0f);

        for (int i = 0; i < aVert.Length - 1; ++i)
        {
          aIndex[iIndex++] = i;
          aIndex[iIndex++] = (i + 1) % aVert.Length;
        }

        mesh.vertices = aVert;
        mesh.SetIndices(aIndex, MeshTopology.LineStrip, 0);

        s_capsule2dWireframeMeshPool.Add(capSegments, mesh);
      }

      if (s_capsule2dWireframeMaterial == null)
        s_capsule2dWireframeMaterial = new Material(Shader.Find("CjLib/PrimitiveWireframeShader"));

      if (s_capsule2dWireframeMaterialProperties == null)
        s_capsule2dWireframeMaterialProperties = new MaterialPropertyBlock();

      s_capsule2dWireframeMaterialProperties.SetColor("_Color", color);
      s_capsule2dWireframeMaterialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));

      Graphics.DrawMesh(mesh, center, Quaternion.AngleAxis(rotationDeg, Vector3.forward), s_capsule2dWireframeMaterial, 0, null, 0, s_capsule2dWireframeMaterialProperties, false, false, false);
    }

    public static void DrawCapsule2D(Vector3 point0, Vector3 point1, float radius, int capSegments, Color color, bool depthTest = false)
    {
      Vector3 axisY = point1 - point0;
      float axisYSelfDot = Vector3.Dot(axisY, axisY);
      if (axisYSelfDot < MathUtil.kEpsilon)
        return;

      axisY = Vector3.Normalize(axisY);
      Vector3 axisX = VectorUtil.Rotate2D(axisY, 90.0f);

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
