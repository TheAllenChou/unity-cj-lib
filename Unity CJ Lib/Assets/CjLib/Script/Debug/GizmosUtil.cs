/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using UnityEngine;

namespace CjLib
{
  public class GizmosUtil
  {
    public enum Style
    {
      Wireframe,
      FlatShaded,
      SmoothShaded,
    };


    // line
    // ------------------------------------------------------------------------

    public static void DrawLine(Vector3 v0, Vector3 v1, Color color)
    {
      Gizmos.color = color;
      Gizmos.DrawLine(v0, v1);
    }

    public static void DrawLines(Vector3[] aVert, Color color)
    {
      Gizmos.color = color;
      for (int i = 0; i < aVert.Length; i += 2)
      {
        Gizmos.DrawLine(aVert[i], aVert[i + 1]);
      }
    }

    public static void DrawLineStrip(Vector3[] aVert, Color color)
    {
      Gizmos.color = color;
      for (int i = 0; i < aVert.Length; ++i)
      {
        Gizmos.DrawLine(aVert[i], aVert[i + 1]);
      }
    }

    // ------------------------------------------------------------------------
    // end: line


    // box
    // ------------------------------------------------------------------------

    public static void DrawBox(Vector3 center, Quaternion rotation, Vector3 dimensions, Color color, Style style = Style.FlatShaded)
    {
      if (dimensions.x < MathUtil.Epsilon || dimensions.y < MathUtil.Epsilon || dimensions.z < MathUtil.Epsilon)
        return;

      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.BoxWireframe();
          break;
        case Style.FlatShaded:
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.BoxFlatShaded();
          break;
      }
      if (mesh == null)
        return;

      Gizmos.color = color;
      if (style == Style.Wireframe)
      {
        Gizmos.DrawWireMesh(mesh, center, rotation, dimensions);
      }
      else
      {
        Gizmos.DrawMesh(mesh, center, rotation, dimensions);
      }
    }

    // ------------------------------------------------------------------------
    // end: box


    // cylinder
    // ------------------------------------------------------------------------

    public static void DrawCylinder(Vector3 center, Quaternion rotation, float height, float radius, int numSegments, Color color, Style style = Style.SmoothShaded)
    {
      if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
        return;

      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.CylinderWireframe(numSegments);
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

      Gizmos.color = color;
      if (style == Style.Wireframe)
      {
        Gizmos.DrawWireMesh(mesh, center, rotation, new Vector3(radius, height, radius));
      }
      else
      {
        Gizmos.DrawMesh(mesh, center, rotation, new Vector3(radius, height, radius));
      }
    }

