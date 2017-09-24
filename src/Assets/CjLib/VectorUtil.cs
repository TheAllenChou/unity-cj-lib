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
  public class VectorUtil
  {
   
    public static Vector3 Rotate2D(Vector3 vector, float rotationDeg)
    {
      Vector3 results = vector;
      float cos = Mathf.Cos(MathUtil.kDeg2Rad * rotationDeg);
      float sin = Mathf.Sin(MathUtil.kDeg2Rad * rotationDeg);
      results.x = cos * vector.x - sin * vector.y;
      results.y = sin * vector.x + cos * vector.y;
      return results;
    }

  }
}
