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
  public class QuaternionUtil
  {
   
    public static float Magnitude(Quaternion q)
    {
      return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
    }

    public static float MagnitudeSqr(Quaternion q)
    {
      return q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
    }

    public static Quaternion Normalize(Quaternion q)
    {
      float magInv = 1.0f / Magnitude(q);
      return new Quaternion(q.x * magInv, q.y * magInv, q.z * magInv, q.w * magInv);
    }

    public static Quaternion NormalizeSafe(Quaternion q, Quaternion fallback)
    {
      float mag = Magnitude(q);
      if (mag > MathUtil.Epsilon)
      {
        float magInv = 1.0f / mag;
        return new Quaternion(q.x * magInv, q.y * magInv, q.z * magInv, q.w * magInv);
      }
      else
      {
        return fallback;
      }
    }

    public static void DecomopseSwingTwist(Quaternion q, Vector3 twistAxis, out Quaternion swing, out Quaternion twist)
    {
      Vector3 r = new Vector3(q.x, q.y, q.z); // (rotaiton axis) * cos(angle / 2)

      // singularity: rotation by 180 degree
      if (r.sqrMagnitude < MathUtil.Epsilon)
      {
        Vector3 rotatedTwistAxis = q * twistAxis;
        Vector3 swingAxis = Vector3.Cross(twistAxis, rotatedTwistAxis);

        if (swingAxis.sqrMagnitude > MathUtil.Epsilon)
        {
          float swingAngle = Vector3.Angle(twistAxis, rotatedTwistAxis);
          swing = Quaternion.AngleAxis(swingAngle, swingAxis);
        }
        else
        {
          // more singularity: rotation axis parallel to twist axis
          swing = Quaternion.identity; // no swing
        }

        // always twist 180 degree on singularity
        twist = Quaternion.AngleAxis(180.0f, twistAxis);
        return;
      }

      // formula & proof: 
      // http://www.euclideanspace.com/maths/geometry/rotations/for/decomposition/
      Vector3 p = Vector3.Project(r, twistAxis);
      twist = new Quaternion(p.x, p.y, p.z, q.w);
      twist = Normalize(twist);
      swing = q * Quaternion.Inverse(twist);
    }

    // swing-twist interpolation
    public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t)
    {
      Quaternion swing;
      Quaternion twist;
      return Sterp(a, b, twistAxis, t, out swing, out twist);
    }
    public static Quaternion Sterp(Quaternion a, Quaternion b, Vector3 twistAxis, float t, out Quaternion swing, out Quaternion twist)
    {
      Quaternion q = b * Quaternion.Inverse(a);
      Quaternion swingFull;
      Quaternion twistFull;
      QuaternionUtil.DecomopseSwingTwist(q, twistAxis, out swingFull, out twistFull);

      swing = Quaternion.Slerp(Quaternion.identity, swingFull, t);
      twist = Quaternion.Slerp(Quaternion.identity, twistFull, t);

      return twist * swing;
    }

  }
}
