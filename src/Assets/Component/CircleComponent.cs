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

public class CircleComponent : CjLibDemoComponent
{
  [Range(0.1f, 10.0f)]
  public float radius = 1.0f;

  [Range(2, 64)]
  public int segments = 16;

  protected override void DebugDraw()
  {
    DebugUtil.DrawCircle(transform.position, transform.rotation * Vector3.up, radius, segments, Color.white);
  }

}
