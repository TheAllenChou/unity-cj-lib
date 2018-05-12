using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CjLib;

public class GizmosUtilDemoComponent : MonoBehaviour
{
  public enum Primitive
  {
    Arrow, 
    Box, 
    Capsule, 
    Cone, 
    Cylinder, 
    Sphere, 
  };

  public Primitive m_primitive = Primitive.Box;
  public GizmosUtil.Style m_style = GizmosUtil.Style.FlatShaded;
  public Color m_color = Color.white;

  [Range(0.1f, 10.0f)]
  public float m_width = 1.0f;

  [Range(0.1f, 10.0f)]
  public float m_depth = 1.0f;

  [Range(0.1f, 10.0f)]
  public float m_height = 1.0f;

  [Range(0.1f, 10.0f)]
  public float m_radius = 0.5f;

  [Range(0.05f, 0.2f)]
  public float m_arrowTipSize = 0.1f;

  [Range(2, 32)]
  public int m_latSegments = 8;

  [Range(2, 32)]
  public int m_longSegments = 16;

  void OnDrawGizmos()
  {
    float width = m_width * Mathf.Abs(transform.localScale.x);
    float height = m_height * Mathf.Abs(transform.localScale.y);
    float depth = m_depth * Mathf.Abs(transform.localScale.z);
    float radius = m_radius * 0.5f * Mathf.Abs(transform.localScale.x + transform.localScale.z);

    switch (m_primitive)
    {
      case Primitive.Arrow:
        Vector3 axisY = transform.rotation * Vector3.up;
        Vector3 endOffset = 0.5f * height * axisY;
        GizmosUtil.DrawArrow(transform.position - endOffset, transform.position + endOffset, m_arrowTipSize, 2.0f * m_arrowTipSize, m_latSegments, 0.0f, m_color, m_style);
        break;
      case Primitive.Box:
        GizmosUtil.DrawBox(transform.position, transform.rotation, new Vector3(width, height, depth), m_color, m_style);
        break;
      case Primitive.Capsule:
        GizmosUtil.DrawCapsule(transform.position, transform.rotation, height, radius, m_latSegments, m_longSegments, m_color, m_style);
        break;
      case Primitive.Cone:
        GizmosUtil.DrawCone(transform.position, transform.rotation, height, radius, m_longSegments, m_color, m_style);
        break;
      case Primitive.Cylinder:
        GizmosUtil.DrawCylinder(transform.position, transform.rotation, height, radius, m_longSegments, m_color, m_style);
        break;
      case Primitive.Sphere:
        GizmosUtil.DrawSphere(transform.position, transform.rotation, radius, m_latSegments, m_longSegments, m_color, m_style);
        break;
    }
  }
}
