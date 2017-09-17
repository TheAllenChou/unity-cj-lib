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

public class CylinderComponent : CjLibDemoComponent
{
  [Range(0.1f, 10.0f)]
  public float radius = 1.0f;

  [Range(0.1f, 10.0f)]
  public float height = 2.0f;

  [Range(2, 64)]
  public int segments = 16;

  protected override void Draw()
  {
    Vector3 up = transform.rotation * Vector3.up;
    Vector3 point0 = transform.position - 0.5f * height * up;
    Vector3 point1 = transform.position + 0.5f * height * up;
    DebugUtil.DrawCylinder(point0, point1, radius, segments, Color.white);
  }

}
