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

    private static Material s_materialWithZTest;
    private static Material s_materialNoZTest;
    private static Material GetMaterial(bool depthTest)
    {
      return
        depthTest
        ? (s_materialWithZTest != null ? s_materialWithZTest : (s_materialWithZTest = new Material(Shader.Find("CjLib/PrimitiveFlatColor"))))
        : (s_materialNoZTest != null ? s_materialNoZTest : (s_materialNoZTest = new Material(Shader.Find("CjLib/PrimitiveFlatColorNoZTest"))));
    }

    private static MaterialPropertyBlock s_materialProperties;
    private static MaterialPropertyBlock GetMaterialPropertyBlock()
    {
      return (s_materialProperties != null) ? s_materialProperties : (s_materialProperties = new MaterialPropertyBlock());
    }


    // box
    // ------------------------------------------------------------------------

    public static void DrawBox(Vector3 center, Quaternion rotation, Vector3 dimensions, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Box();
      if (mesh == null)
        return;

      Material material = GetMaterial(depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();
      
      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(dimensions.x, dimensions.y, dimensions.z, 0.0f));

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    // ------------------------------------------------------------------------
    // end: box

        // line
        // ------------------------------------------------------------------------

        // draw a line from from to to in world space
        public static void DrawLine(Vector3 from, Vector3 to, Color color, bool depthTest = true)
        {
            Mesh mesh = new Mesh();
            Vector3[] aVert ={from,to};
            int[] aIndex ={0, 1,};

            mesh.vertices = aVert;
            mesh.SetIndices(aIndex, MeshTopology.Lines, 0);

            Material material = GetMaterial(depthTest);
            MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

            materialProperties.SetColor("_Color", color);
            Vector3 center = (from + to) * 0.5f;
            Vector3 aabbMin = Vector3.Min(from, to);
            Vector3 aabbMax = Vector3.Max(from, to);
            Vector3 halfExtents = (aabbMax - aabbMin);
            materialProperties.SetVector("_Dimensions", new Vector4(halfExtents.x, halfExtents.y, halfExtents.z, 0.0f));

            Quaternion rotation = new Quaternion(0, 0, 0, 1);
            Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
        }
        // ------------------------------------------------------------------------
        // end: line

        // rect
    // ------------------------------------------------------------------------

    // draw a rectangle on the XZ plane centered at origin in object space, dimensions = (X dimension, Z dimension)
    public static void DrawRect(Vector3 center, Quaternion rotation, Vector2 dimensions, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Rect();
      if (mesh == null)
        return;

      Material material = GetMaterial(depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(dimensions.x, 0.0f, dimensions.y, 0.0f));

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawRect2D(Vector3 center, float rotationDeg, Vector2 dimensions, Color color, bool depthTest = true)
    {
      Quaternion rotation = Quaternion.AngleAxis(rotationDeg, Vector3.forward) * Quaternion.AngleAxis(90.0f, Vector3.right);

      DrawRect(center, rotation, dimensions, color);
    }

    // ------------------------------------------------------------------------
    // end: rect


    // circle
    // ------------------------------------------------------------------------


    // draw a circle on the XZ plane centered at origin in object space
    public static void DrawCircle(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Circle(numSegments);
      if (mesh == null)
        return;

      Material material = GetMaterial(depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0.0f));

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawCircle(Vector3 center, Vector3 normal, float radius, int numSegments, Color color, bool depthTest = true)
    {
      Vector3 normalCrosser = Vector3.Dot(normal, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(normalCrosser, normal));
      Quaternion rotation = Quaternion.LookRotation(tangent, normal);

      DrawCircle(center, rotation, radius, numSegments, color);
    }

    public static void DrawCircle2D(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true)
    {
      DrawCircle(center, Vector3.forward, radius, numSegments, color);
    }

    // ------------------------------------------------------------------------
    // end: circle


    // cylinder
    // ------------------------------------------------------------------------


    public static void DrawCylinder(Vector3 center, Quaternion rotation, float height, float radius, int numSegments, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Cylinder(numSegments);
      if (mesh == null)
        return;

      Material material = GetMaterial(depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawCylinder(Vector3 point0, Vector3 point1, float radius, int numSegments, Color color, bool depthTest = true)
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
    // end: cylinder


    // sphere
    // ------------------------------------------------------------------------

    public static void DrawSphere(Vector3 center, Quaternion rotation, float radius, int latSegments, int longSegments, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Sphere(latSegments, longSegments);
      if (mesh == null)
        return;

      Material material = GetMaterial(depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0.0f));

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    // identity rotation
    public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color, bool depthTest = true)
    {
      DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color);
    }

    public static void DrawSphereTripleCircles(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color, bool depthTest = true)
    {
      Vector3 axisX = rotation * Vector3.right;
      Vector3 axisY = rotation * Vector3.up;
      Vector3 axisZ = rotation * Vector3.forward;
      DrawCircle(center, axisX, radius, numSegments, color);
      DrawCircle(center, axisY, radius, numSegments, color);
      DrawCircle(center, axisZ, radius, numSegments, color);
    }

    // identity rotation
    public static void DrawSphereTripleCircles(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true)
    {
      DrawSphereTripleCircles(center, Quaternion.identity, radius, numSegments, color);
    }

    // ------------------------------------------------------------------------
    // end: sphere


    // capsule
    // ------------------------------------------------------------------------

    public static void DrawCapsule(Vector3 center, Quaternion rotation, float height, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Capsule(latSegmentsPerCap, longSegmentsPerCap);
      if (mesh == null)
        return;

      Material material = GetMaterial(depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, bool depthTest = true)
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

    public static void DrawCapsule2D(Vector3 center, float rotationDeg, float height, float radius, int capSegments, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Capsule2D(capSegments);
      if (mesh == null)
        return;

      Material material = GetMaterial(depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));

      Graphics.DrawMesh(mesh, center, Quaternion.AngleAxis(rotationDeg, Vector3.forward), material, 0, null, 0, materialProperties, false, false, false);
    }

    // ------------------------------------------------------------------------
    // end: capsule
  }
}
