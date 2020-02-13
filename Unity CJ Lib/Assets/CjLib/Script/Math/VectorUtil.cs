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
   
    public static Vector3 Rotate2D(Vector3 v, float deg)
    {
      Vector3 results = v;
      float cos = Mathf.Cos(MathUtil.Deg2Rad * deg);
      float sin = Mathf.Sin(MathUtil.Deg2Rad * deg);
      results.x = cos * v.x - sin * v.y;
      results.y = sin * v.x + cos * v.y;
      return results;
    }
    
    public static Vector3 NormalizeSafe(Vector3 v, Vector3 fallback)
    {
      return 
        v.sqrMagnitude > MathUtil.Epsilon 
          ? v.normalized 
          : fallback;
    }

    // Returns a vector orthogonal to given vector.
    // If the given vector is a unit vector, the returned vector will also be a unit vector.
    public static Vector3 FindOrthogonal(Vector3 v)
    {
      if (Mathf.Abs(v.x) >= MathUtil.Sqrt3Inv)
        return Vector3.Normalize(new Vector3(v.y, -v.x, 0.0f));
      else
        return Vector3.Normalize(new Vector3(0.0f, v.z, -v.y));
    }

    // Yields two extra vectors that form an orthogonal basis with the given vector.
    // If the given vector is a unit vector, the returned vectors will also be unit vectors.
    public static void FormOrthogonalBasis(Vector3 v, out Vector3 a, out Vector3 b)
    {
      a = FindOrthogonal(v);
      b = Vector3.Cross(a, v);
    }

    public static Vector3 Integrate(Vector3 x, Vector3 v, float dt)
    {
      return x + v * dt;
    }

    // Both vectors must be unit vectors.
    public static Vector3 Slerp(Vector3 a, Vector3 b, float t)
    {
      float dot = Vector3.Dot(a, b);

      if (dot > 0.99999f)
      {
        // singularity: two vectors point in the same direction
        return Vector3.Lerp(a, b, t);
      }
      else if (dot < -0.99999f)
      {
        // singularity: two vectors point in the opposite direction
        Vector3 axis = FindOrthogonal(a);
        return Quaternion.AngleAxis(180.0f * t, axis) * a;
      }

      float rad = MathUtil.AcosSafe(dot);
      return (Mathf.Sin((1.0f - t) * rad) * a + Mathf.Sin(t * rad) * b) / Mathf.Sin(rad);
    }

    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
      float tt = t * t;
      return
        0.5f
        * ((2.0f * p1)
          + (-p0 + p2) * t
          + (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * tt
          + (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * tt * t
          );
    }

  }
}
