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
  public class DrawArrow : DrawBase
  {
    public Vector3 LocalEndVector = Vector3.right;

    public float ConeRadius = 0.05f;
    public float ConeHeight = 0.1f;
    public float StemThickness = 0.05f;
    public int NumSegments = 8;

    private void OnValidate()
    {
      ConeRadius = Mathf.Max(0.0f, ConeRadius);
      ConeHeight = Mathf.Max(0.0f, ConeHeight);
      StemThickness = Mathf.Max(0.0f, StemThickness);
      NumSegments = Mathf.Max(4, NumSegments);
    }

    protected override void Draw(Color color, DebugUtil.Style style, bool depthTest)
    {
      DebugUtil.DrawArrow
      (
        transform.position, 
        transform.position + transform.TransformVector(LocalEndVector), 
        ConeRadius, ConeHeight, NumSegments, StemThickness, 
        color, depthTest, style
      );
    }
  }
}
