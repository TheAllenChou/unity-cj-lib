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
  [ExecuteInEditMode]
  public class DrawArc : DrawBase
  {
    public float Radius = 1.0f;
    public int NumSegments = 64;

    public float StartAngle = 0.0f;
    public float ArcAngle = 60.0f;

    private void OnValidate()
    {
      Wireframe = true;
      Style = DebugUtil.Style.Wireframe;

      Radius = Mathf.Max(0.0f, Radius);
      NumSegments = Mathf.Max(0, NumSegments);
    }

    protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
    {
      Quaternion startRot = QuaternionUtil.AxisAngle(Vector3.forward, StartAngle * MathUtil.Deg2Rad);
      DebugUtil.DrawArc
      (
        transform.position, 
        transform.rotation * startRot * Vector3.right,  
        transform.rotation * Vector3.forward, 
        ArcAngle * MathUtil.Deg2Rad, Radius, NumSegments, 
        color, depthTest
      );
    }
  }
}
