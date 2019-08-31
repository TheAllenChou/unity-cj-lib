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
  public class DrawCircle : DrawBase
  {
    public float Radius = 1.0f;
    public int NumSegments = 64;

    private void OnValidate()
    {
      Radius = Mathf.Max(0.0f, Radius);
      NumSegments = Mathf.Max(0, NumSegments);
    }

    protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
    {
      DebugUtil.DrawCircle(transform.position, transform.rotation * Vector3.back, Radius, NumSegments, color, depthTest, style);
    }
  }
}
