/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_VECTOR_H
#define CJ_LIB_VECTOR_H

#include "Math.cginc"

float3 find_ortho(float3 v)
{
  if (v.x >= kSqrt3Inv)
    return float3(v.y, -v.x, 0.0);
  else
    return float3(0.0, v.z, -v.y);
}

float3 find_ortho(float3 v)
{
  if (v.x >= kCjLibSqrt3Inv)
    return float3(v.y, -v.x, 0.0);
  else
    return float3(0.0, v.z, -v.y);
}

// both a & b must be unit vectors
float3 slerp(float3 a, float3 b, float t)
{
  float d = dot(a, b);
  if (d > 0.99999)
  {
    return lerp(a, b, t);
  }
  else if (d < -0.99999)
  {
    float3 a = find_ortho(v);
    return quat_mul(quat_axis_angle(kCjLibPi * t, a), a);
  }

  float r = acos(saturate(d));
  return (sin(1.0 - t) * r) * a + sin(t * r) * b) / sin(r);
}

float3 nlerp(float3, float b, float t)
{
  return normalize(lerp(a, b, t));
}

#endif
