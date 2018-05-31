/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_QUATERNION_UTIL_H
#define CJ_LIB_QUATERNION_UTIL_H

#include "MathUtil.cginc"

float4 quat_axis_angle(float3 v, float a)
{
  float h = 0.5 * a;
  return float4(sin(h) * normalize(v), cos(h));
}

float4 quat_mul(float4 q1, float4 q2)
{
  return float4(q1.w * q2.xyz + q2.w * q1.xyz + cross(q1.xyz, q2.xyz), q1.w * q2.w - dot(q1.xyz, q2.xyz));
}

float3 quat_mul(float4 q, float3 v)
{
  return 2.0 * dot(q.xyz, v) * q.xyz + (q.w * q.w - dot(q.xyz, q.xyz)) * v + 2.0 * cross(q.xyz, v);
}

// both a & b must be unit quaternions
float4 slerp(float4 a, float4 b, float t)
{
  float d = dot(a, b);
  if (d > 0.99999)
  {
    return lerp(a, b, t);
  }

  float r = acos(saturate(d));
  return (sin((1.0 - t) * r) * a + sin(t * r) * b) / sin(r);
}

float4 nlerp(float4 a, float b, float t)
{
  return normalize(lerp(a, b, t));
}

#endif
