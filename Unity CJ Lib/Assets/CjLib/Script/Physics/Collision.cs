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
  // push vector separates A from B
  public class Collision
  {
    public static bool SphereSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB)
    {
      Vector3 vec = centerA - centerB;
      float dd = vec.sqrMagnitude;
      float r = radiusA + radiusB;

      return dd <= r * r;
    }

    public static bool SphereSphere(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB, out Vector3 push)
    {
      push = Vector3.zero;

      Vector3 vec = centerA - centerB;
      float dd = vec.sqrMagnitude;
      float r = radiusA + radiusB;

      if (dd > r * r)
      {
        return false;
      }

      float d = Mathf.Sqrt(dd);

      push = VectorUtil.NormalizeSafe(vec, Vector3.zero) * (r - d);
      return true;
    }

    public static bool SphereCapsule(Vector3 centerA, float radiusA, Vector3 headB, Vector3 tailB, float radiusB)
    {
      Vector3 segVec = tailB - headB;
      float segLenSqr = segVec.sqrMagnitude;
      if (segLenSqr < MathUtil.Epsilon)
        return SphereSphere(centerA, radiusA, 0.5f * (headB + tailB), radiusB);

      float segLenInv = 1.0f / Mathf.Sqrt(segLenSqr);
      Vector3 segDir = segVec * segLenInv;
      Vector3 headToA = centerA - headB;
      float t = Mathf.Clamp01(Vector3.Dot(headToA, segDir) * segLenInv);
      Vector3 closestB = Vector3.Lerp(headB, tailB, t);

      return SphereSphere(centerA, radiusA, closestB, radiusB);
    }

    public static bool SphereCapsule(Vector3 centerA, float radiusA, Vector3 headB, Vector3 tailB, float radiusB, out Vector3 push)
    {
      push = Vector3.zero;

      Vector3 segVec = tailB - headB;
      float segLenSqr = segVec.sqrMagnitude;
      if (segLenSqr < MathUtil.Epsilon)
        return SphereSphere(centerA, radiusA, 0.5f * (headB + tailB), radiusB, out push);

      float segLenInv = 1.0f / Mathf.Sqrt(segLenSqr);
      Vector3 segDir = segVec * segLenInv;
      Vector3 headToA = centerA - headB;
      float t = Mathf.Clamp01(Vector3.Dot(headToA, segDir) * segLenInv);
      Vector3 closestB = Vector3.Lerp(headB, tailB, t);

      return SphereSphere(centerA, radiusA, closestB, radiusB, out push);
    }

    public static bool SphereBox(Vector3 centerOffsetA, float radiusA, Vector3 halfExtentB)
    {
      Vector3 closestOnB = 
        new Vector3
        (
          Mathf.Clamp(centerOffsetA.x, -halfExtentB.x, halfExtentB.x), 
          Mathf.Clamp(centerOffsetA.y, -halfExtentB.y, halfExtentB.y), 
          Mathf.Clamp(centerOffsetA.z, -halfExtentB.z, halfExtentB.z)
        );

      Vector3 vec = centerOffsetA - closestOnB;
      float dd = vec.sqrMagnitude;
      
      return dd <= radiusA * radiusA;
    }

    public static bool SphereBox(Vector3 centerOffsetA, float radiusA, Vector3 halfExtentB, out Vector3 push)
    {
      push = Vector3.zero;

      Vector3 closestOnB = 
        new Vector3
        (
          Mathf.Clamp(centerOffsetA.x, -halfExtentB.x, halfExtentB.x), 
          Mathf.Clamp(centerOffsetA.y, -halfExtentB.y, halfExtentB.y), 
          Mathf.Clamp(centerOffsetA.z, -halfExtentB.z, halfExtentB.z)
        );

      Vector3 vec = centerOffsetA - closestOnB;
      float dd = vec.sqrMagnitude;
      
      if (dd > radiusA * radiusA)
      {
        return false;
      }

      int numInBoxAxes = 
          ((centerOffsetA.x < -halfExtentB.x || centerOffsetA.x > halfExtentB.x) ? 0 : 1)
        + ((centerOffsetA.y < -halfExtentB.y || centerOffsetA.y > halfExtentB.y) ? 0 : 1)
        + ((centerOffsetA.z < -halfExtentB.z || centerOffsetA.z > halfExtentB.z) ? 0 : 1);

      switch (numInBoxAxes)
      {
      case 0: // hit corner
      case 1: // hit edge
      case 2: // hit face
        {
          push = VectorUtil.NormalizeSafe(vec, Vector3.right) * (radiusA - Mathf.Sqrt(dd));
        }
        break;
      case 3: // inside
        {
          Vector3 penetration = 
            new Vector3
            (
              halfExtentB.x - Mathf.Abs(centerOffsetA.x) + radiusA, 
              halfExtentB.y - Mathf.Abs(centerOffsetA.y) + radiusA, 
              halfExtentB.z - Mathf.Abs(centerOffsetA.z) + radiusA
            );

          if (penetration.x < penetration.y)
          {
            if (penetration.x < penetration.z)
              push = new Vector3(Mathf.Sign(centerOffsetA.x) * penetration.x, 0.0f, 0.0f);
            else
              push = new Vector3(0.0f, 0.0f, Mathf.Sign(centerOffsetA.z) * penetration.z);
          }
          else
          {
            if (penetration.y < penetration.z)
                push = new Vector3(0.0f, Mathf.Sign(centerOffsetA.y) * penetration.y, 0.0f);
            else
                push = new Vector3(0.0f, 0.0f, Mathf.Sign(centerOffsetA.z) * penetration.z);
          }
        }
        break;
      }

      return true;
    }
  }
}
