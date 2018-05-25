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

using CjLib;

public class QuaternionSwingTwistDecompositionDemo : MonoBehaviour
{
  
  private enum State
  {
    InitEnd, 
    Comparison, 
    TwistSwing,
    //TwistSwingTrajectory,
    Count
  };

  private Vector3 m_pos0;
  private Vector3 m_pos1;

  private Vector3 m_rotDiffAxis;
  private float m_rotDiffAngle;

  private Quaternion m_rotInit;
  private Quaternion m_rotDiff;
  private Quaternion m_rotEnd;

  private Quaternion m_rot0;
  private Quaternion m_rot1;

  State m_state;
  float m_phase;

  /*
  Queue<Vector3> m_trajectory0;
  Queue<Vector3> m_trajectory1;
  */

  const float kRodThickness = 0.15f;
  const float kRodLegnth = 1.2f;

  public void Start()
  {
    m_rotDiffAxis = new Vector3(0.1f, 1.0f, -0.7f).normalized;
    m_rotDiffAngle = 150.0f;

    m_rotInit = Quaternion.FromToRotation(Vector3.up, new Vector3(2.0f, 0.6f, -1.5f)) * Quaternion.AngleAxis(45.0f, Vector3.up);
    m_rotDiff = Quaternion.AngleAxis(m_rotDiffAngle, m_rotDiffAxis);
    m_rotEnd = m_rotDiff * m_rotInit;

    m_pos0 = new Vector3(-1.0f, -0.5f, 0.0f);
    m_pos1 = new Vector3( 1.0f, -0.5f, 0.0f);

    m_rot0 = m_rotInit;
    m_rot1 = m_rotInit;

    m_phase = 0.0f;

    m_state = State.InitEnd;

    /*
    m_trajectory0 = new Queue<Vector3>();
    m_trajectory1 = new Queue<Vector3>();
    */
  }

  public void Update()
  {
    m_phase = 0.5f * Mathf.Sin(Time.timeSinceLevelLoad * MathUtil.Pi) + 0.5f;

    Vector3 twistAxis = m_rotInit * Vector3.up;
    Quaternion swing;
    Quaternion twist;
    QuaternionUtil.Sterp(m_rotInit, m_rotEnd, twistAxis, m_phase, out swing, out twist);

    switch (m_state)
    {
      case State.InitEnd:
      {
        m_rot0 = m_rotInit;
        m_rot1 = m_rotEnd;
        break;
      }
      case State.Comparison:
      {
        m_rot0 = Quaternion.Slerp(m_rotInit, m_rotEnd, m_phase);
        m_rot1 = swing * twist * m_rotInit;
        break;
      }
      /*
      case State.TwistSwingTrajectory:
      {
        m_rot0 = Quaternion.Slerp(m_rotInit, m_rotEnd, m_phase);
        m_rot1 = swing * twist * m_rotInit;

        const int kQueueCapacity = 20;
        if (m_trajectory0.Count >= kQueueCapacity)
          m_trajectory0.Dequeue();
        m_trajectory0.Enqueue(m_pos0 + m_rot0 * ((kRodLegnth - 0.5f * kRodThickness) * Vector3.up));
        if (m_trajectory1.Count >= kQueueCapacity)
          m_trajectory1.Dequeue();
        m_trajectory1.Enqueue(m_pos1 + m_rot1 * ((kRodLegnth - 0.5f * kRodThickness) * Vector3.up));

        Queue<Vector3> [] aTrajectory = { m_trajectory0, m_trajectory1 };
        foreach (Queue<Vector3> trajectory in aTrajectory)
        {
          Vector3 p0 = trajectory.Peek();
          foreach (Vector3 p1 in trajectory)
          {
            DebugUtil.DrawSphere(p0, 0.01f, 16, 32, Color.white, false, DebugUtil.Style.FlatShaded);
            DebugUtil.DrawCylinder(p0, p1, 0.005f, 16, Color.white, false, DebugUtil.Style.FlatShaded);
            p0 = p1;
          }
        }

        break;
      }
      */
      case State.TwistSwing:
      {
        m_rot0 = swing * m_rotInit;
        m_rot1 = twist * m_rotInit;
        break;
      }
    }
    
    DrawRod(m_pos0, m_rot0);
    DrawRod(m_pos1, m_rot1);

    if (Input.GetKeyDown(KeyCode.Space))
    {
      ++m_state;
      if (m_state >= State.Count)
        m_state = 0;
    }
  }

  private void DrawRod(Vector3 pos, Quaternion rot)
  {
    Vector3 axisX = rot * Vector3.right;
    Vector3 axisY = rot * Vector3.up;
    Vector3 axisZ = rot * Vector3.forward;

    Vector3 offsetX = 0.5f * kRodThickness * axisX;
    Vector3 offsetY = 0.5f * (kRodLegnth - kRodThickness) * axisY;
    Vector3 offsetZ = 0.5f * kRodThickness * axisZ;

    Vector2 dimsSide = new Vector2(kRodThickness, kRodLegnth);
    Vector2 dimsCap = new Vector2(kRodThickness, kRodThickness);

    Quaternion q = Quaternion.AngleAxis(90.0f, Vector3.right);
    Quaternion r = Quaternion.AngleAxis(90.0f, Vector3.up);

    // sides
    DebugUtil.DrawRect(pos + offsetZ + offsetY, rot * q, dimsSide, Color.red, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos + offsetX + offsetY, rot * r * q, dimsSide, Color.green, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos - offsetZ + offsetY, rot * r * r * q, dimsSide, Color.red, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos - offsetX + offsetY, rot * r * r * r * q, dimsSide, Color.green, true, DebugUtil.Style.FlatShaded);

    // caps
    DebugUtil.DrawRect(pos - (0.5f * kRodThickness) * axisY, rot, dimsCap, Color.yellow, true, DebugUtil.Style.FlatShaded);
    DebugUtil.DrawRect(pos + (kRodLegnth  - 0.5f * kRodThickness) * axisY, rot, dimsCap, Color.yellow, true, DebugUtil.Style.FlatShaded);

    // rotationAxis
    DebugUtil.DrawArrow(pos, pos + 1.2f * m_rotDiffAxis, 0.1f, 0.3f, 32, 0.03f, Color.cyan, true, DebugUtil.Style.SmoothShaded);
  }

}
