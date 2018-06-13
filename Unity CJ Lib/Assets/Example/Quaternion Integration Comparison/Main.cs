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

namespace QuaternionIntegrationComparison
{
  public class Main : MonoBehaviour
  {
    [Range(0.0f, 10.0f)]
    public float m_timeScale = 1.0f;

    public Color m_closedColor;
    public Color m_powerColor;
    public Color m_derivativeColor;

    private float m_angle;

    private Quaternion m_quatClosedForm;
    private Quaternion m_quatPower;
    private Quaternion m_quatDerivative;

    private void Reset()
    {
      m_angle = 0.0f;
      m_quatClosedForm = Quaternion.identity;
      m_quatPower = Quaternion.identity;
      m_quatDerivative = Quaternion.identity;
    }

    void Start()
    {
      Reset();
    }

    void Update()
    {
      Vector3 axis = Vector3.down;
      float angularSpeed = 1.0f;
      float dt = m_timeScale * Time.fixedDeltaTime;

      // render
      Quaternion baseRot = Quaternion.FromToRotation(Vector3.one, Vector3.up);
      DebugUtil.DrawBox(Vector3.zero, m_quatClosedForm * baseRot, Vector3.one, m_closedColor, true, DebugUtil.Style.FlatShaded);
      DebugUtil.DrawBox(Vector3.zero, m_quatPower * baseRot, 1.05f * Vector3.one, m_powerColor, true, DebugUtil.Style.Wireframe);
      DebugUtil.DrawBox(Vector3.zero, m_quatDerivative * baseRot, 1.1f * Vector3.one, m_derivativeColor, true, DebugUtil.Style.Wireframe);

      // integrate
      Quaternion v = QuaternionUtil.AxisAngle(axis, angularSpeed);
      Vector3 o = angularSpeed * axis;
      m_quatClosedForm = QuaternionUtil.Normalize(QuaternionUtil.AxisAngle(axis, (m_angle += angularSpeed * dt)));
      m_quatPower = QuaternionUtil.Normalize(QuaternionUtil.Integrate(m_quatPower, v, dt));
      m_quatDerivative = QuaternionUtil.Normalize(QuaternionUtil.Integrate(m_quatDerivative, o, dt));

      if (Input.GetKey(KeyCode.Space))
        Reset();
    }
  }
}
