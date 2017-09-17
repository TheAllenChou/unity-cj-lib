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

public class SphereComponent : CjLibDemoComponent
{
  [Range(0.1f, 10.0f)]
  public float radius = 1.0f;

  [Range(1, 64)]
  public int latSegments = 8;

  [Range(2, 128)]
  public int longSegments = 16;

  protected override void Draw()
  {
    DebugUtil.DrawSphere(transform.position, transform.rotation, radius, latSegments, longSegments, Color.white);
  }

}
