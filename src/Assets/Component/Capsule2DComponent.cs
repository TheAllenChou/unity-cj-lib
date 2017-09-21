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

public class Capsule2DComponent : CjLibDemoComponent
{
  [Range(0.1f, 10.0f)]
  public float radius = 1.0f;

  [Range(0.1f, 10.0f)]
  public float height = 2.0f;

  [Range(2, 64)]
  public int capSegments = 4;

  protected override void Draw()
  {
    float rotation = MathUtil.kDeg2Rad * transform.rotation.eulerAngles.z;
    Vector3 up = VectorUtil.Rotate2D(Vector3.up, rotation);
    Vector3 point0 = transform.position - 0.5f * height * up;
    Vector3 point1 = transform.position + 0.5f * height * up;
    DebugUtil.DrawCapsule2D(point0, point1, radius, capSegments, Color.white);
  }

}
