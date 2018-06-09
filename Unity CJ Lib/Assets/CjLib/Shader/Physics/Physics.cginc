/******************************************************************************/
/*
  Project - Unity CJ Lib
            https://github.com/TheAllenChou/unity-cj-lib
  
  Author  - Ming-Lun "Allen" Chou
  Web     - http://AllenChou.net
  Twitter - @TheAllenChou
*/
/******************************************************************************/

#ifndef CJ_LIB_PHYSICS
#define CJ_LIB_PHYSICS


#include "../Math/Math.cginc"


// common
//-----------------------------------------------------------------------------

struct CollisionResult
{
  float3 position;
  float3 velocity;
};

inline float3 bounce(float3 i, float3 n, float restitution)
{
  return -(1.0 + restitution) * dot(i, n) * n;
}

inline float3 bounce(float3 i, float3 n, float restitution, float friction)
{
  float d = dot(i, n);
  float f = -d / length(i);
  float3 perp = d * n;
  float3 para = i - perp;
  return -(1.0 + restitution) * perp - friction * f * para;
}

inline CollisionResult resolveCollision(float3 pos, float3 norm, float3 vel, float penetrationDepth, float restitution)
{
  CollisionResult res;
  res.position = pos + penetrationDepth * norm;
  res.velocity = vel + step(kEpsilon, penetrationDepth) * bounce(vel, norm, restitution);
  return res;
}

inline CollisionResult resolveCollision(float3 pos, float3 norm, float3 vel, float penetrationDepth, float restitution, float friction)
{
  CollisionResult res;
  res.position = pos + penetrationDepth * norm;
  res.velocity = vel + step(kEpsilon, penetrationDepth) * bounce(vel, norm, restitution, friction);
  return res;
}

//-----------------------------------------------------------------------------
// end: common


// VS plane
//-----------------------------------------------------------------------------

inline float3 pointVsPlane(float3 p, float4 plane)
{
  float d = max(0.0, -dot(float4(p, 1.0), plane));
  float3 n = plane.xyz;
  return p + d * n;
}

inline CollisionResult pointVsPlane(float3 p, float4 plane, float3 vel, float restitution)
{
  CollisionResult res;
  float d = max(0.0, -dot(float4(p, 1.0), plane));
  float3 n = plane.xyz;
  return resolveCollision(p, n, vel, d, restitution);
}

inline CollisionResult pointVsPlane(float3 p, float4 plane, float3 vel, float restitution, float friction)
{
  float d = max(0.0, -dot(float4(p, 1.0), plane));
  float3 n = plane.xyz;
  return resolveCollision(p, n, vel, d, restitution, friction);
}

inline float3 sphereVsPlane(float4 s, float4 plane)
{
  float d = max(0.0, s.w - dot(float4(s.xyz, 1.0), plane));
  float3 n = plane.xyz;
  return s.xyz + d * n;
}

inline CollisionResult sphereVsPlane(float4 s, float4 plane, float3 vel, float restitution)
{
  float d = max(0.0, s.w - dot(float4(s.xyz, 1.0), plane));
  float3 n = plane.xyz;
  return resolveCollision(s.xyz, n, vel, d, restitution);
}

inline CollisionResult sphereVsPlane(float4 s, float4 plane, float3 vel, float restitution, float friction)
{
  float d = max(0.0, s.w - dot(float4(s.xyz, 1.0), plane));
  float3 n = plane.xyz;
  return resolveCollision(s.xyz, n, vel, d, restitution, friction);
}

//-----------------------------------------------------------------------------
// end: VS plane


// VS sphere
//-----------------------------------------------------------------------------

inline float3 pointVsSphere(float3 p, float4 sphere)
{
  float3 r = p - sphere.xyz;
  float d = max(0.0, sphere.w - length(r));
  float3 n = normalize(r);
  return p + d * n;
}

inline CollisionResult pointVsSphere(float3 p, float4 sphere, float3 vel, float restitution)
{
  float3 r = p - sphere.xyz;
  float d = min(0.0, sphere.w - length(r));
  float3 n = normalize(r);
  return resolveCollision(p, n, vel, d, restitution);
}

inline CollisionResult pointVsSphere(float3 p, float4 sphere, float3 vel, float restitution, float friction)
{
  float3 r = p - sphere.xyz;
  float d = max(0.0, sphere.w - length(r));
  float3 n = normalize(r);
  return resolveCollision(p, n, vel, d, restitution, friction);
}

inline float3 sphereVsSphere(float4 s, float4 sphere)
{
  float3 r = s.xyz - sphere.xyz;
  float d = max(0.0, s.w + sphere.w - length(r));
  float3 n = normalize(r);
  return s.xyz + d * n;
}

inline CollisionResult sphereVsSphere(float4 s, float4 sphere, float3 vel, float restitution)
{
  float3 r = s.xyz - sphere.xyz;
  float d = max(0.0, s.w + sphere.w - length(r));
  float3 n = normalize(r);
  return resolveCollision(s.xyz, n, vel, d, restitution);
}

inline CollisionResult sphereVsSphere(float4 s, float4 sphere, float3 vel, float restitution, float friction)
{
  float3 r = s.xyz - sphere.xyz;
  float d = max(0.0, s.w + sphere.w - length(r));
  float3 n = normalize(r);
  return resolveCollision(s.xyz, n, vel, d, restitution, friction);
}

//-----------------------------------------------------------------------------
// end: VS sphere

#endif
