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
    public enum Style
    {
      Wireframe, 
      SolidColor, 
      FlatShaded, 
      SmoothShaded, 
    };

    private static float s_wireframeZBias = 1.0e-4f;

    private static Material s_materialNormalOnZTestOn;
    private static Material s_materialNormalOffZTestOn;
    private static Material s_materiaNormalOnZTestOff;
    private static Material s_materiaNormalOffZTestOff;
    private static Material GetMaterial(Style style, bool depthTest)
    {
      bool normalOn = false;
      switch (style)
      {
        case Style.FlatShaded:
        case Style.SmoothShaded:
          normalOn = true;
          break;
      }

      if (depthTest)
      {
        if (normalOn)
        {
          if (s_materialNormalOnZTestOn == null)
          {
            s_materialNormalOnZTestOn = new Material(Shader.Find("CjLib/PrimitiveFlatColor"));
            s_materialNormalOnZTestOn.EnableKeyword("NORMAL_ON");
          }
          return s_materialNormalOnZTestOn;
        }
        else
        {
          if (s_materialNormalOffZTestOn == null)
          {
            s_materialNormalOffZTestOn = new Material(Shader.Find("CjLib/PrimitiveFlatColor"));
            s_materialNormalOffZTestOn.DisableKeyword("NORMAL_ON");
          }
          return s_materialNormalOffZTestOn;
        }
      }
      else
      {
        if (normalOn)
        {
          if (s_materiaNormalOnZTestOff == null)
          {
            s_materiaNormalOnZTestOff = new Material(Shader.Find("CjLib/PrimitiveFlatColor"));
            s_materiaNormalOnZTestOff.EnableKeyword("NORMAL_ON");
          }
          return s_materiaNormalOnZTestOff;
        }
        else
        {
          if (s_materiaNormalOffZTestOff == null)
          {
            s_materiaNormalOffZTestOff = new Material(Shader.Find("CjLib/PrimitiveFlatColor"));
            s_materiaNormalOffZTestOff.DisableKeyword("NORMAL_ON");
          }
          return s_materiaNormalOffZTestOff;
        }
      }
    }

    private static MaterialPropertyBlock s_materialProperties;
    private static MaterialPropertyBlock GetMaterialPropertyBlock()
    {
      return (s_materialProperties != null) ? s_materialProperties : (s_materialProperties = new MaterialPropertyBlock());
    }


    // line
    // ------------------------------------------------------------------------

    public static void DrawLine(Vector3 v0, Vector3 v1, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Line(v0, v1);
      if (mesh == null)
        return;

      Material material = GetMaterial(Style.Wireframe, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();
      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(1.0f, 1.0f, 1.0f, 0.0f));
      materialProperties.SetFloat("_ZBias", s_wireframeZBias);

      Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawLines(Vector3[] aVert, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.Lines(aVert);
      if (mesh == null)
        return;

      Material material = GetMaterial(Style.Wireframe, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();
      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(1.0f, 1.0f, 1.0f, 0.0f));
      materialProperties.SetFloat("_ZBias", s_wireframeZBias);

      Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawLineStrip(Vector3[] aVert, Color color, bool depthTest = true)
    {
      Mesh mesh = PrimitiveMeshFactory.LineStrip(aVert);
      if (mesh == null)
        return;

      Material material = GetMaterial(Style.Wireframe, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();
      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(1.0f, 1.0f, 1.0f, 0.0f));
      materialProperties.SetFloat("_ZBias", s_wireframeZBias);

      Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0, null, 0, materialProperties, false, false, false);
    }

    // ------------------------------------------------------------------------
    // end: line


    // box
    // ------------------------------------------------------------------------

    public static void DrawBox(Vector3 center, Quaternion rotation, Vector3 dimensions, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.BoxWireframe();
          break;
        case Style.SolidColor:
          mesh = PrimitiveMeshFactory.BoxSolidColor();
          break;
        case Style.FlatShaded:
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.BoxFlatShaded();
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();
      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(dimensions.x, dimensions.y, dimensions.z, 0.0f));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    // ------------------------------------------------------------------------
    // end: box


    // rect
    // ------------------------------------------------------------------------

    // draw a rectangle on the XZ plane centered at origin in object space, dimensions = (X dimension, Z dimension)
    public static void DrawRect(Vector3 center, Quaternion rotation, Vector2 dimensions, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.RectWireframe();
          break;
        case Style.SolidColor:
          mesh = PrimitiveMeshFactory.RectSolidColor();
          break;
        case Style.FlatShaded:
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.RectFlatShaded();
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(dimensions.x, 0.0f, dimensions.y, 0.0f));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawRect2D(Vector3 center, float rotationDeg, Vector2 dimensions, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Quaternion rotation = Quaternion.AngleAxis(rotationDeg, Vector3.forward) * Quaternion.AngleAxis(90.0f, Vector3.right);

      DrawRect(center, rotation, dimensions, color, depthTest, style);
    }

    // ------------------------------------------------------------------------
    // end: rect


    // circle
    // ------------------------------------------------------------------------

    // draw a circle on the XZ plane centered at origin in object space
    public static void DrawCircle(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.CircleWireframe(numSegments);
          break;
        case Style.SolidColor:
          mesh = PrimitiveMeshFactory.CircleSolidColor(numSegments);
          break;
        case Style.FlatShaded:
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.CircleFlatShaded(numSegments);
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0.0f));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawCircle(Vector3 center, Vector3 normal, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Vector3 normalCrosser = Vector3.Dot(normal, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(normalCrosser, normal));
      Quaternion rotation = Quaternion.LookRotation(tangent, normal);

      DrawCircle(center, rotation, radius, numSegments, color, depthTest, style);
    }

    public static void DrawCircle2D(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      DrawCircle(center, Vector3.forward, radius, numSegments, color, depthTest, style);
    }

    // ------------------------------------------------------------------------
    // end: circle


    // cylinder
    // ------------------------------------------------------------------------

    public static void DrawCylinder(Vector3 center, Quaternion rotation, float height, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.CylinderWireframe(numSegments);
          break;
        case Style.SolidColor:
          mesh = PrimitiveMeshFactory.CylinderSolidColor(numSegments);
          break;
        case Style.FlatShaded:
          mesh = PrimitiveMeshFactory.CylinderFlatShaded(numSegments);
          break;
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.CylinderSmoothShaded(numSegments);
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();
      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawCylinder(Vector3 point0, Vector3 point1, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Vector3 axisY = point1 - point0;
      float height = axisY.magnitude;
      if (height < MathUtil.kEpsilon)
        return;

      Vector3 center = 0.5f * (point0 + point1);

      Vector3 axisYCrosser = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      DrawCylinder(center, rotation, height, radius, numSegments, color, depthTest, style);
    }

    // ------------------------------------------------------------------------
    // end: cylinder


    // sphere
    // ------------------------------------------------------------------------

    public static void DrawSphere(Vector3 center, Quaternion rotation, float radius, int latSegments, int longSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.SphereWireframe(latSegments, longSegments);
          break;
        case Style.SolidColor:
          mesh = PrimitiveMeshFactory.SphereSolidColor(latSegments, longSegments);
          break;
        case Style.FlatShaded:
          mesh = PrimitiveMeshFactory.SphereFlatShaded(latSegments, longSegments);
          break;
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.SphereSmoothShaded(latSegments, longSegments);
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, 0.0f));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    // identity rotation
    public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color, depthTest, style);
    }

    public static void DrawSphereTripleCircles(Vector3 center, Quaternion rotation, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Vector3 axisX = rotation * Vector3.right;
      Vector3 axisY = rotation * Vector3.up;
      Vector3 axisZ = rotation * Vector3.forward;
      DrawCircle(center, axisX, radius, numSegments, color, depthTest, style);
      DrawCircle(center, axisY, radius, numSegments, color, depthTest, style);
      DrawCircle(center, axisZ, radius, numSegments, color, depthTest, style);
    }

    // identity rotation
    public static void DrawSphereTripleCircles(Vector3 center, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      DrawSphereTripleCircles(center, Quaternion.identity, radius, numSegments, color, depthTest, style);
    }

    // ------------------------------------------------------------------------
    // end: sphere


    // capsule
    // ------------------------------------------------------------------------

    public static void DrawCapsule(Vector3 center, Quaternion rotation, float height, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.CapsuleWireframe(latSegmentsPerCap, longSegmentsPerCap);
          break;
        case Style.SolidColor:
          mesh = PrimitiveMeshFactory.CapsuleSolidColor(latSegmentsPerCap, longSegmentsPerCap);
          break;
        case Style.FlatShaded:
          mesh = PrimitiveMeshFactory.CapsuleFlatShaded(latSegmentsPerCap, longSegmentsPerCap);
          break;
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.CapsuleSmoothShaded(latSegmentsPerCap, longSegmentsPerCap);
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, center, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, bool depthTest = true, Style style = Style.Wireframe)
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

      DrawCapsule(center, rotation, height, radius, latSegmentsPerCap, longSegmentsPerCap, color, depthTest, style);
    }

    public static void DrawCapsule2D(Vector3 center, float rotationDeg, float height, float radius, int capSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.Capsule2DWireframe(capSegments);
          break;
        case Style.SolidColor:
          mesh = PrimitiveMeshFactory.Capsule2DSolidColor(capSegments);
          break;
        case Style.FlatShaded:
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.Capsule2DFlatShaded(capSegments);
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, radius, radius, height));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, center, Quaternion.AngleAxis(rotationDeg, Vector3.forward), material, 0, null, 0, materialProperties, false, false, false);
    }

    // ------------------------------------------------------------------------
    // end: capsule


    // cone
    // ------------------------------------------------------------------------

    public static void DrawCone(Vector3 baseCenter, Quaternion rotation, float height, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
        case Style.SolidColor:
        case Style.FlatShaded:
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.ConeWireframe(numSegments);
          break;
      }
      if (mesh == null)
        return;

      Material material = GetMaterial(style, depthTest);
      MaterialPropertyBlock materialProperties = GetMaterialPropertyBlock();

      materialProperties.SetColor("_Color", color);
      materialProperties.SetVector("_Dimensions", new Vector4(radius, height, radius, 0.0f));
      materialProperties.SetFloat("_ZBias", (style == Style.Wireframe) ? s_wireframeZBias : 0.0f);

      Graphics.DrawMesh(mesh, baseCenter, rotation, material, 0, null, 0, materialProperties, false, false, false);
    }

    public static void DrawCone(Vector3 baseCenter, Vector3 top, float radius, int numSegments, Color color, bool depthTest = true, Style style = Style.Wireframe)
    {
      if (numSegments <= 1)
        return;

      Vector3 axisY = top - baseCenter;
      float height = axisY.magnitude;
      if (height < MathUtil.kEpsilon)
        return;

      Vector3 axisYCrosser = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      DrawCone(baseCenter, rotation, height, radius, numSegments, color, depthTest, style);
    }

    // ------------------------------------------------------------------------
    // end: cone
  }
}
