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

  private DebugUtil.Style[] m_aStyle =
   {
     DebugUtil.Style.Wireframe, 
     DebugUtil.Style.SolidColor, 
     DebugUtil.Style.FlatShaded, 
     DebugUtil.Style.SmoothShaded, 
   };
  private DebugUtil.Style m_style;
  private int m_iStyle;

  public void Start()
  {
    m_phase = 0.0f;
    m_basePos = new Vector3(-4.5f, 6.0f, 0.0f);

    m_wireframeColor = Color.white;

    m_iStyle = 0;
  }

  public void Update()
  {
    m_phase += Time.deltaTime * 2.0f * Mathf.PI / 3.0f;

    while (m_phase >= MathUtil.TwoPi)
    {
      m_iStyle = (m_iStyle + 1) % m_aStyle.Length;
      m_style = m_aStyle[m_iStyle];
      m_phase -= MathUtil.TwoPi;
    }

    m_baseRotDeg = -20.0f * Mathf.Cos(m_phase);
    m_baseRotQuat = Quaternion.Euler(20.0f * Mathf.Sin(m_phase), 0.0f, -20.0f * Mathf.Cos(m_phase));

    DrawBoxDimensions(new Vector3(0.0f, 0.0f, 0.0f));
    DrawRectDimensions(new Vector3(3.0f, 0.0f, 0.0f));

    DrawArrowDimensions(new Vector3(6.0f, 0.0f, 0.0f));
    DrawArrowSegments(new Vector3(9.0f, 0.0f, 0.0f));

    DrawCircleSegments(new Vector3(0.0f, -3.0f, 0.0f));
    DrawSphereTripleCirclesSegments(new Vector3(3.0f, -3.0f, 0.0f));
    DrawSphereLatSegments(new Vector3(6.0f, -3.0f, 0.0f));
    DrawSphereLongSegments(new Vector3(9.0f, -3.0f, 0.0f));

    DrawCylinderDimensions(new Vector3(0.0f, -6.0f, 0.0f));
    DrawCylinderSegments(new Vector3(3.0f, -6.0f, 0.0f));

    DrawConeDimensions(new Vector3(6.0f, -6.0f, 0.0f));
    DrawConeSegments(new Vector3(9.0f, -6.0f, 0.0f));

    DrawCapsuleDimensions(new Vector3(0.0f, -9.0f, 0.0f));
    DrawCapsuleLatSegments(new Vector3(3.0f, -9.0f, 0.0f));
    DrawCapsuleLongSegments(new Vector3(6.0f, -9.0f, 0.0f));
    DrawCapsule2DSegments(new Vector3(9.0f, -9.0f, 0.0f));
  }

  private void DrawBoxDimensions(Vector3 center)
  {
    Vector3 dimensions = new Vector3
    (
      1.0f + 0.2f * Mathf.Sin(m_phase), 
      1.0f + 0.2f * Mathf.Sin(m_phase + 0.6f * Mathf.PI), 
      1.0f + 0.2f * Mathf.Sin(m_phase + 1.2f * Mathf.PI)
    );

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.7f, 0.2f, 0.2f));

    DebugUtil.DrawBox(center + m_basePos, m_baseRotQuat, dimensions, color, true, m_style);
  }

  private void DrawRectDimensions(Vector3 center)
  {
    Vector2 dimensions = new Vector2
    (
      1.0f + 0.2f * Mathf.Sin(m_phase),
      1.0f + 0.2f * Mathf.Sin(m_phase + 0.6f * Mathf.PI)
    );

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.7f, 0.2f, 0.2f));

    DebugUtil.DrawRect2D(center + m_basePos, m_baseRotDeg, dimensions, color, true, m_style);
  }

  private void DrawArrowDimensions(Vector3 center)
  {
    float height = 1.1f + 0.3f * Mathf.Sin(m_phase);
    float radius = 0.3f - 0.1f * Mathf.Cos(m_phase);

    Vector3 v0 = center + m_basePos - 0.5f * height * (m_baseRotQuat * Vector3.up);
    Vector3 v1 = center + m_basePos + 0.5f * height * (m_baseRotQuat * Vector3.up);

    int numSegments = (int)Mathf.Floor(6.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.9f, 0.9f, 0.1f));

    DebugUtil.DrawArrow(v0, v1, radius, 2.0f * radius, numSegments, radius * 0.4f, color, true, m_style);
  }

  private void DrawArrowSegments(Vector3 center)
  {
    Vector3 v0 = center + m_basePos - 0.7f * (m_baseRotQuat * Vector3.up);
    Vector3 v1 = center + m_basePos + 0.7f * (m_baseRotQuat * Vector3.up);

    int numSegments = (int)Mathf.Floor(6.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.9f, 0.9f, 0.1f));

    DebugUtil.DrawArrow(v0, v1, 0.4f, 0.8f, numSegments, 0.0f, color, true, m_style);
  }

  private void DrawCircleSegments(Vector3 center)
  {
    int numSegments = (int) Mathf.Floor(8.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.8f, 0.5f, 0.0f));

    DebugUtil.DrawCircle2D(center + m_basePos, 1.0f, numSegments, color, true, m_style);
  }

  private void DrawSphereTripleCirclesSegments(Vector3 center)
  {
    int numSegments = (int) Mathf.Floor(8.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.8f, 0.5f, 0.0f));

    DebugUtil.DrawSphereTripleCircles(center + m_basePos, m_baseRotQuat, 1.0f, numSegments, color, true, m_style);
  }

  private void DrawSphereLatSegments(Vector3 center)
  {
    int latSegments = (int) Mathf.Floor(4.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.8f, 0.5f, 0.0f));

    DebugUtil.DrawSphere(center + m_basePos, m_baseRotQuat, 1.0f, latSegments, 8, color, true, m_style);
  }

  private void DrawSphereLongSegments(Vector3 center)
  {
    int longSegments = (int) Mathf.Floor(4.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.8f, 0.5f, 0.0f));

    DebugUtil.DrawSphere(center + m_basePos, m_baseRotQuat, 1.0f, 8, longSegments, color, true, m_style);
  }

  private void DrawCylinderDimensions(Vector3 center)
  {
    float height = 1.2f + 0.2f * Mathf.Sin(m_phase);
    float radius = 0.6f - 0.2f * Mathf.Cos(m_phase);

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.2f, 0.6f, 0.0f));

    DebugUtil.DrawCylinder(center + m_basePos, m_baseRotQuat, height, radius, 12, color, true, m_style);
  }

  private void DrawCylinderSegments(Vector3 center)
  {
    int numSegments = (int)Mathf.Floor(6.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.2f, 0.6f, 0.0f));

    DebugUtil.DrawCylinder(center + m_basePos, m_baseRotQuat, 1.2f, 0.6f, numSegments, color, true, m_style);
  }

  private void DrawConeDimensions(Vector3 baseCenter)
  {
    float height = 1.2f + 0.2f * Mathf.Sin(m_phase);
    float radius = 0.6f - 0.2f * Mathf.Cos(m_phase);

    baseCenter -= 0.5f * (m_baseRotQuat * Vector3.up);

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.0f, 0.4f, 0.8f));

    DebugUtil.DrawCone(baseCenter + m_basePos, m_baseRotQuat, height, radius, 12, color, true, m_style);
  }

  private void DrawConeSegments(Vector3 baseCenter)
  {
    int numSegments = (int)Mathf.Floor(6.0f + 8.0f * (1.0f + Mathf.Sin(m_phase)));

    baseCenter -= 0.5f * (m_baseRotQuat * Vector3.up);

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.0f, 0.4f, 0.8f));

    DebugUtil.DrawCone(baseCenter + m_basePos, m_baseRotQuat, 1.0f, 0.5f, numSegments, color, true, m_style);
  }

  private void DrawCapsuleDimensions(Vector3 center)
  {
    float height = 1.0f + 0.2f * Mathf.Sin(m_phase);
    float radius = 0.5f - 0.2f * Mathf.Cos(m_phase);

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.5f, 0.1f, 0.7f));

    DebugUtil.DrawCapsule(center + m_basePos, m_baseRotQuat, height, radius, 4, 6, color, true, m_style);
  }

  private void DrawCapsuleLatSegments(Vector3 center)
  {
    int latSegments = (int) Mathf.Floor(2.0f + 4.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.5f, 0.1f, 0.7f));

    DebugUtil.DrawCapsule(center + m_basePos, m_baseRotQuat, 1.0f, 0.5f, latSegments, 6, color, true, m_style);
  }

  private void DrawCapsuleLongSegments(Vector3 center)
  {
    int longSegments = (int) Mathf.Floor(4.0f + 4.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.5f, 0.1f, 0.7f));

    DebugUtil.DrawCapsule(center + m_basePos, m_baseRotQuat, 1.0f, 0.5f, 4, longSegments, color, true, m_style);
  }

  private void DrawCapsule2DSegments(Vector3 center)
  {
    float height = 1.0f + 0.2f * Mathf.Sin(m_phase);
    float radius = 0.5f - 0.2f * Mathf.Cos(m_phase);
    int capSegments = (int)Mathf.Floor(2.0f + 4.0f * (1.0f + Mathf.Sin(m_phase)));

    Color color = (m_style == DebugUtil.Style.Wireframe ? m_wireframeColor : new Color(0.5f, 0.1f, 0.7f));

    DebugUtil.DrawCapsule2D(center + m_basePos, m_baseRotDeg, height, radius, capSegments, color, true, m_style);
  }

}
