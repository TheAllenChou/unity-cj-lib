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
  public class MathUtil
  {

    public static readonly float kPi      = Mathf.PI;
    public static readonly float kTwoPi   = 2.0f * Mathf.PI;
    public static readonly float kHalfPi  = Mathf.PI / 2.0f;
    public static readonly float kThirdPi = Mathf.PI / 3.0f;

    public static readonly float kSqrt2    = Mathf.Sqrt(2.0f);
    public static readonly float kSqrt2Inv = 1.0f / Mathf.Sqrt(2.0f);
    public static readonly float kSqrt3    = Mathf.Sqrt(3.0f);
    public static readonly float kSqrt3Inv = 1.0f / Mathf.Sqrt(3.0f);


    public static readonly float kEpsilon = 1.0e-16f;
    public static readonly float kRad2Deg = 180.0f / Mathf.PI;
    public static readonly float kDeg2Rad = Mathf.PI / 180.0f;

    public static float AsinSafe(float x)
    {
      return Mathf.Asin(Mathf.Clamp(x, -1.0f, 1.0f));
    }

    public static float AcosSafe(float x)
    {
      return Mathf.Acos(Mathf.Clamp(x, -1.0f, 1.0f));
    }

  }
}
