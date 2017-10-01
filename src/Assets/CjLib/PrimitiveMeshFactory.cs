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
  // use meshes returned by this factory right away because they are shared from cached mesh pools
  public class PrimitiveMeshFactory
  {
    
    // box
    // ------------------------------------------------------------------------

    private static Mesh s_boxWireframeMesh;

    public static Mesh Box()
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

      return s_boxWireframeMesh;
    }

    // ------------------------------------------------------------------------
    // end: box


    // rect
    // ------------------------------------------------------------------------

    private static Mesh s_rectWireframeMesh;

    // rectangle on the XZ plane centered at origin in object space, dimensions = (X dimension, Z dimension)
    public static Mesh Rect()
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

      return s_rectWireframeMesh;
    }

    // ------------------------------------------------------------------------
    // end: rect


    // circle
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_circleWireframeMeshPool;

    // draw a circle on the XZ plane centered at origin in object space
    public static Mesh Circle(int numSegments)
    {
      if (numSegments <= 1)
        return null;

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

      return mesh;
    }

    // ------------------------------------------------------------------------
    // end: circle


    // cylinder
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_cylinderWireframeMeshPool;

    public static Mesh Cylinder(int numSegments)
    {
      if (numSegments <= 1)
        return null;

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

      return mesh;
    }

    // ------------------------------------------------------------------------
    // end: cylinder


    // sphere
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_sphereWireframeMeshPool;

    public static Mesh Sphere(int latSegments, int longSegments)
    {
      if (latSegments <= 0 || longSegments <= 1)
        return null;

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

      return mesh;
    }

    // ------------------------------------------------------------------------
    // end: sphere


    // capsule
    // ------------------------------------------------------------------------

    private static Dictionary<int, Mesh> s_capsuleWireframeMeshPool;

    public static Mesh Capsule(int latSegmentsPerCap, int longSegmentsPerCap)
    {
      if (latSegmentsPerCap <= 0 || longSegmentsPerCap <= 1)
        return null;

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

      return mesh;
    }

    private static Dictionary<int, Mesh> s_capsule2dWireframeMeshPool;

    public static Mesh Capsule2D(int capSegments)
    {
      if (capSegments <= 0)
        return null;

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

      return mesh;
    }

    // ------------------------------------------------------------------------
    // end: capsule
  }
}
