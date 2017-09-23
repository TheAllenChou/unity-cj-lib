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

public class Box2DComponent : CjLibDemoComponent
{
  [Range(0.1f, 10.0f)]
  public float dimensionX = 1.0f;

  [Range(0.1f, 10.0f)]
  public float dimensionY = 1.0f;

  protected override void DebugDraw()
  {
    float rotation = MathUtil.kDeg2Rad * transform.rotation.eulerAngles.z;
    DebugUtil.DrawBox2D(transform.position, new Vector2(dimensionX, dimensionY), rotation, Color.white);
  }

}