    public static void DrawCylinder(Vector3 point0, Vector3 point1, float radius, int numSegments, Color color, Style style = Style.SmoothShaded)
    {
      Vector3 axisY = point1 - point0;
      float height = axisY.magnitude;
      if (height < MathUtil.Epsilon)
        return;

      axisY.Normalize();

      Vector3 center = 0.5f * (point0 + point1);

      Vector3 axisYCrosser = Vector3.Dot(axisY.normalized, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      DrawCylinder(center, rotation, height, radius, numSegments, color, style);
    }

    // ------------------------------------------------------------------------
    // end: cylinder


    // sphere
    // ------------------------------------------------------------------------

    public static void DrawSphere(Vector3 center, Quaternion rotation, float radius, int latSegments, int longSegments, Color color, Style style = Style.SmoothShaded)
    {
      if (radius < MathUtil.Epsilon)
        return;

      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.SphereWireframe(latSegments, longSegments);
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

      Gizmos.color = color;
      if (style == Style.Wireframe)
      {
        Gizmos.DrawWireMesh(mesh, center, rotation, new Vector3(radius, radius, radius));
      }
      else
      {
        Gizmos.DrawMesh(mesh, center, rotation, new Vector3(radius, radius, radius));
      }
    }

    // identity rotation
    public static void DrawSphere(Vector3 center, float radius, int latSegments, int longSegments, Color color, Style style = Style.SmoothShaded)
    {
      DrawSphere(center, Quaternion.identity, radius, latSegments, longSegments, color, style);
    }

    // ------------------------------------------------------------------------
    // end: sphere


    // capsule
    // ------------------------------------------------------------------------

    public static void DrawCapsule(Vector3 center, Quaternion rotation, float height, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, Style style = Style.SmoothShaded)
    {
      if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
        return;

      Mesh meshCaps = null;
      Mesh meshSides = null;
      switch (style)
      {
        case Style.Wireframe:
          meshCaps = PrimitiveMeshFactory.CapsuleWireframe(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
          meshSides = PrimitiveMeshFactory.CapsuleWireframe(latSegmentsPerCap, longSegmentsPerCap, false, false, true);
          break;
        case Style.FlatShaded:
          meshCaps = PrimitiveMeshFactory.CapsuleFlatShaded(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
          meshSides = PrimitiveMeshFactory.CapsuleFlatShaded(latSegmentsPerCap, longSegmentsPerCap, false, false, true);
          break;
        case Style.SmoothShaded:
          meshCaps = PrimitiveMeshFactory.CapsuleSmoothShaded(latSegmentsPerCap, longSegmentsPerCap, true, true, false);
          meshSides = PrimitiveMeshFactory.CapsuleSmoothShaded(latSegmentsPerCap, longSegmentsPerCap, false, false, true);
          break;
      }
      if (meshCaps == null || meshSides == null)
        return;

      Vector3 axisY = rotation * Vector3.up;
      Vector3 topCapOffset = 0.5f * (height - radius) * axisY;
      Vector3 topCapCenter = center + topCapOffset;
      Vector3 bottomCapCenter = center - topCapOffset;

      Quaternion bottomCapRotation = Quaternion.AngleAxis(180.0f, axisY) * rotation;

      Gizmos.color = color;
      if (style == Style.Wireframe)
      {
        Gizmos.DrawWireMesh(meshCaps, topCapCenter, rotation, new Vector3(radius, radius, radius));
        Gizmos.DrawWireMesh(meshCaps, bottomCapCenter, bottomCapRotation, new Vector3(-radius, -radius, radius));
        Gizmos.DrawWireMesh(meshSides, center, rotation, new Vector3(radius, height, radius));
      }
      else
      {
        Gizmos.DrawMesh(meshCaps, topCapCenter, rotation, new Vector3(radius, radius, radius));
        Gizmos.DrawMesh(meshCaps, bottomCapCenter, bottomCapRotation, new Vector3(-radius, -radius, radius));
        Gizmos.DrawMesh(meshSides, center, rotation, new Vector3(radius, height, radius));
      }
    }

    public static void DrawCapsule(Vector3 point0, Vector3 point1, float radius, int latSegmentsPerCap, int longSegmentsPerCap, Color color, Style style = Style.SmoothShaded)
    {
      Vector3 axisY = point1 - point0;
      float height = axisY.magnitude;
      if (height < MathUtil.Epsilon)
        return;

      axisY.Normalize();

      Vector3 center = 0.5f * (point0 + point1);

      Vector3 axisYCrosser = Vector3.Dot(axisY.normalized, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      DrawCapsule(center, rotation, height, radius, latSegmentsPerCap, longSegmentsPerCap, color, style);
    }

    // ------------------------------------------------------------------------
    // end: capsule


    // cone
    // ------------------------------------------------------------------------

    public static void DrawCone(Vector3 baseCenter, Quaternion rotation, float height, float radius, int numSegments, Color color, Style style = Style.FlatShaded)
    {
      if (height < MathUtil.Epsilon || radius < MathUtil.Epsilon)
        return;

      Mesh mesh = null;
      switch (style)
      {
        case Style.Wireframe:
          mesh = PrimitiveMeshFactory.ConeWireframe(numSegments);
          break;
        case Style.FlatShaded:
          mesh = PrimitiveMeshFactory.ConeFlatShaded(numSegments);
          break;
        case Style.SmoothShaded:
          mesh = PrimitiveMeshFactory.ConeSmoothShaded(numSegments);
          break;
      }
      if (mesh == null)
        return;

      Gizmos.color = color;
      if (style == Style.Wireframe)
      {
        Gizmos.DrawWireMesh(mesh, baseCenter, rotation, new Vector3(radius, height, radius));
      }
      else
      {
        Gizmos.DrawMesh(mesh, baseCenter, rotation, new Vector3(radius, height, radius));
      }
    }

    public static void DrawCone(Vector3 baseCenter, Vector3 top, float radius, int numSegments, Color color, Style style = Style.FlatShaded)
    {
      Vector3 axisY = top - baseCenter;
      float height = axisY.magnitude;
      if (height < MathUtil.Epsilon)
        return;

      axisY.Normalize();

      Vector3 axisYCrosser = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      DrawCone(baseCenter, rotation, height, radius, numSegments, color, style);
    }

    // ------------------------------------------------------------------------
    // end: cone


    // arrow
    // ------------------------------------------------------------------------

    public static void DrawArrow(Vector3 from, Vector3 to, float coneRadius, float coneHeight, int numSegments, float stemThickness, Color color, Style style = Style.FlatShaded)
    {
      Vector3 axisY = to - from;
      float axisLength = axisY.magnitude;
      if (axisLength < MathUtil.Epsilon)
        return;

      axisY.Normalize();

      Vector3 axisYCrosser = Vector3.Dot(axisY, Vector3.up) < 0.5f ? Vector3.up : Vector3.forward;
      Vector3 tangent = Vector3.Normalize(Vector3.Cross(axisYCrosser, axisY));
      Quaternion rotation = Quaternion.LookRotation(tangent, axisY);

      Vector3 coneBaseCenter = to - coneHeight * axisY; // top of cone ends at "to"
      DrawCone(coneBaseCenter, rotation, coneHeight, coneRadius, numSegments, color, style);

      if (stemThickness <= 0.0f)
      {
        if (style != Style.Wireframe)
          to -= coneHeight * axisY;

        DrawLine(from, to, color);
      }
      else if (coneHeight < axisLength)
      {
        to -= coneHeight * axisY;

        DrawCylinder(from, to, 0.5f * stemThickness, numSegments, color, style);
      }
    }

    public static void DrawArrow(Vector3 from, Vector3 to, float size, Color color, Style style = Style.FlatShaded)
    {
      DrawArrow(from, to, 0.5f * size, size, 8, 0.0f, color, style);
    }

    // ------------------------------------------------------------------------
    // end: arrow
  }
}
