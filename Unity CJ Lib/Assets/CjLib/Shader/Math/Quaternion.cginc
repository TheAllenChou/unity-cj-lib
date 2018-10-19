/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_QUATERNION
#define CJ_LIB_QUATERNION

#include "Math.cginc"

inline float4 quat_identity()
{
  return float4(0.0f, 0.0f, 0.0f, 1.0f);
}

inline float4 quat_conj(float4 q)
{
  return float4(-q.xyz, q.w);
}

// q must be unit quaternion
inline float4 quat_pow(float4 q, float p)
{
  float r = length(q.xyz);
  if (r < kEpsilon)
    return quat_identity();

  float t = p * atan2(q.w, r);

  return float4(sin(t) * q.xyz / r, cos(t));
}

inline float4 quat_axis_angle(float3 v, float a)
{
  float h = 0.5 * a;
  return float4(sin(h) * normalize(v), cos(h));
}

inline float4 quat_from_to(float3 from, float3 to)
{
  float3 c = cross(from, to);
  float cc = dot(c, c);

  if (cc < kEpsilon)
    return quat_identity();

  float3 axis = c / sqrt(cc);
  float angle = acos(clamp(dot(from, to), -1.0f, 1.0f));
  return quat_axis_angle(axis, angle);
}

inline float3 quat_get_axis(float4 q)
{
  float d = dot(q.xyz, q.xyz);
  return 
    d > kEpsilon 
    ? q.xyz / sqrt(d)
    : float3(0.0f, 0.0f, 1.0f);
}

inline float3 quat_get_angle(float4 q)
{
  return 2.0f * acos(clamp(q.w, -1.0f, 1.0f));
}

inline float4 quat_concat(float4 q1, float4 q2)
{
  return 
    float4
    (
      q1.w * q2.xyz + q2.w * q1.xyz + cross(q1.xyz, q2.xyz), 
      q1.w * q2.w - dot(q1.xyz, q2.xyz)
    );
}

inline float3 quat_rot(float4 q, float3 v)
{
  return
    dot(q.xyz, v) * q.xyz
    + q.w * q.w * v
    + 2.0 * q.w * cross(q.xyz, v)
    - cross(cross(q.xyz, v), q.xyz);
}

inline float4 quat_look_at(float3 front, float3 up)
{
  float4 quatA = quat_from_to(float3(0.0f, 0.0f, 0.0f), front);
  float3 upA = quat_rot(quatA, up);
  float4 quatB = quat_from_to(upA, up);
  return quat_concat(quatB, quatA);
}

inline float4 slerp(float4 a, float4 b, float t)
{
  float d = dot(normalize(a), normalize(b));
  if (d > kEpsilonComp)
  {
    return lerp(a, b, t);
  }

  float r = acos(clamp(d, -1.0f, 1.0f));
  return (sin((1.0 - t) * r) * a + sin(t * r) * b) / sin(r);
}

inline float4 nlerp(float4 a, float b, float t)
{
  return normalize(lerp(a, b, t));
}

#endif
