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
  public class DrawLine : DrawBase
  {
    public Vector3 LocalEndVector = Vector3.right;

    private void OnValidate()
    {
      Wireframe = true;
      Style = DebugUtil.Style.Wireframe;
    }

    protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
    {
      DebugUtil.DrawLine
      (
        transform.position, 
        transform.position + transform.TransformVector(LocalEndVector), 
        color, depthTest
      );
    }
  }
}
