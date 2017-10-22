/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

using CjLib;
using UnityEngine;

public class CjLibDebugUtilDemo : MonoBehaviour
{

  private float m_phase;
  private Vector3 m_basePos;
  private float m_baseRotDeg;
  private Quaternion m_baseRotQuat;

  private Color m_wireframeColor;
  private Color m_solidColor;

  public void Start()
  {
    m_phase = 0.0f;
    m_basePos = new Vector3(-4.5f, 6.0f, 0.0f);

    m_wireframeColor = Color.white;
    m_solidColor = new Color(0.0f, 0.5f, 0.5f);
  }

  public void Update()
  {
    m_baseRotDeg = -20.0f * Mathf.Cos(m_phase);
    m_baseRotQuat = Quaternion.Euler(20.0f * Mathf.Sin(m_phase), 0.0f, -20.0f * Mathf.Cos(m_phase));

    DrawBoxDimensions(new Vector3(0.0f, 0.0f, 0.0f));
    DrawRectDimensions(new Vector3(3.0f, 0.0f, 0.0f));

    DrawCircleSegments(new Vector3(0.0f, -3.0f, 0.0f));
    DrawSphereTripleCirclesSegments(new Vector3(3.0f, -3.0f, 0.0f));
    DrawSphereLatSegments(new Vector3(6.0f, -3.0f, 0.0f));
    DrawSphereLongSegments(new Vector3(9.0f, -3.0f, 0.0f));

    DrawCylinderDimensions(new Vector3(0.0f, -6.0f, 0.0f));
    DrawCylinderSegments(new Vector3(3.0f, -6.0f, 0.0f));

    DrawCapsuleDimensions(new Vector3(0.0f, -9.0f, 0.0f));
    DrawCapsuleLatSegments(new Vector3(3.0f, -9.0f, 0.0f));
    DrawCapsuleLongSegments(new Vector3(6.0f, -9.0f, 0.0f));

    m_phase += Time.deltaTime * 2.0f * Mathf.PI / 3.0f;
  }

  private void DrawBoxDimensions(Vector3 center)
  {
    Vector3 dimensions = new Vector3
    (
      1.0f + 0.2f * Mathf.Sin(m_phase), 
      1.0f + 0.2f * Mathf.Sin(m_phase + 0.6f * Mathf.PI), 
      1.0f + 0.2f * Mathf.Sin(m_phase + 1.2f * Mathf.PI)
    );

    DebugUtil.DrawBox(center + m_basePos, m_baseRotQuat, dimensions, m_solidColor, true, DebugUtil.Style.SolidFlatShaded);
    DebugUtil.DrawBox(center + m_basePos, m_baseRotQuat, dimensions, m_wireframeColor);
  }

  private void DrawRectDimensions(Vector3 center)
  {
    Vector2 dimensions = new Vector2
    (
      1.0f + 0.2f * Mathf.Sin(m_phase),
      1.0f + 0.2f * Mathf.Sin(m_phase + 0.6f * Mathf.PI)
    );

    DebugUtil.DrawRect2D(center + m_basePos, m_baseRotDeg, dimensions, m_solidColor, true, DebugUtil.Style.SolidFlatShaded);
    DebugUtil.DrawRect2D(center + m_basePos, m_baseRotDeg, dimensions, m_wireframeColor);
  }

  private void DrawCircleSegments(Vector3 center)
  {
    int numSegments = (int) Mathf.Floor(8.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    DebugUtil.DrawCircle2D(center + m_basePos, 1.0f, numSegments, m_solidColor, true, DebugUtil.Style.SolidFlatShaded);
    DebugUtil.DrawCircle2D(center + m_basePos, 1.0f, numSegments, m_wireframeColor);
  }

  private void DrawSphereTripleCirclesSegments(Vector3 center)
  {
    int numSegments = (int) Mathf.Floor(8.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    DebugUtil.DrawSphereTripleCircles(center + m_basePos, m_baseRotQuat, 1.0f, numSegments, m_solidColor, true, DebugUtil.Style.SolidFlatShaded);
    DebugUtil.DrawSphereTripleCircles(center + m_basePos, m_baseRotQuat, 1.0f, numSegments, m_wireframeColor);
  }

  private void DrawSphereLatSegments(Vector3 center)
  {
    int latSegments = (int) Mathf.Floor(4.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    DebugUtil.DrawSphere(center + m_basePos, m_baseRotQuat, 1.0f, latSegments, 8, m_wireframeColor);
  }

  private void DrawSphereLongSegments(Vector3 center)
  {
    int longSegments = (int) Mathf.Floor(4.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    DebugUtil.DrawSphere(center + m_basePos, m_baseRotQuat, 1.0f, 8, longSegments, m_wireframeColor, false);
  }

  private void DrawCylinderDimensions(Vector3 center)
  {
    float height = 1.2f + 0.2f * Mathf.Sin(m_phase);
    float radius = 0.6f - 0.2f * Mathf.Cos(m_phase);

    DebugUtil.DrawCylinder(center + m_basePos, m_baseRotQuat, height, radius, 12, m_wireframeColor);
  }

  private void DrawCylinderSegments(Vector3 center)
  {
    int numSegments = (int)Mathf.Floor(6.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    DebugUtil.DrawCylinder(center + m_basePos, m_baseRotQuat, 1.2f, 0.6f, numSegments, m_wireframeColor);
  }

  private void DrawCapsuleDimensions(Vector3 center)
  {
    float height = 1.0f + 0.2f * Mathf.Sin(m_phase);
    float radius = 0.5f - 0.2f * Mathf.Cos(m_phase);

    DebugUtil.DrawCapsule(center + m_basePos, m_baseRotQuat, height, radius, 4, 6, m_wireframeColor);
  }

  private void DrawCapsuleLatSegments(Vector3 center)
  {
    int latSegments = (int) Mathf.Floor(2.0f + 4.0f * (1.0f + Mathf.Sin(m_phase)));

    DebugUtil.DrawCapsule(center + m_basePos, m_baseRotQuat, 1.0f, 0.5f, latSegments, 6, m_wireframeColor);
  }

  private void DrawCapsuleLongSegments(Vector3 center)
  {
    int longSegments = (int) Mathf.Floor(4.0f + 4.0f * (1.0f + Mathf.Sin(m_phase)));

    DebugUtil.DrawCapsule(center + m_basePos, m_baseRotQuat, 1.0f, 0.5f, 4, longSegments, m_wireframeColor);
  }

}
