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
  public float height = 2.0f;

  [Range(0.1f, 10.0f)]
  public float radius = 1.0f;

  [Range(2, 64)]
  public int capSegments = 4;

  protected override void DebugDraw()
  {
    float rotation = transform.rotation.eulerAngles.z;
    DebugUtil.DrawCapsule2D(transform.position, rotation, height, radius, capSegments, Color.white);
  }

}
