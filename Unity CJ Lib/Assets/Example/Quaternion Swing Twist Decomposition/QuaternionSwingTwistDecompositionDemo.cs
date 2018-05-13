using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CjLib;

public class QuaternionSwingTwistDecompositionDemo : MonoBehaviour
{
  
  private enum State
  {
    Comparison, 
    Twist, 
    Swing, 
    TwistSwing, 
  };

  private Vector3 m_axis;
  private float m_angle;
  private Quaternion m_rotFull;

  private Vector3 m_pos0;
  private Vector3 m_pos1;
  
  private Quaternion m_rotInit;

  private Quaternion m_rot0;
  private Quaternion m_rot1;

  float m_phase;


  public void Start()
  {
    m_axis = new Vector3(0.1f, 1.0f, -0.7f).normalized;
    m_angle = 150.0f;
    m_rotFull = Quaternion.AngleAxis(m_angle, m_axis);

    m_rotInit = Quaternion.FromToRotation(Vector3.up, new Vector3(2.0f, 1.0f, -1.5f)) * Quaternion.AngleAxis(45.0f, Vector3.up);

    m_pos0 = new Vector3(-1.0f, -0.5f, 0.0f);
    m_pos1 = new Vector3( 1.0f, -0.5f, 0.0f);

    m_rot0 = m_rotInit;
    m_rot1 = m_rotInit;

    m_phase = 0.0f;
  }

  public void Update()
  {
    m_phase = 0.5f * Mathf.Sin(Time.frameCount * MathUtil.kTwoPi / 120.0f) + 0.5f;

    m_rot0 = Quaternion.Slerp(m_rotInit, m_rotFull * m_rotInit, m_phase);
    DrawRod(m_pos0, m_rot0);

    Vector3 twistAxis = m_rotInit * Vector3.up;
    Quaternion swingFull, twistFull;
    QuaternionUtil.DecomopseSwingTwist(m_rotFull, twistAxis, out swingFull, out twistFull);

    Quaternion swing = Quaternion.Slerp(Quaternion.identity, swingFull, m_phase);
    Quaternion twist = Quaternion.Slerp(Quaternion.identity, twistFull, m_phase);
    m_rot1 = swing * twist * m_rotInit;
    DrawRod(m_pos1, m_rot1);

    //DrawRod(m_pos0, m_rotStart);
    //DrawRod(m_pos1, m_rotStart);

    //DrawRod(m_pos0, m_rotEnd);
    //DrawRod(m_pos1, m_rotEnd);

    m_phase += Time.deltaTime;
  }

  private void DrawRod(Vector3 pos, Quaternion rot)
  {
    float thickness = 0.15f;
    float length = 1.2f;

    Vector3 axisX = rot * Vector3.right;
    Vector3 axisY = rot * Vector3.up;
    Vector3 axisZ = rot * Vector3.forward;

    Vector3 offsetX = 0.5f * thickness * axisX;
    Vector3 offsetY = 0.5f * (length - thickness) * axisY;
    Vector3 offsetZ = 0.5f * thickness * axisZ;

    Vector2 dimsSide = new Vector2(thickness, length);
    Vector2 dimsCap = new Vector2(thickness, thickness);

    Quaternion q = Quaternion.AngleAxis(90.0f, Vector3.right);
    Quaternion r = Quaternion.AngleAxis(90.0f, Vector3.up);

    // sides
    DebugUtil.DrawRect(pos + offsetZ + offsetY, rot * q, dimsSide, Color.red, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos + offsetX + offsetY, rot * r * q, dimsSide, Color.green, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos - offsetZ + offsetY, rot * r * r * q, dimsSide, Color.red, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos - offsetX + offsetY, rot * r * r * r * q, dimsSide, Color.green, true, DebugUtil.Style.FlatShaded);

    // caps
    DebugUtil.DrawRect(pos - (0.5f * thickness) * axisY, rot, dimsCap, Color.yellow, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos + (length  - 0.5f * thickness) * axisY, rot, dimsCap, Color.yellow, true, DebugUtil.Style.FlatShaded);

    // rotationAxis
    DebugUtil.DrawArrow(pos, pos + 1.2f * m_axis, 0.1f, 0.3f, 32, 0.03f, Color.cyan, true, DebugUtil.Style.SmoothShaded);
  }

}
